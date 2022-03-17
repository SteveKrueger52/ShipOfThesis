using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class Sailboat : MonoBehaviour , WindZone.IWindObject
{
    [Serializable]
    public class SeekEasingValues
    {
        public float MaxDelta;
        public float InnerThresh;
        public float OuterThresh;
    }
    
    #region Public Members

    private bool simpleControls;
    public bool SimpleControls
    {
        get => simpleControls;
        set {
            simpleControls = value;
            input.SwitchCurrentActionMap(simpleControls ? "Simple" : "Complex");
        }
    }

    public Transform windIndicator;
    [Space]
    
    [Header("Physics Constraints")]
    public float maxSpeed;
    public float decayRate;
    public float rudderImpulse;
    public float boatRotateSpeed;
    public float sailRotateSpeed; // tuned for 10 knots, full sail. Scales accordingly.
    public float mainsailMinimum;

    [Space]

    public SeekEasingValues HalyardEasings;
    public SeekEasingValues HeadingEasings;
    public SeekEasingValues MainsheetEasings;
    
    [Space] 

    [Header("Sailing Curves")]
    // All Animation Curves are in the range of X,Y = [0,1].
    // For angles, this translates to 0 = 0 and 180 = 1 (wind) and 90 = 1 (sail), with assumed symmetry for negative angles.
    // Furthermore, there will be exactly 1 X value for every Y value and vice versa,
    // even if this cannot be enforced in code.

    // The three graphed lines will approximate this chart of projected speeds by boat heading
    // https://physicstoday.scitation.org/na101/home/literatum/publisher/aip/journals/content/pto/2008/pto.2008.61.issue-2/1.2883908/production/images/medium/1.2883908.figures.f5.gif
    // The Physics of Sailing, Physics Today 61, 2, 38 (2008); https://doi.org/10.1063/1.2883908
    public AnimationCurve Knots5;
    public AnimationCurve Knots10, Knots20;
    [Space] 
    
    // Used to approximate the best sail angle at a given point of sail, for wind effect drop off
    // Saoud, Hadi & Hua, Minh-Duc & Plumet, Frederic & Amar, Faïz. (2015). Optimal sail angle computation for an
    // autonomous sailboat robot. 10.1109/CDC.2015.7402329. 
    public AnimationCurve OptimalSailAngle; // From Apparent Wind angle 
    
    public AnimationCurve SailAngleFalloff; // The decay of effectiveness as sail angle leaves the optimal angle.
    
    #endregion

    #region Private Members

    private float mpsToKnots = 1.94384f; // Meters per second to knots
    private List<WindZone> windZones;
    private Vector3 currentWind;
    private Rigidbody rb;
    private Animator anim;
    private Camera mainCam;

    // Input
    private PlayerInput input;
    
    private bool halyardToggleUp;
    private float halyardDelta;
    private float sailAngleDelta;
    private Vector3 steerTarget;
    private float rudderInput;
    
    // Animation & Physics Values
    private float sailHeight; 
    private float sailAngle;
    private float sailAngleLocalMax;
    private float rudderAngle;
    private float sailEffect;
    private float lastRudderAngle;

    #endregion
    
    
     private void Awake()
     {
         rb = GetComponent<Rigidbody>();
         anim = GetComponent<Animator>();
         input = GetComponent<PlayerInput>();
         windZones = new List<WindZone>();
         mainCam = Camera.main;
         
         SimpleControls = false;
         currentWind = AverageWind();
     }

     private void Update()
     {
         // Wind Particles
         windIndicator.rotation = Quaternion.LookRotation(currentWind, Vector3.up);
         
         // Apparent Wind
         Vector3 apparentWind = currentWind - rb.velocity;
         float windAngle = Vector3.SignedAngle(transform.right,apparentWind, Vector3.up); // transform.right is the stern of the boat
         
         
         // Get Steering - Sail height, Rudder angle, and Mainsheet max (from centerline)
         if (SimpleControls) 
             SimpleSteering(Mathf.Abs(windAngle));
         else ComplexSteering();
         
         // Move sail toward wind angle - constrained by mainsheet
         sailAngle = Mathf.MoveTowardsAngle(sailAngle * 90, windAngle, Time.deltaTime * sailRotateSpeed * sailHeight * (currentWind.magnitude / 10)) / 90; 
         sailAngle = Mathf.Abs(sailAngle) > sailAngleLocalMax ? sailAngleLocalMax * Mathf.Sign(sailAngle) : sailAngle;

         // Update Animations
         UpdateAnimation();
     }

     // TODO collapse into readable helper functions
    private void FixedUpdate()
    {
       // Determine Constants for frame
       Vector3 flatVelocity = rb.velocity.ProjectOntoPlane(Vector3.up);
       Vector3 trueWind = currentWind.ProjectOntoPlane(Vector3.up);
       Vector3 apparentWind = trueWind - flatVelocity;
       Vector3 heading = -transform.right.ProjectOntoPlane(Vector3.up); // -transform.right is the bow of the boat

       float windAngle = Vector3.SignedAngle(-heading, apparentWind, Vector3.up);
       float optimalAngle = OptimalSailAngle.Evaluate(Mathf.Abs(windAngle) / 180f) * Mathf.Sign(windAngle) * 90f;
       
       // Determine max speed from wind angle and 'optimal' heading angle - lerp by wind speed across curves
       float knots = trueWind.magnitude;
       float headingFromWind = Vector3.Angle(-heading, trueWind)/180;
       float frameSpeed;
       
       if (knots <= 5f) 
           frameSpeed = Mathf.Lerp(0f, Knots5.Evaluate(headingFromWind), knots / 5f);
       else if (5f < knots && knots <= 10f)
           frameSpeed = Mathf.Lerp(Knots5.Evaluate(headingFromWind), Knots10.Evaluate(headingFromWind), (knots - 5f) / 5f);
       else if (10f < knots && knots < 20f)
           frameSpeed = Mathf.Lerp(Knots10.Evaluate(headingFromWind), Knots20.Evaluate(headingFromWind), (knots - 10f) / 10f);
       else frameSpeed = Knots20.Evaluate(headingFromWind);

       
       // Adjust for falloff based on difference between Sail Angle and Optimal Angle, based on proximity to wind
       float reflected = ReflectAngle(windAngle, sailAngle * 90f);
       float degreesFromOptimal = Mathf.DeltaAngle(optimalAngle, (sailAngle * 90f));
       float reflectedFromOptimal = Mathf.DeltaAngle(optimalAngle, reflected);
       
       float effectAngle = Mathf.Abs(degreesFromOptimal) < Mathf.Abs(reflectedFromOptimal) ? sailAngle * 90f : reflected;
       
       // Get T position of the sail (or its reflection) Lerping from the optimal angle to the wind axis.
       float effect = Mathf.Min(Mathf.Abs(degreesFromOptimal), Mathf.Abs(reflectedFromOptimal)) / 
           Mathf.Abs(Mathf.DeltaAngle(optimalAngle, Mathf.Abs(effectAngle) > Mathf.Abs(optimalAngle) ? windAngle : windAngle + 180f));

       sailEffect = SailAngleFalloff.Evaluate(effect) * frameSpeed * sailHeight;
       
       
       
       // Capture old speed for momentum preservation
       Vector3 oldVel = rb.velocity.ProjectOntoPlane(Vector3.up);
       
       rb.AddTorque(new Vector3(0f, boatRotateSpeed * -rudderAngle * Mathf.Clamp01(rb.velocity.magnitude), 0f), ForceMode.VelocityChange);

       // Zero out prior velocity
       rb.AddForce(-oldVel, ForceMode.VelocityChange);
       rb.AddForce(-transform.right.ProjectOntoPlane(Vector3.up) * 
                   Mathf.Max(oldVel.magnitude * (1-(Time.fixedDeltaTime * decayRate)), sailEffect * maxSpeed), ForceMode.VelocityChange);

       // Rudder Impulse
       if (!Mathf.Approximately(rudderAngle, lastRudderAngle))
           rb.AddForce(-transform.right * rudderImpulse, ForceMode.Acceleration);
       lastRudderAngle = rudderAngle;
    }
    

    #region Interface Methods

    public void EnterWind(WindZone wind)
    {
        Debug.Log("Entered Wind");
        windZones.Add(wind);
        currentWind = AverageWind();
    }

    public void ExitWind(WindZone wind)
    {
        Debug.Log("Left Wind");
        windZones.Remove(wind);
        currentWind = AverageWind();
    }
    
    #endregion
    
    #region Input Management
    
    public void OnToggleHalyard()
    {
        //Debug.Log("Toggle Halyard");
        halyardToggleUp = !halyardToggleUp;
    }
    
    public void OnGetHalyard(InputValue value)
    {
        
        // Debug.Log("Move Halyard");
        halyardDelta = value.Get<float>();
        //Debug.Log("Halyard Delta: " + halyardDelta);

    }

    public void OnGetSteering(InputValue value)
    {
        // Debug.Log("Get Steering");
        var inputSteering = value.Get<Vector2>();
        steerTarget = (mainCam.transform.right * inputSteering.x + mainCam.transform.forward * inputSteering.y)
            .ProjectOntoPlane(Vector3.up).normalized;
        //Debug.Log("Steering: " + steerTarget);

    }
    
    public void OnGetRudder(InputValue value)
    {
        // Debug.Log("Move Rudder");
        rudderInput = value.Get<float>();
        //Debug.Log("Rudder Input: " + rudderInput);

    }
    
    public void OnGetMainsheet(InputValue value)
    {
        // Debug.Log("Move Mainsheet");
        sailAngleDelta = value.Get<float>();
        //Debug.Log("Mainsheet Input: " + sailAngleDelta);
    }

    public void OnSwapControls()
    {
        SimpleControls = !simpleControls;
    }

    public void OnDebugInfo()
    {
        Debug.Log("Sail Angle: " + sailAngle + " | Rudder Angle: " + rudderAngle + " | Mainsail Height: " + sailHeight + "\n" +
                  "Simple: " + simpleControls + " | HalyardToggle: " + halyardToggleUp + " | Mainsheet: " + sailAngleLocalMax);
    }

    #endregion

    #region Helper Methods

    private void UpdateAnimation()
    {
        anim.SetFloat("SailHeight", sailHeight);
        anim.SetFloat("SailAngle", sailAngle/2 + 0.5f);
        anim.SetFloat("RudderAngle", rudderAngle/2 + 0.5f);
        anim.SetFloat("SailEffect", sailEffect);
    }

    private void SimpleSteering(float windAngle)
    {
        // Halyard Toggle
        float debug = 0f;
        if ( halyardToggleUp && !Mathf.Approximately(sailHeight, 1f)) 
            sailHeight = SeekEase(sailHeight, 1f, HalyardEasings, out debug);
        if (!halyardToggleUp && !Mathf.Approximately(sailHeight, 0f)) 
            sailHeight = SeekEase(sailHeight, 0f, HalyardEasings, out debug);
        
        // Rudder from Target Heading
        float angleToTarget = Vector3.SignedAngle(-transform.right, steerTarget, Vector3.up); // -transform.right is the bow of the boat
        if (steerTarget != Vector3.zero && !Mathf.Approximately(0f, angleToTarget))
        {
            float effort;
            SeekEase(0f, angleToTarget, HeadingEasings, out effort);
            rudderAngle = -effort;
        }
        else rudderAngle = 0f;
        
        // Mainsheet from optimal angle for current heading
        sailAngleLocalMax = Mathf.Max(mainsailMinimum,OptimalSailAngle.Evaluate(windAngle / 180f));

        // Debug.Log(debug);
        Debug.Log(halyardToggleUp + ": " + sailHeight + " | " + angleToTarget + " | " + steerTarget);
    }

    private void ComplexSteering()
    {
        // Halyard Height - delta control
        sailHeight = Mathf.Clamp01(sailHeight + (halyardDelta * Time.deltaTime * HalyardEasings.MaxDelta));
        
        // Rudder - direct control
        rudderAngle = rudderInput;
        
        // Mainsheet - delta control
        sailAngleLocalMax = Mathf.Clamp(sailAngleLocalMax + (sailAngleDelta * Time.deltaTime * MainsheetEasings.MaxDelta),mainsailMinimum,1f);
    }

    private Vector3 AverageWind ()
    {
        if (windZones.Count == 0) return Vector3.zero;

        Vector3 acc = Vector3.zero;
        foreach (WindZone w in windZones)
            acc += w.WindVector;
        //Debug.Log("Wind Zones: " + windZones.Count + " | avg: " + (acc/windZones.Count));
        return  acc / windZones.Count;
    }

    private float ReflectAngle(float normal, float incident)
    {
        return AngleWrap(normal - (incident - normal));
    }

    public float AngleWrap(float angle)
    {
        return Mathf.DeltaAngle(0, angle);
    }
    
    private float SeekEase(float origin, float target, SeekEasingValues easings)
    {
        return SeekEase(origin, target, easings.MaxDelta, easings.OuterThresh, easings.InnerThresh, out _);
    }
    private float SeekEase(float origin, float target, float maxDelta, float outerThresh, float innerThresh)
    {
        return SeekEase(origin, target, maxDelta, outerThresh, innerThresh, out _);
    }
    private float SeekEase(float origin, float target, SeekEasingValues easings, out float effort)
    {
        return SeekEase(origin, target, easings.MaxDelta, easings.OuterThresh, easings.InnerThresh, out effort);
    }
    private float SeekEase(float origin, float target, float maxDelta, float outerThresh, float innerThresh, out float effort)
    {
        float delta = target - origin;
        float toMove = maxDelta * Mathf.Sign(delta);

        effort = Mathf.Sign(toMove);
        if (Mathf.Abs(delta) > outerThresh) 
            return origin + toMove * Time.deltaTime;
        if (Mathf.Abs(delta) > innerThresh)
        {
            //Debug.Log("Slow Down");
            effort *= (Mathf.Abs(delta) / outerThresh);
            return origin + (toMove * (Mathf.Abs(delta) / outerThresh) * Time.deltaTime);
        }
        effort = 0f;
        return target;
    }

    private float InverseCurve(AnimationCurve curve, float target, int iterations = 10)
    {
        
        Keyframe startFrame = curve[0];
        Keyframe endFrame = curve[curve.length - 1];
        
        // Debug.Log(startFrame.value + " | " + target + " | " + endFrame.value);
        
        // Is curve positive or negative over time
        bool positive = startFrame.value < endFrame.value;
        
        // If target exists outside bounds of curve, clamp to curve
        if ((target > startFrame.value && !positive) || (target < startFrame.value &&  positive)) return startFrame.time;
        if ((target > endFrame.value   &&  positive) || (target < endFrame.value   && !positive)) return endFrame.time;
        
        // Binary search by midpoint along time axis
        float lower = startFrame.time;
        float upper = endFrame.time;
        float mid,midVal;
        for (int i = 0; i < iterations; i++)
        {
            mid = upper + lower / 2;
            midVal = curve.Evaluate(mid);
            if      ((midVal > target && !positive) || (midVal < target &&  positive)) lower = mid; // Target in upper half
            else if ((midVal > target && positive)  || (midVal < target && !positive))  upper = mid; // Target in lower half
            else return midVal; // Goldilocks. Target is exactly midVal
        }
        
        // Out of iterations, return closest midpoint (with 10 iterations, accuracy is within 0.05%)
        return upper + lower / 2;
    }
    
    #endregion

    // #region Debug
    //
    // private void OnDrawGizmos()
    // {
    //     Vector3 flatVelocity = rb != null ? rb.velocity.ProjectOntoPlane(Vector3.up) : Vector3.zero;
    //     Vector3 trueWind = currentWind.ProjectOntoPlane(Vector3.up);
    //     Vector3 apparentWind = trueWind - flatVelocity;
    //     Vector3 heading = -transform.right.ProjectOntoPlane(Vector3.up); // -transform.right is the bow of the boat
    //
    //     float windAngle = Vector3.SignedAngle(-heading, apparentWind, Vector3.up);
    //     float optimalAngle = OptimalSailAngle.Evaluate(Mathf.Abs(windAngle) / 180f) * Mathf.Sign(windAngle) * 90f;
    //     
    //     
    //     // Adjust for falloff based on difference between Sail Angle and Optimal Angle, based on proximity to wind
    //     float reflected = ReflectAngle(windAngle, sailAngle * 90f);
    //     float degreesFromOptimal = Mathf.DeltaAngle(optimalAngle, (sailAngle * 90f));
    //     float reflectedFromOptimal = Mathf.DeltaAngle(optimalAngle, reflected);
    //    
    //     float effectAngle = Mathf.Abs(degreesFromOptimal) < Mathf.Abs(reflectedFromOptimal) ? sailAngle * 90f : reflected;
    //    
    //     // Get T position of the sail (or its reflection) Lerping from the optimal angle to the wind axis.
    //     
    //     Vector3 origin = transform.position;
    //     // Wind Angle
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(origin, origin + currentWind.normalized * 5);
    //     
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(windAngle, Vector3.up) * transform.right) * 5);
    //     
    //     // Optimal Sail Angle
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(optimalAngle, Vector3.up) * transform.right) * 5);
    //     
    //     // Sail and Reflection
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(sailAngle * 90, Vector3.up) * transform.right) * 5);
    //     
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(reflected, Vector3.up) * transform.right) * 5);
    //     
    //     Gizmos.color = Color.white;
    //     Vector3 dir =
    //         Quaternion.AngleAxis(Mathf.Abs(effectAngle) > Mathf.Abs(optimalAngle) ? windAngle : windAngle + 180f, Vector3.up) *
    //         transform.right;
    //     Gizmos.DrawLine(origin + (dir * 5), origin + (dir * 6));
    //     
    //     // Simple Controls - Target Heading
    //     if (simpleControls)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawLine(origin, origin + steerTarget * 5);
    //     }
    // }
    //
    // #endregion
    
}
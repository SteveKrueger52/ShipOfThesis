using System;
using System.Collections.Generic;
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
       float optimalAngle = OptimalSailAngle.Evaluate(Mathf.Abs(windAngle) / 180f) * -Mathf.Sign(windAngle) * 90f;
       
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

       
       // TODO Adjust for falloff based on difference between true and optimal heading
       float reflected = ReflectAngle(windAngle, sailAngle * 90);
       float degreesFromOptimal = optimalAngle - 
           (Mathf.DeltaAngle(sailAngle * 90, optimalAngle) > Mathf.DeltaAngle(reflected, optimalAngle)
           ? reflected : (sailAngle * 90f));
       
       
       Debug.Log("SailAngle: " + (sailAngle * 90f) + " | Optimal: " + optimalAngle * Mathf.Sign(windAngle) + " | Degrees From Optimal: " + degreesFromOptimal);
           
           // "Velocity: " + rb.velocity.ProjectOntoPlane(Vector3.up).magnitude + "  | Wind Angle: " + windAngle + " | Effect : " + sailEffect + " | Frame Speed: " + frameSpeed + " | 
           
       // // Rudder Impulse
       // if (!Mathf.Approximately(rudderAngle, lastRudderAngle))
       //     rb.AddForce(-transform.right * rudderImpulse, ForceMode.Impulse);
       // lastRudderAngle = rudderAngle;

       // TODO set boat velocity if lower than above result, decay if higher.

       // TODO optional - Determine skid angle so velocity isn't aligned with heading
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
    
    public void OnGetCameraX(InputValue value)
    {
        // Debug.Log("CameraX");
    }
    
    public void OnGetCameraY(InputValue value)
    {
        // Debug.Log("CameraY");
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
        if ( halyardToggleUp && !Mathf.Approximately(sailHeight, 1f)) SeekEase(sailHeight, 1f, HalyardEasings);
        if (!halyardToggleUp && !Mathf.Approximately(sailHeight, 0f)) SeekEase(sailHeight, 0f, HalyardEasings);
        
        // Rudder from Target Heading
        float angleToTarget = Vector3.SignedAngle(-transform.right, steerTarget, Vector3.up); // -transform.right is the bow of the boat
        if (steerTarget != Vector3.zero && !Mathf.Approximately(0f, angleToTarget))
        {
            float effort;
            SeekEase(0f, angleToTarget, HeadingEasings, out effort);
            rudderAngle = effort * Mathf.Sign(angleToTarget);
        }
        
        // Mainsheet from optimal angle for current heading
        sailAngleLocalMax = Mathf.Max(mainsailMinimum,OptimalSailAngle.Evaluate(windAngle / 180f));
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
        float result =  -(incident - normal) + normal;
        return (result > 180 || result <= -180) ? (result - 180f % 360f) + 180f : result;
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
        float toMove = Mathf.Clamp(delta, -maxDelta, maxDelta) * Time.deltaTime;

        effort = toMove / Mathf.Abs(toMove);
        if (Mathf.Abs(delta) > outerThresh) return origin + toMove;
        if (Mathf.Abs(delta) > innerThresh)
        {
            effort *= (Mathf.Abs(delta) / outerThresh);
            return origin + (toMove * (Mathf.Abs(delta) / outerThresh));
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

    #region Debug

    private void OnDrawGizmos()
    {
        Vector3 flatVelocity = rb != null ? rb.velocity.ProjectOntoPlane(Vector3.up) : Vector3.zero;
        Vector3 trueWind = currentWind.ProjectOntoPlane(Vector3.up);
        Vector3 apparentWind = trueWind - flatVelocity;
        Vector3 heading = -transform.right.ProjectOntoPlane(Vector3.up); // -transform.right is the bow of the boat

        float windAngle = Vector3.SignedAngle(-heading, apparentWind, Vector3.up);
        float optimalAngle = OptimalSailAngle.Evaluate(Mathf.Abs(windAngle) / 180f) * Mathf.Sign(windAngle) * 90f;
        
        Vector3 origin = transform.position;
        // Wind Angle
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + currentWind.normalized * 5);
        
        // Optimal Sail Angle
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(optimalAngle, Vector3.up) * transform.right) * 5);
        
        // Sail and Reflection
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(sailAngle * 90, Vector3.up) * transform.right) * 5);

        float reflected = ReflectAngle(windAngle, sailAngle * 90f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origin, origin + (Quaternion.AngleAxis(reflected, Vector3.up) * transform.right) * 5);
        
        // Simple Controls - Target Heading
        if (simpleControls)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + steerTarget * 5);
        }
    }

    #endregion
    
}
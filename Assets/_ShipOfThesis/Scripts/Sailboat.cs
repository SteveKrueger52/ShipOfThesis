﻿using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class Sailboat : MonoBehaviour , WindZone.IWindObject
{
    #region Public Members

    public float maxSpeed;
    
    // All Animation Curves are in the range of X,Y = [0,1].
    // For angles, this translates to 0 = 0 and 180 = 1 (wind) and 90 = 1 (sail), with assumed symmetry for negative angles.
    // Furthermore, there will be exactly 1 X value for every Y value and vice versa,
    // even if this cannot be enforced in code.
    
    // The three graphed lines will approximate this chart of projected speeds by boat heading
    // https://physicstoday.scitation.org/na101/home/literatum/publisher/aip/journals/content/pto/2008/pto.2008.61.issue-2/1.2883908/production/images/medium/1.2883908.figures.f5.gif
    // The Physics of Sailing, Physics Today 61, 2, 38 (2008); https://doi.org/10.1063/1.2883908
    public AnimationCurve Knots5, Knots10, Knots20;
    [Space] 
    
    // Used to approximate the best sail angle at a given point of sail, for wind effect drop off
    // Saoud, Hadi & Hua, Minh-Duc & Plumet, Frederic & Amar, Faïz. (2015). Optimal sail angle computation for an
    // autonomous sailboat robot. 10.1109/CDC.2015.7402329. 
    public AnimationCurve OptimalSailAngle; // From Apparent Wind angle
    
    public AnimationCurve SailAngleFalloff; // The decay of effectiveness as sail angle leaves the optimal angle. 
    
    // For a boat with an inoptimal trim, their speed will be determined by the following method:
    // Assume the angle of attack (between apparent wind and sail) is optimal.
    // Find the optimal boat heading for that angle of attack (inverse of OptimalSailAngle)
    // Determine the boat's optimal speed for the faux heading, and apply falloff based on the angle difference.
    
    
    #endregion

    #region Private Members

    private float mpsToKnots = 1.94384f;
    private List<WindZone> windZones;

    #endregion

    #region Interface Methods
    
    public void EnterWind(WindZone wind) { windZones.Add(wind); }
    public void ExitWind(WindZone wind) { windZones.Remove(wind); }
    
    private void FixedUpdate()
    {
        Vector2 wind = AverageWind().ProjectOntoPlane(Vector3.up);
        Vector2 header = transform.forward.ProjectOntoPlane(Vector3.up);
    }
    
    #endregion
    
    #region Input Management
    
    #endregion

    #region Helper Methods

    private Vector3 AverageWind ()
    {
        if (windZones.Count == 0) return Vector3.zero;

        Vector3 acc = Vector3.zero;
        foreach (WindZone w in windZones)
            acc += w.WindVector;
        return acc / windZones.Count;
    }

    
    #endregion
    
}
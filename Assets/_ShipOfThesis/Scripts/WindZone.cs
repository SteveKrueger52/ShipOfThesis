using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WindZone : MonoBehaviour
{
    public interface IWindObject
    {
        void EnterWind(WindZone wind);
        void ExitWind(WindZone wind);
    }
    
    private Vector3 _windVector;
    public Vector3 WindVector
    {
        get { return _windVector; }
        private set { _windVector = value; }
    }
    
    public Vector3 WindDirection
    {
        get { return _windVector.normalized; }
        private set { WindVector = value.normalized * WindSpeed; }
    }
    
    public float WindSpeed
    {
        get { return _windVector.magnitude; }
        private set { WindVector = WindDirection * value; }
    }
    

    private void Awake()
    {
        foreach (Collider c in GetComponents<Collider>())
            c.isTrigger = true;
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        IWindObject obj = other.gameObject.GetComponent<IWindObject>();
        obj.EnterWind(this);
    }

    private void OnTriggerExit(Collider other)
    {
        IWindObject obj = other.gameObject.GetComponent<IWindObject>();
        obj.ExitWind(this);
    }
    
    
}

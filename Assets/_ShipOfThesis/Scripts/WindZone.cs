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
    
    [SerializeField]
    private Vector3 windVector;
    public Vector3 WindVector
    {
        get { return windVector; }
        private set { windVector = value; }
    }
    
    public Vector3 WindDirection
    {
        get { return windVector.normalized; }
        private set { WindVector = value.normalized * WindSpeed; }
    }
    
    public float WindSpeed
    {
        get { return windVector.magnitude; }
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
        if (obj != null)
            obj.EnterWind(this);
    }

    private void OnTriggerExit(Collider other)
    {
        IWindObject obj = other.gameObject.GetComponent<IWindObject>();
        obj.ExitWind(this);
    }
    
    
}

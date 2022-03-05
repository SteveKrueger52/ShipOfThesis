using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class WindVolume : MonoBehaviour
{
    public interface IWindObject
    {
        
    }
    
    public Vector3 windDirection;
    public float windSpeed;
    private List<GameObject> _affectedEntities;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        _affectedEntities = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IWindObject>() != null && !_affectedEntities.Contains(other.gameObject))
            _affectedEntities.Add(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IWindObject>() != null)
            _affectedEntities.Remove(other.gameObject);
    }
}

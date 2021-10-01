using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyGizmos : MonoBehaviour
{
    public Rigidbody _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawSphere(_rb.worldCenterOfMass,0.1f);
    }
}

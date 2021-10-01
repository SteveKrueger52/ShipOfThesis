using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ABoatController : MonoBehaviour, WindVolume.IWindObject
{
    public enum ControlType { Simple, Moderate, Complex}
    public abstract ControlType GetBoatType();

    public void GetCameraX(InputValue value)
    {
    }

    public void GetCameraY(InputValue value)
    {
    }
}

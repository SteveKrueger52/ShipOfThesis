using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComplexBoatController : ABoatController
{
    public override ControlType GetBoatType()
    {
        return ControlType.Complex;
    }

    #region Input Actions

    public void GetHalyard(InputValue value)
    {
    }
    
    public void GetMainsail(InputValue value)
    {
    }
    
    public void GetRudder(InputValue value)
    {
    }

    #endregion
    
    
    
    
}

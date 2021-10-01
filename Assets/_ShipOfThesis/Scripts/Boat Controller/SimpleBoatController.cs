using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoatController : ABoatController
{
    public override ControlType GetBoatType()
    {
        return ControlType.Simple;
    }
}


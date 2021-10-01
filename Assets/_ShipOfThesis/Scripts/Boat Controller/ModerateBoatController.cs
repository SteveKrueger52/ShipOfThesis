using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModerateBoatController : ABoatController
{
    public override ControlType GetBoatType()
    {
        return ControlType.Moderate;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flotilla : Entity
{
    protected override void CalcSteeringForces()
    {
        finalForce += Move();
    }
}

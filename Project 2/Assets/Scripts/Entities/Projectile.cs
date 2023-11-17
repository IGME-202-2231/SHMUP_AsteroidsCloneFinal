using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    protected override void SetUpVariables()
    {
        StartCoroutine(Despawn(3.0f));
    }

    protected override void CalcSteeringForces()
    {
        finalForce += Move();
    }

}

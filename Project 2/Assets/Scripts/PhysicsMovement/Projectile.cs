using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    protected override void CalcSteeringForces()
    {
        finalForce += Move();
    }

    // How to implement the new despawn?
    // cannot be added to the CalcSterringForces, cause in update
    // Start will override the agent class
}

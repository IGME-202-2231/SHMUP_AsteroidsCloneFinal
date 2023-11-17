using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flotilla : Entity
{
    protected override void SetUpVariables() { }

    protected override void CalcSteeringForces()
    {
        finalForce += Move();
    }

    // A large group of enemies, they will slowly move towards the player to a certain distance before opening fire


}

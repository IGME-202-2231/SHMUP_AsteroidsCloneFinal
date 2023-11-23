using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flotilla : Entity
{
    [SerializeField] private float lookAheadTime;

    [SerializeField] private float seperateWeight;

    [SerializeField] private float boundsWeight;

    protected override void SetUpVariables() { }

    protected override void CalcSteeringForces()
    {
        finalForce += Move();

        finalForce += Seperate();

        finalForce += Seperate() * seperateWeight;

        finalForce += StayInBounds(lookAheadTime) * boundsWeight;
    }

    // A large group of enemies, they will slowly move towards the player to a certain distance before opening fire


}

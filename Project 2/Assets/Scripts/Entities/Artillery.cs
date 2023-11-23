using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Entity
{
    [SerializeField] private float lookAheadTime;

    [SerializeField] private float seperateWeight;

    [SerializeField] private float boundsWeight;

    protected override void SetUpVariables() { }

    protected override void CalcSteeringForces()
    {
        if (target.gameObject.activeSelf)
        {
            physicsObj.SetDirection(target.position - transform.position);

            finalForce += Seek(target.position);

            finalForce += Seperate() * seperateWeight;

            finalForce += StayInBounds(lookAheadTime) * boundsWeight;
        }
    }

    // They should sit on the fringes of the player's screen and fire precision shots - retreating if the player ever get too close 
}

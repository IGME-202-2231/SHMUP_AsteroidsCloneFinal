using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Entity
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

    // Very fast but with no projectiles, the bomber will need to turn on a dime and have very good tracking, should upgrade to pursue, perhaps slowing down upon reaching the target
}

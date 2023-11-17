using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Entity
{
    protected override void SetUpVariables() { }

    protected override void CalcSteeringForces()
    {
        if (target.gameObject.activeSelf)
        {
            physicsObj.SetDirection(target.position - transform.position);

            finalForce += Seek(target.position);
        }
    }

    // Very fast but with no projectiles, the bomber will need to turn on a dime and have very good tracking, should upgrade to pursue, perhaps slowing down upon reaching the target
}

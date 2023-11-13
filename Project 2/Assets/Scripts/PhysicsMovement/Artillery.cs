using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Entity
{
    protected override void CalcSteeringForces()
    {
        if (target.gameObject.activeSelf)
        {
            physicsObj.SetDirection(target.position - transform.position);

            finalForce += Seek(target.position);
        }
    }
}

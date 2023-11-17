using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Entity
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

    // They should sit on the fringes of the player's screen and fire precision shots - retreating if the player ever get too close 
}

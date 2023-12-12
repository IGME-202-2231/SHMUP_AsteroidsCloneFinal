using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BomberStates
{
    Player_Far,
    Player_Close
}

public class Bomber : Entity
{
    [SerializeField] private float bubbleRadius = 2.0f;
    [SerializeField] private float lookAhead = 2.0f;

    [Header("Weights")]
    [SerializeField] private float pursueWeight;
    [SerializeField] private float seperateWeight;
    [SerializeField] private float boundsWeight;

    private float targetRadius; 

    private BomberStates currentState;

    protected override void SetUpVariables() 
    {
        currentState = BomberStates.Player_Far;

        targetRadius = target.gameObject.GetComponent<PhysicsBehavior>().Radius;
    }

    protected override void CalcSteeringForces()
    {
        if (target.gameObject.activeSelf)
        {
            Vector3 futureTargetPos = target.gameObject.GetComponent<Entity>().CalcFuturePosition(lookAhead);

            physicsObj.SetDirection(futureTargetPos - transform.position);

            finalForce += Seperate(CollisionManager.Instance.Enemies) * seperateWeight;

            finalForce += StayInBounds(lookAhead) * boundsWeight;

            switch (currentState)
            {
                case BomberStates.Player_Far:
                    finalForce += Seek(futureTargetPos) * pursueWeight;

                    if (BoundryCheck(bubbleRadius))
                    {
                        currentState = BomberStates.Player_Close;
                    }
                    break;

                case BomberStates.Player_Close:
                    finalForce += Arrival(futureTargetPos, Vector3.Distance(target.position, transform.position), bubbleRadius) * pursueWeight;

                    if (!BoundryCheck(bubbleRadius))
                    {
                        currentState = BomberStates.Player_Far;
                    }
                    break;
            }
        }
    }
}

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
    [SerializeField] private float rammingSpeed; // max
    [SerializeField] private float incomingSpeed; // low

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
            // Change lookAhead variable, relying on it for both bounds and Pursue

            // Vector3 futureTargetPos = target.gameObject.GetComponent<Entity>().CalcFuturePosition(lookAhead)

            physicsObj.SetDirection(target.position - transform.position);

            finalForce += Seek(target.gameObject.GetComponent<Entity>().CalcFuturePosition(lookAhead)) * pursueWeight;

            finalForce += Seperate(CollisionManager.Instance.Enemies) * seperateWeight;

            finalForce += StayInBounds(lookAhead) * boundsWeight;

            switch (currentState)
            {
                case BomberStates.Player_Far:
                    if (BoundryCheck(bubbleRadius))
                    {
                        //StartCoroutine(physicsObj.IncrementMaxSpeed(incomingSpeed, -2.0f));

                        currentState = BomberStates.Player_Close;
                    }
                    break;

                case BomberStates.Player_Close:
                    if (!BoundryCheck(bubbleRadius))
                    {
                        //StartCoroutine(physicsObj.IncrementMaxSpeed(rammingSpeed, 2.0f));

                        currentState = BomberStates.Player_Far;
                    }
                    break;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, physicsObj.Radius + 5);
    }

}

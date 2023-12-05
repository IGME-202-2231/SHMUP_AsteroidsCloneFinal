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
    [SerializeField] private float lookAheadTime;

    [SerializeField] private float seperateWeight;

    [SerializeField] private float boundsWeight;

    [SerializeField] private float rammingSpeed; // max

    [SerializeField] private float incomingSpeed; // low

    [SerializeField] private float enemyFutureDist = 2.0f;

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
            physicsObj.SetDirection(target.position - transform.position);

            finalForce += Seek(target.gameObject.GetComponent<Entity>().CalcFuturePosition(enemyFutureDist));

            finalForce += Seperate() * seperateWeight;

            finalForce += StayInBounds(lookAheadTime) * boundsWeight;

            switch (currentState)
            {
                case BomberStates.Player_Far:
                    if (BoundryCheck(5))
                    {
                        physicsObj.MaxSpeed = incomingSpeed;

                        currentState = BomberStates.Player_Close;
                    }
                    break;

                case BomberStates.Player_Close:
                    if (!BoundryCheck(5))
                    {
                        physicsObj.MaxSpeed = rammingSpeed;

                        currentState = BomberStates.Player_Far;
                    }
                    break;
            }
        }
    }

    // basic circle check
    private bool BoundryCheck(float bubbleWeight)
    {
        float a = transform.position.x - target.position.x;
        float b = transform.position.y - target.position.y;
        float c = physicsObj.Radius + targetRadius + bubbleWeight;

        if (a * a + b * b < c * c) { return true; }

        return false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, physicsObj.Radius + 5);
    }

    // Very fast but with no projectiles, the bomber will need to turn on a dime and have very good tracking, should upgrade to pursue, perhaps slowing down upon reaching the target
    // high force, high max speed

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArtilleryStates
{
    Fire,
    Get_Closer,
    Run_Away
}

public class Artillery : Entity
{
    [SerializeField] private float bubbleRadius = 3.0f;

    [Header("Weights")]
    [SerializeField] private float lookAheadTime;
    [SerializeField] private float seperateWeight;
    [SerializeField] private float boundsWeight;

    private ArtilleryStates state;

    private float generalMaxSpeed;

    protected override void SetUpVariables() 
    {
        state = ArtilleryStates.Get_Closer;

        generalMaxSpeed = physicsObj.MaxSpeed;
    }

    protected override void CalcSteeringForces()
    {
        if (target.gameObject.activeSelf)
        {
            finalForce += Seperate(CollisionManager.Instance.Enemies) * seperateWeight;

            finalForce += StayInBounds(lookAheadTime) * boundsWeight;

            switch(state)
            {
                case ArtilleryStates.Fire:
                    physicsObj.SetDirection(target.position - transform.position);

                    if (BoundryCheck(bubbleRadius - 1))
                    {
                        state = ArtilleryStates.Run_Away;

                        StopCoroutine(physicsObj.IncrementMaxSpeed(0.0f, -2.5f)); // stop slowing down

                        StopCoroutine(Firing(2.0f, EntityType.artillery)); // Stop firing, we need to run away

                        StartCoroutine(physicsObj.IncrementMaxSpeed(generalMaxSpeed, 3f)); // too close, run away
                    }

                    else if (!BoundryCheck(bubbleRadius + 3))
                    {
                        state = ArtilleryStates.Get_Closer;

                        StopCoroutine(physicsObj.IncrementMaxSpeed(0.0f, -2.5f)); // stop slowing down

                        StopCoroutine(Firing(2.0f, EntityType.artillery)); // stop firing, too far away

                        StartCoroutine(physicsObj.IncrementMaxSpeed(generalMaxSpeed, 3f)); // get closer
                    }

                    break;

                case ArtilleryStates.Get_Closer:
                    finalForce += Seek(target.position);

                    physicsObj.SetDirection(target.position - transform.position);

                    if (BoundryCheck(bubbleRadius))
                    {
                        state = ArtilleryStates.Fire;

                        StopCoroutine(physicsObj.IncrementMaxSpeed(generalMaxSpeed, 3f)); // Stop moving closer

                        StartCoroutine(physicsObj.IncrementMaxSpeed(0.0f, -2.5f)); // Start slowing down to fire

                        StartCoroutine(Firing(2.0f, EntityType.artillery)); // We're close enough, start firing
                    }

                    break;

                case ArtilleryStates.Run_Away:
                    finalForce += Flee(target.position);

                    physicsObj.SetDirection(transform.position - target.position);

                    if (BoundryCheck(bubbleRadius + 3)) 
                    { 
                        state = ArtilleryStates.Fire;

                        StopCoroutine(physicsObj.IncrementMaxSpeed(generalMaxSpeed, 3f)); // Stop running away

                        StartCoroutine(physicsObj.IncrementMaxSpeed(0.0f, -2.5f)); // Start slowing down to fire

                        StartCoroutine(Firing(2.0f, EntityType.artillery)); // We're at a safe distance, start firing
                    }

                    break;
            }
        }
    }
}

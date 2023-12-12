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
    [SerializeField] private float firingSpeed = 2.0f;

    [Header("Weights")]
    [SerializeField] private float lookAheadTime;
    [SerializeField] private float seperateWeight;
    [SerializeField] private float boundsWeight;

    private ArtilleryStates state;
    private IEnumerator coroutine;

    private Color tempColor;

    protected override void SetUpVariables() 
    {
        coroutine = Firing(firingSpeed, EntityType.enemyProjectile);

        state = ArtilleryStates.Get_Closer;
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
                    tempColor = Color.blue;

                    physicsObj.SetDirection(target.position - transform.position);

                    if (BoundryCheck(bubbleRadius - 1.5f))
                    {
                        state = ArtilleryStates.Run_Away;

                        StopCoroutine(coroutine); // Stop firing, we need to run away
                    }

                    else if (!BoundryCheck(bubbleRadius + 1.5f))
                    {
                        state = ArtilleryStates.Get_Closer;

                        StopCoroutine(coroutine); // stop firing, too far away
                    }

                    break;

                case ArtilleryStates.Get_Closer:
                    tempColor = Color.yellow;

                    // finalForce += Seek(target.position);

                    finalForce += Arrival(target.position, Vector3.Distance(target.position, transform.position) - bubbleRadius, 5.0f);

                    physicsObj.SetDirection(target.position - transform.position);

                    if (BoundryCheck(bubbleRadius + 1.5f))
                    {
                        state = ArtilleryStates.Fire;

                        StartCoroutine(coroutine); // We're close enough, start firing
                    }

                    break;

                case ArtilleryStates.Run_Away:
                    tempColor = Color.red;

                    finalForce += Flee(target.position);

                    physicsObj.SetDirection(target.position - transform.position);

                    if (!BoundryCheck(bubbleRadius - 1.5f)) 
                    { 
                        state = ArtilleryStates.Fire;

                        StartCoroutine(coroutine); // We're at a safe distance, start firing
                    }

                    break;
            }
        }
    }

    // Moved from Entity because it needs a direct reference to the coroutine variable, rather than one passed in
    private IEnumerator Firing(float reloadTime, EntityType bulletType)
    {
        yield return new WaitForSeconds(reloadTime);

        FireProjectile.Instance.Fire(transform, bulletType);

        // Makes sure to reuse the same coroutine variable, makes sure only one firing coroutine at a time
        coroutine = Firing(firingSpeed, EntityType.enemyProjectile);

        StartCoroutine(coroutine);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = tempColor;

        Gizmos.DrawWireSphere(transform.position, bubbleRadius - 1.5f);
        Gizmos.DrawWireSphere(transform.position, bubbleRadius + 1.5f);
    }

}

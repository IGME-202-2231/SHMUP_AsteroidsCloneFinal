using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlotillaStates
{
    Charge,
    Regroup
}

public class Flotilla : Entity
{
    [SerializeField] private float bubbleRadius = 2.0f;

    private IEnumerator coroutine;
    private FlotillaStates state;

    private Vector3 centerPoint;
    private Vector3 sharedDirection;

    [SerializeField] private float lookAheadTime;

    [SerializeField] private float firingSpeed = 2.0f; // MUST BE ABOVE 0.5

    [Header("Weights")]
    [SerializeField] private float seekWeight;
    [SerializeField] private float seperateWeight;
    [SerializeField] private float boundsWeight;
    [SerializeField] private float cohesionWeight;
    [SerializeField] private float alignmentWeight;

    private List<GameObject> flotillaRef; // FLOTILLA BUILD
    public List<GameObject> FlotillaRef 
    { 
        get { return flotillaRef; } 
        set { flotillaRef = value; }
    }

    protected override void SetUpVariables() 
    {
        firingSpeed = Random.Range(firingSpeed - 0.5f, firingSpeed + 0.5f);

        state = FlotillaStates.Charge;

        coroutine = Firing(firingSpeed, EntityType.enemyProjectile);

        centerPoint = Vector3.zero;
        sharedDirection = Vector3.zero;

        StartCoroutine(coroutine); // FLOTILLA BUILD, move to charge state
    }

    protected override void CalcSteeringForces()
    {
        List<GameObject> seperateList = CollisionManager.Instance.Enemies;

        // seperateList.Add(target.gameObject);

        finalForce += Seperate(seperateList) * seperateWeight;

        finalForce += StayInBounds(lookAheadTime) * boundsWeight;

        // Limited Regroup implementation, focus on FLOTILLA BUILD for now
        switch(state)
        {
            case FlotillaStates.Charge:
                finalForce += Seek(target.position) * seekWeight;

                physicsObj.SetDirection(target.position - transform.position);

                // Flocking Behavior
                finalForce += Seek(centerPoint) * cohesionWeight;
                finalForce += ((physicsObj.MaxSpeed * sharedDirection) - physicsObj.Velocity) * alignmentWeight;

                // If flotilla numbers get too low
                    // Disband flotilla

                // if flotillaList.count > 0 && no current flotilla
                    // go to regroup

                break;

            case FlotillaStates.Regroup:
                // Pick first flotilla from flotilla list
                    // add self to flotilla list
                    // seek the center of the flotilla list
                    // if within range of center, go to charge

                // If no active flotillas, go to charge

                break;
        }
    }

    private IEnumerator Firing(float reloadTime, EntityType bulletType)
    {
        yield return new WaitForSeconds(reloadTime);

        FireProjectile.Instance.Fire(transform, bulletType);

        // Makes sure to reuse the same coroutine variable, makes sure only one firing coroutine at a time
        coroutine = Firing(firingSpeed, EntityType.enemyProjectile);

        StartCoroutine(coroutine);
    }

    public void GrabFlockingInfo(Vector3 centerPoint, Vector3 sharedDirection)
    {
        this.centerPoint = centerPoint;

        this.sharedDirection = sharedDirection;
    }

    public void GrabFlotillaReference(List<GameObject> flotillaRef) // FLOTILLA BUILD
    {
        this.flotillaRef = flotillaRef;
    }
}

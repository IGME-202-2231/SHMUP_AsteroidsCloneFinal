using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flotilla : Entity
{
    private IEnumerator coroutine;

    private Vector3 centerPoint;
    private Vector3 sharedDirection;

    [SerializeField] private float lookAheadTime;

    [SerializeField] private float firingSpeed = 2.0f;

    [Header("Weights")]
    [SerializeField] private float seperateWeight;
    [SerializeField] private float boundsWeight;
    [SerializeField] private float cohesionWeight;
    [SerializeField] private float alignmentWeight;

    protected override void SetUpVariables() 
    {
        coroutine = Firing(firingSpeed, EntityType.enemyProjectile);

        centerPoint = Vector3.zero;
        sharedDirection = Vector3.zero;
    }

    protected override void CalcSteeringForces()
    {
        finalForce += Seek(target.position);

        List<GameObject> seperateList = CollisionManager.Instance.Enemies;

        // seperateList.Add(target.gameObject);

        // Flocking Behavior
        finalForce += Seperate(seperateList) * seperateWeight;
        finalForce += Seek(centerPoint) * cohesionWeight;
        finalForce += ((physicsObj.MaxSpeed * sharedDirection) - physicsObj.Velocity) * alignmentWeight;

        finalForce += StayInBounds(lookAheadTime) * boundsWeight;
    }

    private IEnumerator Firing(float reloadTime, EntityType bulletType)
    {
        yield return new WaitForSeconds(reloadTime);

        FireProjectile.Instance.Fire(transform, bulletType);

        // Makes sure to reuse the same coroutine variable, makes sure only one firing coroutine at a time
        coroutine = Firing(firingSpeed, EntityType.enemyProjectile);

        StartCoroutine(coroutine);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    player,
    artillery,
    exploder,
    flotilla,
    enemyProjectile,
    playerProjectile
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected Transform target;

    [SerializeField] protected PhysicsBehavior physicsObj;

    [SerializeField] protected EntityType entityType;

    [SerializeField] protected FireProjectile projectileManager;

    [SerializeField] private float maxForce;

    [SerializeField] private float maxHealth;

    private float health;

    protected Vector3 finalForce;

    private float cameraHeight;
    private float cameraWidth;

    public float Health
    {
        get { return health; }
    }

    void Start()
    {
        health = maxHealth;

        SetUpVariables();

        cameraHeight = Camera.main.orthographicSize;

        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (physicsObj.isColliding)
        {
            health--;

            physicsObj.isColliding = false;
        }

        if (health <= 0) // To be used in Collision manager w/ isColliding to replace current collision setup DONE
        {
            CollisionManager.Instance.CleanUp(gameObject, entityType);
        }

        finalForce = Vector3.zero;

        CalcSteeringForces();

        finalForce = Vector3.ClampMagnitude(finalForce, maxForce);

        physicsObj.ApplyForce(finalForce);
    }

    protected abstract void SetUpVariables();

    protected abstract void CalcSteeringForces();

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public IEnumerator Despawn(float timeDespawn)
    {
        yield return new WaitForSeconds(timeDespawn);

        CollisionManager.Instance.CleanUp(gameObject, entityType);
    }

    protected Vector3 Move()
    {
        Vector3 desiredVelocity = physicsObj.Direction * physicsObj.MaxSpeed;

        Vector3 pointForce = desiredVelocity - physicsObj.Velocity;

        return pointForce;
    }

    protected Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - transform.position;

        desiredVelocity = desiredVelocity.normalized * physicsObj.MaxSpeed;

        Vector3 seekingForce = desiredVelocity - physicsObj.Velocity;

        return seekingForce;
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = transform.position - targetPos;

        desiredVelocity = desiredVelocity.normalized * physicsObj.MaxSpeed;

        Vector3 fleeingForce = desiredVelocity - physicsObj.Velocity;

        return fleeingForce;
    }

    public Vector3 CalcFuturePosition(float time)
    {
        return physicsObj.Velocity * time + transform.position;
    }

    // currently relies on cameraWidth and cameraHeight, if moving to new boundries, needs refactoring
    protected Vector3 StayInBounds(float time)
    {
        Vector3 futurePos = CalcFuturePosition(time);

        if (futurePos.x > cameraWidth ||
            futurePos.x < -cameraWidth ||
            futurePos.y > cameraHeight ||
            futurePos.y < -cameraHeight)
        {
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }

    public void GetInfo(EntityType entityType, Vector3 direction, FireProjectile projectileManager)
    {
        this.entityType = entityType;

        physicsObj.SetDirection(direction);

        this.projectileManager = projectileManager;
    }

    public void GetInfo(Transform target, EntityType entityType, Vector3 direction, FireProjectile projectileManager)
    {
        this.target = target;

        this.entityType = entityType;

        physicsObj.SetDirection(direction);

        this.projectileManager = projectileManager;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionType
{
    enemy,
    player,
    enemyProjectile,
    playerProjectile
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected Transform target;

    [SerializeField] protected PhysicsBehavior physicsObj;

    [SerializeField] private CollisionType collisionType;

    [SerializeField] private float maxForce;

    [SerializeField] private float radius = 1f;

    [SerializeField] private float maxHealth;

    private float health;

    protected Vector3 finalForce;

    /// <summary>
    /// Whether an object is experiencing a collision
    /// </summary>
    public bool isColliding { get; set; }

    public float Radius
    {
        get { return radius; }
    }

    public float Health
    {
        get { return health; }
    }

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            health--;

            isColliding = false;
        }

        if (health <= 0) // To be used in Collision manager w/ isColliding to replace current collision setup DONE
        {
            CollisionManager.Instance.CleanUp(gameObject, collisionType);
        }

        finalForce = Vector3.zero;

        CalcSteeringForces();

        finalForce = Vector3.ClampMagnitude(finalForce, maxForce);

        physicsObj.ApplyForce(finalForce);
    }

    protected abstract void CalcSteeringForces();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public IEnumerator Despawn(float timeDespawn)
    {
        yield return new WaitForSeconds(timeDespawn);

        CollisionManager.Instance.CleanUp(gameObject, collisionType);
    }

    // Refactor to use the SetDirection() in physicsBehaviors
    public void PointDirection(Vector2 cameraPosition)
    {
        // Made sure the cameraPosition is Vector2D, prevents objects from moving on z axis
        Vector3 direction = (cameraPosition - new Vector2(transform.position.x, transform.position.y));

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }

    // Pursue

    // Flock

    // Seperate

    // TempMove
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

        if (futurePos.x > physicsObj.cameraWidth ||
            futurePos.x < -physicsObj.cameraWidth ||
            futurePos.y > physicsObj.cameraHeight ||
            futurePos.y < -physicsObj.cameraHeight)
        {
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }
}

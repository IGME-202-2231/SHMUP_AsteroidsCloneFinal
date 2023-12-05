using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    asteroid,
    player,
    artillery,
    exploder,
    flotilla,
    enemyProjectile,
    playerProjectile
}

public abstract class Entity : MonoBehaviour
{
    protected Vector3 targetPos; // Very temporary for testing obstacle collisions

    private float currentAngle;

    private float timer;

    [SerializeField] protected Transform target;

    [SerializeField] protected PhysicsBehavior physicsObj;

    [SerializeField] protected EntityType entityType;

    [SerializeField] protected FireProjectile projectileManager;

    [SerializeField] private float maxForce;

    [SerializeField] private float maxHealth;

    [SerializeField] private float seperateRange = 1;

    protected List<Vector3> foundObstacles = new List<Vector3>();

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
        currentAngle = Random.Range(-Mathf.PI, Mathf.PI);

        timer = 1.0f;

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

    protected Vector3 Seperate()
    {
        Vector3 seperateForce = Vector3.zero;

        foreach (GameObject enemy in CollisionManager.Instance.Enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Is the agent on top of another agent
            if (Mathf.Epsilon < distance)
            {
                seperateForce += Flee(enemy.transform.position) * (seperateRange / distance);
            }
        }

        return seperateForce;
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

    protected Vector3 AvoidObstacles(float avoidRange)
    {
        Vector3 totalAvoidForce = Vector3.zero;

        foundObstacles.Clear();

        foreach (GameObject asteroid in CollisionManager.Instance.Asteroids)
        {
            Vector3 aTo0 = asteroid.transform.position - transform.position;
            float rightDot, forwardDot;

            Vector3 futurePos = CalcFuturePosition(avoidRange);

            float dist = Vector3.Distance(transform.position, futurePos) + physicsObj.Radius;

            forwardDot = Vector3.Dot(physicsObj.Direction, aTo0);

            // If the obstacles are in front of the navigator
            if (forwardDot >= -asteroid.GetComponent<PhysicsBehavior>().Radius)
            {
                // If the obstacle is within range of the navigator
                if (forwardDot <= dist + asteroid.GetComponent<PhysicsBehavior>().Radius) // something wrong, needs check
                {
                    // Assumes the agent it looking in movement direction
                    rightDot = Vector3.Dot(transform.right, aTo0);

                    // Checking if an obstacle is too far left or right to be within path
                    if (Mathf.Abs(rightDot) <= physicsObj.Radius + asteroid.GetComponent<PhysicsBehavior>().Radius)
                    {
                        foundObstacles.Add(asteroid.transform.position);

                        Vector3 steeringForce = transform.right * (1 - forwardDot / dist) * physicsObj.MaxSpeed;

                        if (rightDot >= 0)
                        {
                            totalAvoidForce += steeringForce;
                        }

                        else
                        {
                            totalAvoidForce -= steeringForce;
                        }
                    }
                }
            }
        }

        return totalAvoidForce;
    }

    protected Vector3 Wander(float time, float radius)
    {
        Vector3 futurePos = CalcFuturePosition(time);

        if (timer < 0.0f)
        {
            // currentAngle += Random.Range(-Mathf.PI / 6, Mathf.PI / 6);
            currentAngle = Random.Range(-Mathf.PI, Mathf.PI);
            timer = 1.0f;
        }

        else { timer -= Time.deltaTime; }

        targetPos = futurePos;
        targetPos.x += Mathf.Cos(currentAngle) * radius;
        targetPos.y += Mathf.Sin(currentAngle) * radius;
        targetPos.z = 0.0f;

        return Seek(targetPos);
    }
}

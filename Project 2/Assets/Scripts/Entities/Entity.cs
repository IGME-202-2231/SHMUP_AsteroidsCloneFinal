using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    asteroid,
    player,
    artillery,
    exploder,
    flotillaShip,
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

    public bool BoundryCheck(float bubbleRadius)
    {
        float a = transform.position.x - target.position.x;
        float b = transform.position.y - target.position.y;
        float c = physicsObj.Radius + target.gameObject.GetComponent<PhysicsBehavior>().Radius + bubbleRadius;

        if (a * a + b * b < c * c) { return true; }

        return false;
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
            // Debug.Log("I'm going out of bounds!!");

            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }

    protected Vector3 Seperate(List<GameObject> seperateList)
    {
        Vector3 seperateForce = Vector3.zero;

        foreach (GameObject enemy in seperateList)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Makes sure not to compare an agent to itself
            if (Mathf.Epsilon < distance)
            {
                seperateForce += Flee(enemy.transform.position) * (seperateRange / distance);
            }
        }

        return seperateForce;
    }


    // IF BULLETS NOT WORKING, CHECK HERE - REMOVED BULLET SPAWNER REFERENCE, USING SINGLETON

    public void GetInfo(EntityType entityType, Vector3 direction)
    {
        this.entityType = entityType;

        physicsObj.SetDirection(direction);
    }

    public void GetInfo(Transform target, EntityType entityType, Vector3 direction)
    {
        this.target = target;

        this.entityType = entityType;

        physicsObj.SetDirection(direction);
    }

    protected Vector3 AvoidObstacles(float avoidRange) // NEEDS FIXING
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

    /// <summary>
    /// Similar to Seek method, but will slow down as it reaches the target
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="targetDist"></param>
    /// <param name="maxDist"></param>
    /// <returns></returns>
    protected Vector3 Arrival(Vector3 targetPos, float targetDist, float maxDist)
    {
        Vector3 desiredVelocity = targetPos - transform.position;

        float minSpeed = (targetDist / maxDist) * physicsObj.MaxSpeed;

        desiredVelocity = desiredVelocity.normalized * minSpeed;

        Vector3 arrivingForce = desiredVelocity - physicsObj.Velocity;
        
        return arrivingForce;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBehavior : MonoBehaviour
{
    [SerializeField] private bool rotateFreely = false;

    public bool enableBoost;

    public bool isColliding;

    private Vector3 position;
    private Vector2 direction;
    private Vector3 velocity;
    private Vector3 acceleration = Vector3.zero;

    [SerializeField] private float mass = 1;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float radius;

    public float Radius { get { return radius; } }
    public Vector2 Direction { get { return direction; } }
    public Vector3 Velocity { get { return velocity; } }
    public float MaxSpeed { get { return maxSpeed; } }

    // Start is called before the first frame update
    void Start()
    {
        enableBoost = true;

        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableBoost)
        {
            velocity += acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

        position += velocity * Time.deltaTime;

        if (!rotateFreely)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        }

        transform.position = position;

        acceleration = Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    public void SetDirection(Vector3 newDirection)
    {
        if (direction != null)
        {
            direction = newDirection.normalized;
        }
    }

    public void SetMaterials(float mass, float radius)
    {
        this.mass = mass;
        this.radius = radius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBehavior : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private bool rotateFreely = false;

    // [Range(0.0f, 360.0f)]
    // [SerializeField] private float spriteRotation;

    private float cameraHeight;
    private float cameraWidth;

    public bool enableBoost;
    public bool isColliding;

    private Vector3 position;
    private Vector2 direction;
    private Vector3 velocity;
    private Vector3 acceleration = Vector3.zero;

    [Header("Physical Traits")]
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

        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;
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

            // A nice polish to the game would be to rotate mis-aligned sprites to their proper rotation
                // for now, too much to rework
                // also, in the future rotate the assets prior to importing them
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

    /// <summary>
    /// Allows for the customization of an physics object's proportions
    /// </summary>
    /// <param name="mass"></param>
    /// <param name="radius"></param>
    public void SetMaterials(float mass, float radius)
    {
        this.mass = mass;
        this.radius = radius;
    }

    public void PlayerBounds()
    {
        if (position.y + radius > cameraHeight)
        {
            position.y = cameraHeight - radius;
        }

        if (position.y - radius < -cameraHeight)
        {
            position.y = -cameraHeight + radius;
        }

        if (position.x + radius > cameraWidth)
        {
            position.x = cameraWidth - radius;
        }

        if (position.x - radius < -cameraWidth)
        {
            position.x = -cameraWidth + radius;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

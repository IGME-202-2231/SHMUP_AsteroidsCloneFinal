using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private bool enableBoost;

    private float speed = 0.0f;

    [SerializeField]
    private float acceleration = 2.0f;

    [SerializeField]
    private float maxSpeed = 2.0f;

    private Vector3 velocity = Vector3.zero;

    private Vector3 objectPosition = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    private float cameraHeight;
    private float cameraWidth;

    public Vector3 Direction
    { 
        get { return direction; } 
    }

    public bool EnableBoost
    {
        get { return enableBoost; }
        set { enableBoost = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Prevents teleporting to Vector3.zero
        objectPosition = transform.position;

        cameraHeight = Camera.main.orthographicSize;

        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableBoost)
        {
            speed += acceleration;

            if (speed > maxSpeed)
            { 
                speed = maxSpeed; 
            }

            velocity += direction * speed * Time.deltaTime;
        }

        objectPosition += velocity;

        // Check Position; if using collisions, need to make sure object isn't in a restricted area before moving it
        if (objectPosition.y + GetComponent<SpriteInfo>().Radius > cameraHeight)
        {
            objectPosition.y = cameraHeight - GetComponent<SpriteInfo>().Radius;
        }

        if (objectPosition.y - GetComponent<SpriteInfo>().Radius < -cameraHeight)
        {
            objectPosition.y = -cameraHeight + GetComponent<SpriteInfo>().Radius;
        }

        if (objectPosition.x + GetComponent<SpriteInfo>().Radius > cameraWidth)
        {
            objectPosition.x = cameraWidth - GetComponent<SpriteInfo>().Radius;
        }

        if (objectPosition.x - GetComponent<SpriteInfo>().Radius < -cameraWidth)
        {
            objectPosition.x = -cameraWidth + GetComponent<SpriteInfo>().Radius;
        }

        // Updates the actual object's position
        transform.position = objectPosition;
    }

    public void PointDirection(Vector2 cameraPosition)
    {
        // Made sure the cameraPosition is Vector2D, prevents objects from moving on z axis
        direction = (cameraPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }
}

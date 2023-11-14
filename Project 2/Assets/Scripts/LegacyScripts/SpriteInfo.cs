using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionTypeLEGACY
{
    enemy,
    player,
    enemyProjectile,
    playerProjectile
}

public class SpriteInfo : MonoBehaviour
{
    // The below fields are all determined by their prefab, so no need to set them to variables during start
    [SerializeField]
    private EntityType collisionType;

    [SerializeField]
    private float radius = 1f;

    // health is a float variable for the sake of drawing the health bar, it 
    [SerializeField]
    private float maxHealth;

    private float health;

    /// <summary>
    /// Whether an object is experiencing a collision
    /// </summary>
    public bool isColliding {  get; set; }

    public float Radius
    {
        get { return radius; }
    }

    public float Health
    {
        get { return  health; }
    }

    public EntityType CollisionType
    {
        get { return collisionType; }
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
    }

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
}
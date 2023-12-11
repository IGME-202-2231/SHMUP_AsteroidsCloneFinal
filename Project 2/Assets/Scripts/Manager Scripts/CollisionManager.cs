using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : Singleton<CollisionManager>
{
    [SerializeField] private GameObject player;

    private List<GameObject> playerProjectiles = new List<GameObject>();

    private List<GameObject> enemyProjectiles = new List<GameObject>();

    private List<GameObject> enemies = new List<GameObject>();

    // TODO: seperate enemies into indiviudal lists, makes for better functionality

        // private List<Bomber> bomberList = new List<Bomber>();

        // private List<Artillery> artilleryList = new List<Artillery>();

        // private List<List<Flotilla>> flotillaList = new List<List<Flotilla>>();

    [SerializeField] private List<GameObject> asteroids = new List<GameObject>();

    private int score;

    public int Score
    {
        get { return score; }

        set { score = value; }
    }

    public int EnemyCount
    {
        get { return enemies.Count; }
    }

    public List<GameObject> Enemies { get { return enemies; } }

    public List<GameObject> Asteroids { get {  return asteroids; } }

    void Update()
    {
        if (enemies.Count > 0 && playerProjectiles.Count > 0)
        {
            for (int i = 0; i < playerProjectiles.Count; i++)
            {
                PhysicsBehavior projectile = playerProjectiles[i].GetComponent<PhysicsBehavior>();

                for (int j = 0; j < enemies.Count; j++)
                {
                    PhysicsBehavior enemy = enemies[j].GetComponent<PhysicsBehavior>();

                    if (CircleCheck(projectile, enemy))
                    {
                        projectile.isColliding = true;
                        enemy.isColliding = true;
                    }
                }
            }
        }

        if (player.gameObject != null)
        {
            PlayerCollision(enemies);

            PlayerCollision(enemyProjectiles);
        }
    }

    private void PlayerCollision(List<GameObject> list)
    {
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                PhysicsBehavior projectile = list[i].GetComponent<PhysicsBehavior>();

                PhysicsBehavior player = this.player.GetComponent<PhysicsBehavior>();

                if (CircleCheck(player, projectile))
                {
                    projectile.isColliding = true;
                    player.isColliding = true;

                    Debug.Log("collision");
                }
            }
        }
    }

    private bool CircleCheck(PhysicsBehavior objA, PhysicsBehavior objB)
    {
        // Do circle check
        float a = objA.transform.position.x - objB.transform.position.x;
        float b = objA.transform.position.y - objB.transform.position.y;
        float c = objA.Radius + objB.Radius;

        if (a * a + b * b < c * c) { return true; }

        return false;
    }

    public void AddCollidable(GameObject collidable, EntityType listType)
    {
        switch (listType)
        {
            case EntityType.artillery:
            case EntityType.exploder:
            case EntityType.flotilla:
                enemies.Add(collidable);
                break;

            case EntityType.playerProjectile:
                playerProjectiles.Add(collidable);
                break;

            case EntityType.enemyProjectile:
                enemyProjectiles.Add(collidable);
                break;

            case EntityType.asteroid:
                asteroids.Add(collidable);
                break;
        }
    }

    // Could simplify by adding a timer to Destroy() upon despawning, but with the collisionManager, it requires removal from the list == more work
    public void CleanUp(GameObject gameObject, EntityType listType)
    {
        switch(listType)
        {
            case EntityType.artillery:
            case EntityType.flotilla:
            case EntityType.exploder:
                enemies.Remove(gameObject);
                score += 10;
                goto default;

            case EntityType.player:
                // player is deleted, enemies no longer spawn or reference the player
                player.SetActive(false);
                break;

            case EntityType.playerProjectile:
                playerProjectiles.Remove(gameObject);
                goto default;

            case EntityType.enemyProjectile:
                enemyProjectiles.Remove(gameObject);
                goto default;

            default:
                Destroy(gameObject);
                break;
        }
    }

    private Vector3 GetCenterPoint(List<Entity> flock)
    {
        Vector3 sumVector = Vector3.zero;

        foreach (Entity entity in flock)
        {
            sumVector += entity.transform.position;
        }

        return sumVector / flock.Count;
    }

    private Vector3 GetSharedDirection(List<Entity> flock)
    {
        Vector3 sumDirection = Vector3.zero;

        foreach (Entity entity in flock)
        {
            sumDirection += entity.transform.up;
        }

        return sumDirection.normalized;
    }
}
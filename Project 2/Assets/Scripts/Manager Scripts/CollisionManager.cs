using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : Singleton<CollisionManager>
{
    [SerializeField]
    private GameObject player;

    private List<GameObject> playerProjectiles = new List<GameObject>();

    private List<GameObject> enemyProjectiles = new List<GameObject>();

    private List<GameObject> enemies = new List<GameObject> ();

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

    // Start might be used to dynamically add all game objects in a scene to this list

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0 && playerProjectiles.Count > 0)
        {
            for (int i = 0; i < playerProjectiles.Count; i++)
            {
                SpriteInfo projectile = playerProjectiles[i].GetComponent<SpriteInfo>();

                for (int j = 0; j < enemies.Count; j++)
                {
                    SpriteInfo enemy = enemies[j].GetComponent<SpriteInfo>();

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
                SpriteInfo projectile = list[i].GetComponent<SpriteInfo>();

                if (CircleCheck(player.GetComponent<SpriteInfo>(), projectile))
                {
                    projectile.isColliding = true;
                    player.GetComponent<SpriteInfo>().isColliding = true;
                }
            }
        }
    }

    private bool CircleCheck(SpriteInfo spriteA, SpriteInfo spriteB)
    {
        // Do circle check
        float a = spriteA.transform.position.x - spriteB.transform.position.x;
        float b = spriteA.transform.position.y - spriteB.transform.position.y;
        float c = spriteA.Radius + spriteB.Radius;

        if (a * a + b * b < c * c)
        {
            return true;
        }

        return false;
    }

    public void AddCollidable(GameObject collidable, CollisionType listType)
    {
        switch (listType)
        {
            case CollisionType.enemy:
                enemies.Add(collidable);
                break;

            case CollisionType.playerProjectile:
                playerProjectiles.Add(collidable);
                break;

            case CollisionType.enemyProjectile:
                enemyProjectiles.Add(collidable);
                break;
        }
    }

    // Could simplify by adding a timer to Destroy() upon despawning, but with the collisionManager, it requires removal from the list == more work
    public void CleanUp(GameObject gameObject, CollisionType listType)
    {
        switch(listType)
        {
            case CollisionType.enemy:
                enemies.Remove(gameObject);
                score += 10;
                goto default;

            case CollisionType.player:
                // player is deleted, enemies no longer spawn or reference the player
                player.SetActive(false);
                break;

            case CollisionType.playerProjectile:
                playerProjectiles.Remove(gameObject);
                goto default;

            case CollisionType.enemyProjectile:
                enemyProjectiles.Remove(gameObject);
                goto default;

            default:
                Destroy(gameObject);
                break;
        }
    }
}
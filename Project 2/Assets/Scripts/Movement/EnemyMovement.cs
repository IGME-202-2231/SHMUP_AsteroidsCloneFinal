using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    exploder,
    flotilla,
    artillery
}

public class EnemyMovement : MonoBehaviour
{
    private float speed = 0.0f;

    private Vector3 direction = Vector3.zero;

    private Transform player;

    private EnemyType enemyType;

    [SerializeField]
    private FireProjectile projectileManager;

    public Vector3 Direction
    {
        get { return direction; }
    }

    void Start()
    {
        player = transform.parent.GetComponent<EnemySpawner>().GetTarget;

        switch (enemyType)
        {
            case EnemyType.exploder:
                speed = 4;
                break;

            case EnemyType.flotilla:
                speed = 1.5f;
                StartCoroutine(gameObject.GetComponent<SpriteInfo>().Despawn(15.0f));
                StartCoroutine(Barrage());
                break;

            case EnemyType.artillery:
                speed = 1;
                StartCoroutine(Halt());
                break;
        }

        SetDirection(direction);
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.exploder:
                // charges towards the player to deal damage

                if (player.gameObject.activeSelf)
                {
                    SetDirection(player.transform.position - transform.position);
                }

                transform.position += direction * speed * Time.deltaTime;

                break;

            case EnemyType.flotilla:
                // moves across the screen, continuously shooting

                transform.position += direction * speed * Time.deltaTime;

                break;

            case EnemyType.artillery:
                // sits on the edge of the game area, shooting at the player

                if (player.gameObject.activeSelf)
                {
                    SetDirection(player.transform.position - transform.position);
                }

                transform.position += direction * speed * Time.deltaTime;

                break;
        }
    }

    private void SetDirection(Vector3 newDirection)
    {
        if (direction != null)
        {
            direction = newDirection.normalized;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
            }
        }
    }

    private IEnumerator Halt()
    {
        yield return new WaitForSeconds(1);

        speed = 0;

        StartCoroutine(Barrage());
    }

    private IEnumerator Barrage()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 0.8f));

        if (gameObject.activeSelf)
        {
            projectileManager.Fire(gameObject.transform);

            StartCoroutine(Barrage());
        }

    }

    public void GetInfo(EnemyType enemyType, Vector3 direction, FireProjectile projectileManager)
    {
        this.enemyType = enemyType;
        
        this.direction = direction;

        this.projectileManager = projectileManager;
    }
}

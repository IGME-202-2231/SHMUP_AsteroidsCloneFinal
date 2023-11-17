using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyProjectile;

    [SerializeField]
    private GameObject playerProjectile;

    public void Fire(Transform host, EntityType entityType)
    {
        GameObject projectilePrefab;

        if (entityType == EntityType.playerProjectile) { projectilePrefab = playerProjectile; }

        else { projectilePrefab = enemyProjectile; }

        GameObject projectile = Instantiate(projectilePrefab, host.position, host.rotation, transform);

        projectile.GetComponent<Projectile>().GetInfo(entityType, host.gameObject.GetComponent<PhysicsBehavior>().Direction, this);

        CollisionManager.Instance.AddCollidable(projectile, entityType);
    }
}

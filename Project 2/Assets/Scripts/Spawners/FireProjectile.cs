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

        if (entityType == EntityType.player) { projectilePrefab = playerProjectile; }

        else { projectilePrefab = enemyProjectile; }

        GameObject projectile = Instantiate(projectilePrefab, host.position, host.rotation, transform);

        projectile.GetComponent<Projectile>().GetInfo(Vector3.zero, entityType, projectile.GetComponent<Projectile>().PhysicsObj.Direction, this);

        CollisionManager.Instance.AddCollidable(projectile, entityType);
    }
}

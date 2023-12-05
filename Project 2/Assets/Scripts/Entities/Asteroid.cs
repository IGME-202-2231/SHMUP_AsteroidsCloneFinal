using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Entity
{
    private float asteroidSize;
    private float angle;
    private Vector2 rotation;

    protected override void SetUpVariables() 
    {
        asteroidSize = Random.Range(1.0f, 6.0f);

        rotation = Vector2.zero;

        transform.localScale = new Vector3(asteroidSize, asteroidSize, asteroidSize);

        physicsObj.SetMaterials(asteroidSize, physicsObj.Radius * asteroidSize);

        StartCoroutine(Despawn(20.0f));
    }

    protected override void CalcSteeringForces()
    {
        angle += Mathf.PI / 6 * Time.deltaTime;

        rotation.x = Mathf.Cos(angle);
        rotation.y = Mathf.Sin(angle);

        transform.rotation = Quaternion.LookRotation(Vector3.back, rotation.normalized);

        finalForce += Move();
    }

    // Needs to randomly generate a few things:
        // size of asteroid
        // it's proportional radius
        // speed of rotation

    // Continue to rotate within update

    // Despawn after a while and remove from collisions

}

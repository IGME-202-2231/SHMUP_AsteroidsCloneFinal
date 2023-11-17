using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Entity
{
    [SerializeField]
    FireProjectile projectileSpawner;

    [SerializeField]
    Camera cam;

    protected override void SetUpVariables() { }

    protected override void CalcSteeringForces()
    {
        finalForce += Move();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            physicsObj.enableBoost = !physicsObj.enableBoost;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // PointDirection(cam.ScreenToWorldPoint(context.ReadValue<Vector2>()));

        Vector3 temp = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());

        physicsObj.SetDirection(temp - new Vector3(transform.position.x, transform.position.y, 0));
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            projectileSpawner.Fire(transform, EntityType.playerProjectile);
        }
    }

}

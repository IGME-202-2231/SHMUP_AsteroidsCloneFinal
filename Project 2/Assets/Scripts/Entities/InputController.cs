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
        if (context.started)
        {
            physicsObj.enableBoost = true;
        }

        if (context.canceled)
        {
            physicsObj.enableBoost = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());

        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

        physicsObj.SetDirection(mousePosition - playerPosition);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            projectileSpawner.Fire(transform, EntityType.playerProjectile);
        }
    }

}

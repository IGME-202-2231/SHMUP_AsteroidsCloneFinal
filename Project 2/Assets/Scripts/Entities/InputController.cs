using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Entity
{
    // Only need to grab the camera for player-bounds, might need to initialize in the future
    [SerializeField] Camera cam;

    protected override void SetUpVariables() 
    {
        physicsObj.enableBoost = false;
    }

    protected override void CalcSteeringForces()
    {
        physicsObj.PlayerBounds();

        finalForce += Move();
    }

    // Player only accelerates if they press down the space-bar
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
            FireProjectile.Instance.Fire(transform, EntityType.playerProjectile);
        }
    }

}

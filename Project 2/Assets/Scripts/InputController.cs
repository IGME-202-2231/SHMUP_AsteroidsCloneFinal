using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    MovementController movementController;

    [SerializeField]
    FireProjectile projectileSpawner;

    [SerializeField]
    Camera cam;

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementController.EnableBoost = !movementController.EnableBoost;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        movementController.PointDirection(cam.ScreenToWorldPoint(context.ReadValue<Vector2>()));
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            projectileSpawner.Fire();
        }
    }

}

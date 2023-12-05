using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Navigator : Entity
{
    [SerializeField] private float wanderRadius = 1.0f;

    [SerializeField] private float wanderTime = 1.0f;

    [SerializeField] private float boundsWeight = 1.0f;

    [SerializeField] private float wanderWeight = 1.0f;

    [SerializeField] private float avoidWeight = 1.0f;

    [SerializeField] private float avoidRange = 1.0f; 
    
    [SerializeField] private Camera cam;

    private Vector2 mousePosTest = Vector3.zero;

    protected override void SetUpVariables() { }

    protected override void CalcSteeringForces()
    {
        physicsObj.SetDirection(physicsObj.Velocity);

        // finalForce += Wander(wanderTime, wanderRadius) * wanderWeight;

        finalForce += Seek(mousePosTest) * wanderWeight;

        finalForce += AvoidObstacles(avoidRange) * avoidWeight;
       
        finalForce += StayInBounds(wanderTime) * boundsWeight;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mousePosTest = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(CalcFuturePosition(wanderTime), wanderRadius);

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(transform.position, targetPos);

        //  Draw safe space box
        Vector3 futurePos = CalcFuturePosition(3.0f);

        float dist = Vector3.Distance(transform.position, futurePos) + physicsObj.Radius;

        Vector3 boxSize = new Vector3(physicsObj.Radius * 2f, dist, physicsObj.Radius * 2f);

        Vector3 boxCenter = Vector3.zero;
        boxCenter.y += dist / 2.0f;

        Gizmos.color = Color.green;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;

        //  Draw lines to found obstacles
        Gizmos.color = Color.yellow;

        foreach (Vector3 asteroid in foundObstacles)
        {
            Gizmos.DrawLine(transform.position, asteroid);
        }

    }

}

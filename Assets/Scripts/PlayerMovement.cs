using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveInput;
    private Rigidbody rb;
    public GameObject playerModel;

    private Vector2 prevMouseDelta;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Debug.Assert(!rb.IsUnityNull());
        Debug.Assert(moveSpeed > 0);
    }

    private void FixedUpdate()
    {
        var forwardDirection = playerModel.transform.forward;
        var inputDirection = moveInput.normalized;
        
        var moveDirection = (forwardDirection * inputDirection.z + playerModel.transform.right * inputDirection.x);
        rb.velocity = moveDirection * moveSpeed;
        
        var mouseDelta = Mouse.current.delta.ReadValue();
        var mouseXDifference = Math.Abs(mouseDelta.x - prevMouseDelta.x);
        
        if (!(mouseXDifference > 0)) 
            return;
        
        var rotationAmount = mouseXDifference switch
        {
            > 0 and <= 1 => (mouseDelta.x < prevMouseDelta.x) ? -2.0f : 2.0f,
            > 1 and <= 3 => (mouseDelta.x < prevMouseDelta.x) ? -3.0f : 3.0f,
            > 3 and <= 5 => (mouseDelta.x < prevMouseDelta.x) ? -4.0f : 4.0f,
            _ => (mouseDelta.x < prevMouseDelta.x) ? -6.0f : 6.0f
        };
        
        var rotation = playerModel.transform.rotation;
        playerModel.transform.Rotate(rotation.x, rotation.y + rotationAmount, rotation.z);
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector3>();
    }
}

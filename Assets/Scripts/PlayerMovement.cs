using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    // Public fields
    public float moveSpeed;
    public GameObject viewPort;
    public GameObject playerModel;
    
    // Private fields
    private Rigidbody _rb;
    private Vector3 _moveInput;
    private Vector2 _prevMouseDelta;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        Debug.Assert(!_rb.IsUnityNull());
        Debug.Assert(moveSpeed > 0);
    }

    private void FixedUpdate()
    {
        //HandlePlayerMovement();
        //HandlePlayerLeftRightRotation();
        //HandleUpDownRotation();
        HandleMouseLookAround();
        HandlePlayerMovement();
    }

    private float rotationX = 0f;
    private float rotationY = 0f;
    public Vector2 sensitivity = Vector2.one * 360f;

    // https://u3ds.blogspot.com/2020/02/look-around-using-mouse.html
    // https://www.youtube.com/watch?v=W70n_bXp7Dc
    private void HandleMouseLookAround()
    {
        rotationY += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity.x;
        rotationX += Input.GetAxis("Mouse Y") * Time.deltaTime * -1 * sensitivity.y;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        viewPort.transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }

    // Handles player movement backward, forward, left, and right
    private void HandlePlayerMovement()
    {
        var forwardDirection = viewPort.transform.forward;
        var inputDirection = _moveInput.normalized;
        var moveDirection = (forwardDirection * inputDirection.z + viewPort.transform.right * inputDirection.x);
        _rb.velocity = moveDirection * moveSpeed;
    }

    // Handles player 360 degree rotation left and right
    private void HandlePlayerLeftRightRotation()
    {
        var mouseDelta = Mouse.current.delta.ReadValue();
        var mouseXDifference = Math.Abs(mouseDelta.x - _prevMouseDelta.x);
        
        if (!(mouseXDifference > 0)) 
            return;
        
        var rotationAmount = mouseXDifference switch
        {
            > 0 and <= 1 => (mouseDelta.x < _prevMouseDelta.x) ? -2.0f : 2.0f,
            > 1 and <= 3 => (mouseDelta.x < _prevMouseDelta.x) ? -3.0f : 3.0f,
            > 3 and <= 5 => (mouseDelta.x < _prevMouseDelta.x) ? -4.0f : 4.0f,
            _ => (mouseDelta.x < _prevMouseDelta.x) ? -6.0f : 6.0f
        };

        //rotationAmount *= Time.deltaTime;
        
        var rotation = playerModel.transform.rotation;
        playerModel.transform.Rotate(rotation.x, rotation.y + rotationAmount, rotation.z);
    }

    // Handles player up down head rotation
    private void HandleUpDownRotation()
    {
        var rotation = viewPort.transform.rotation;
        viewPort.transform.Rotate(0, rotation.y + 10, 0);
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector3>();
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        { }
    }
}

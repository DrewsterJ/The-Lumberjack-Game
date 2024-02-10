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
    public GameObject playerHead;
    
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
        rotationY += Input.GetAxis("Mouse X") * (Time.deltaTime * 1.5f) * sensitivity.x;
        rotationX += Input.GetAxis("Mouse Y") * (Time.deltaTime * 1.5f) * -1 * sensitivity.y;
        rotationX = Mathf.Clamp(rotationX, -90f, 85f);
        
        viewPort.transform.localEulerAngles = new Vector3(rotationX, transform.rotation.y, 0);
        playerHead.transform.localEulerAngles = new Vector3(rotationX, transform.rotation.y, 0);
        transform.localEulerAngles = new Vector3(transform.rotation.x, rotationY, 0);
    }

    // Handles player movement backward, forward, left, and right
    private void HandlePlayerMovement()
    {
        var forwardDirection = transform.forward;
        var inputDirection = _moveInput.normalized;
        var moveDirection = (forwardDirection * inputDirection.z + transform.right * inputDirection.x);
        _rb.velocity = moveDirection * moveSpeed;
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

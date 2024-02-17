using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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

        var curRotation = transform.rotation;
        viewPort.transform.localEulerAngles = new Vector3(rotationX, curRotation.y, 0);
        playerHead.transform.localEulerAngles = new Vector3(rotationX, curRotation.y, 0);
        transform.localEulerAngles = new Vector3(curRotation.x, rotationY, 0);
    }

    // Handles player movement backward, forward, left, and right
    private void HandlePlayerMovement()
    {
        var forwardDirection = transform.forward;
        var inputDirection = _moveInput.normalized;
        var moveDirection = (forwardDirection * inputDirection.z + transform.right * inputDirection.x);
        _rb.velocity = moveDirection * moveSpeed;
    }
    
    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector3>();
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            bool animStarted = GetComponent<EquippedItemController>().PerformLeftClickAnimation();
            if (!animStarted)
                return;
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, 0.17f))
            {
                if (hit.collider.CompareTag("Tree"))
                {
                    var tree = hit.collider.GetComponent<Tree>();
                    var extractedLumber = tree.ExtractLumber(2);
                    var playerInventory = GetComponent<PlayerInventory>();
                    playerInventory.AddLumber(extractedLumber);
                    tree.KillTreeIfHasNoLumber();
                }
            }
        }
        else if (ctx.performed)
        { }
        else if (ctx.canceled)
        { }
    }
}

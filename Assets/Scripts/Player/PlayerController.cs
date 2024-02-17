using System;
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
    private bool _holdingItem;
    
    // External fields
    public GameObject actionTextGameObject;
    public GameObject playerItemHoldPosition;
    private Collider colliderPlayerIsLookingAt = null;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        actionTextGameObject.SetActive(false);
        _holdingItem = false;
        
        Debug.Assert(!_rb.IsUnityNull());
        Debug.Assert(moveSpeed > 0);
    }

    private void Update()
    {
        // Recommend keeping a way to keep track of all interactable objects in a container, and checking if the player is within range of any
        // of them. If the player is within 1.0f of any interactable object, we run the raycast. Otherwise, we don't keep checking in this update loop.
        var layerMask = LayerMask.GetMask("InteractableObject");
        if (Physics.Raycast(playerHead.transform.position, playerHead.transform.TransformDirection(Vector3.forward),
                out var hit, 0.45f, layerMask))
        {
            if (hit.collider.CompareTag("LogBundle") && !_holdingItem)
            {
                actionTextGameObject.SetActive(true);
                colliderPlayerIsLookingAt = hit.collider;
            }
        }
        else
        {
            colliderPlayerIsLookingAt = null;
            actionTextGameObject.SetActive(false);
        }
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
    
    public void OnKeyboardInput(InputAction.CallbackContext ctx)
    {
        var key = ctx.action.name;
        
        if (ctx.started)
        {
            if (key == "E")
            {
                if (actionTextGameObject.activeSelf)
                    PickupInteractiveObject(colliderPlayerIsLookingAt);
            }
        }
        else if (ctx.performed)
        { }
        else if (ctx.canceled)
        { }
    }

    private void PickupInteractiveObject(Collider itemToPickup)
    {
        if (itemToPickup.IsUnityNull() || itemToPickup.IsDestroyed() || _holdingItem)
            return;
        
        //itemToPickup.transform.position = playerItemHoldPosition.transform.position;
        //itemToPickup.transform.rotation = playerItemHoldPosition.transform.rotation;
        var prevPos = playerItemHoldPosition.transform.position;
        var prevRot = playerItemHoldPosition.transform.rotation;

        var newObject = Instantiate(itemToPickup.gameObject, prevPos, prevRot, playerItemHoldPosition.transform);
        
        // Disable colliders of object being held
        var colliders = newObject.GetComponents<BoxCollider>();
        foreach (var elem in colliders)
            Destroy(elem);

        var childrenColliders = newObject.GetComponentsInChildren<MeshCollider>();
        foreach (var elem in childrenColliders)
            Destroy(elem);
        
        _holdingItem = true;
        //playerItemHoldPosition.transform.position = prevPos;
        //playerItemHoldPosition.transform.rotation = prevRot;
        
        Destroy(itemToPickup.gameObject);
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

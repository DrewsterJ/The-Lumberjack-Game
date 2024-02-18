using System;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private Collider _colliderPlayerIsLookingAt = null;
    private GameObject _itemPlayerIsHolding;

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
        var layerMask = LayerMask.NameToLayer("InteractableObject");
        var ignoreLayerMask = LayerMask.GetMask("Player");
        if (Physics.Raycast(playerHead.transform.position, playerHead.transform.TransformDirection(Vector3.forward),
                out var hit, 0.45f, ~ignoreLayerMask))
        {
            if ((hit.collider.gameObject.layer & layerMask) == layerMask)
            {
                if (hit.collider.CompareTag("LogBundle") && !_holdingItem)
                {
                    actionTextGameObject.SetActive(true);
                    _colliderPlayerIsLookingAt = hit.collider;
                }
                else
                {
                    _colliderPlayerIsLookingAt = null;
                    actionTextGameObject.SetActive(false);
                }
            }
            else
            {
                _colliderPlayerIsLookingAt = null;
                actionTextGameObject.SetActive(false);
            }
        }
        else
        {
            _colliderPlayerIsLookingAt = null;
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
                if (_holdingItem)
                {
                    DropItemPlayerIsHolding();
                }
                else
                {
                    if (actionTextGameObject.activeSelf)
                        PickupInteractiveObject(_colliderPlayerIsLookingAt);
                }
            }
        }
        else if (ctx.performed)
        { }
        else if (ctx.canceled)
        { }
    }

    private void DropItemPlayerIsHolding()
    {
        if (!_holdingItem || _itemPlayerIsHolding.IsUnityNull())
            return;
        
        var dropPosition = transform.position + transform.forward * 0.30f;
        //dropPosition.y = 0.2f;
        
        var droppedItem = Instantiate(_itemPlayerIsHolding, dropPosition, _itemPlayerIsHolding.transform.rotation);
        
        // https://forum.unity.com/threads/set-layermask-to-everything-via-code-c.468180/
        var noLayerMask = LayerMask.GetMask("Nothing");
        
        // Enable colliders of object that was dropped
        var colliders = droppedItem.GetComponents<BoxCollider>();
        foreach (var elem in colliders)
            elem.excludeLayers = noLayerMask;

        var childrenColliders = droppedItem.GetComponentsInChildren<MeshCollider>();
        foreach (var elem in childrenColliders)
            elem.excludeLayers = noLayerMask;
        
        var droppedItemRb = droppedItem.GetComponent<Rigidbody>();
        droppedItemRb.isKinematic = false;
        droppedItemRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                                    RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                    RigidbodyConstraints.FreezeRotationZ;
        
        Destroy(_itemPlayerIsHolding);

        _holdingItem = false;
        GetComponent<EquippedItemController>().activeItem.SetActive(true);
        _itemPlayerIsHolding = null;
        GetComponent<PlayerInventory>().ExtractAllLumber();
    }

    private void PickupInteractiveObject(Collider itemToPickup)
    {
        if (itemToPickup.IsUnityNull() || itemToPickup.IsDestroyed() || _holdingItem)
            return;

        var newPosition = playerItemHoldPosition.transform.position;
        newPosition.y += .027f;
        
        var newObject = Instantiate(itemToPickup.gameObject, newPosition, 
            playerItemHoldPosition.transform.rotation, playerItemHoldPosition.transform);
        
        // https://forum.unity.com/threads/set-layermask-to-everything-via-code-c.468180/
        var allLayerMask = ~0;
        
        // Disable colliders of object being held
        var colliders = newObject.GetComponents<BoxCollider>();
        foreach (var elem in colliders)
            elem.excludeLayers = allLayerMask;

        var childrenColliders = newObject.GetComponentsInChildren<MeshCollider>();
        foreach (var elem in childrenColliders)
            elem.excludeLayers = allLayerMask;

        var objRb = newObject.GetComponent<Rigidbody>();
        objRb.isKinematic = true;
        objRb.constraints = RigidbodyConstraints.None;
        
        GetComponent<EquippedItemController>().activeItem.SetActive(false);
        _holdingItem = true;
        _itemPlayerIsHolding = newObject;
        GetComponent<PlayerInventory>().AddLumber(3);
        
        Destroy(itemToPickup.gameObject);
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (_holdingItem)
                return;
            
            bool animStarted = GetComponent<EquippedItemController>().PerformLeftClickAnimation();
            if (!animStarted)
                return;
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, 0.17f))
            {
                if (hit.collider.CompareTag("Tree"))
                {
                    var tree = hit.collider.GetComponent<Tree>();
                    //var extractedLumber = tree.ExtractLumber(2);
                    tree.DamageTree(2);
                }
            }
        }
        else if (ctx.performed)
        { }
        else if (ctx.canceled)
        { }
    }
}

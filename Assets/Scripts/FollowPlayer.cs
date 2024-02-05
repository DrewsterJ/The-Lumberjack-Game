using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject playerViewport;

    void Start()
    {
        Debug.Assert(!playerViewport.IsUnityNull());
    }
    
    void FixedUpdate()
    {
        transform.position = playerViewport.transform.position;
        transform.rotation = playerViewport.transform.rotation;
    }
}

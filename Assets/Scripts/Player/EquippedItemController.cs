using System;
using Unity.VisualScripting;
using UnityEngine;

public class EquippedItemController : MonoBehaviour
{
    public GameObject activeItem;
    public Animator activeItemClickAnim;
    private float lastActionStartTime;

    private static readonly int Clicked = Animator.StringToHash("Clicked");

    private void Start()
    {
        lastActionStartTime = Time.time;
        Debug.Assert(!activeItem.IsUnityNull());
    }

    // Returns whether the animation successfully started
    public bool PerformLeftClickAnimation()
    {
        if (Math.Abs(lastActionStartTime - Time.time) < 0.5f)
            return false;

        lastActionStartTime = Time.time;
        
        activeItemClickAnim.SetTrigger(Clicked);
        return true;
    }
}

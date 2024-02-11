using System;
using System.Collections;
using System.Collections.Generic;
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

    public void PerformLeftClickAnimation()
    {
        if (Math.Abs(lastActionStartTime - Time.time) < 0.5f)
            return;

        lastActionStartTime = Time.time;
        
        activeItemClickAnim.SetTrigger(Clicked);
    }
}

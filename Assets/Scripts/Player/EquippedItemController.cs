using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquippedItemController : MonoBehaviour
{
    public GameObject activeItem;
    public Animator activeItemClickAnim;

    private static readonly int Clicked = Animator.StringToHash("Clicked");

    private void Start()
    {
        Debug.Assert(!activeItem.IsUnityNull());
    }

    public void PerformLeftClickAnimation()
    {
        activeItemClickAnim.SetTrigger(Clicked);
    }
}

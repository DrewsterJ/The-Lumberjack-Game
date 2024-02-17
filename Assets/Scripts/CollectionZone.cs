using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    public TMP_Text lumberCollectionText;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInventory = other.GetComponent<PlayerInventory>();
            GameManager.instance.AddLumber(playerInventory.ExtractAllLumber());
            lumberCollectionText.text = GameManager.instance.collectedLumber.ToString();
        }
    }
}

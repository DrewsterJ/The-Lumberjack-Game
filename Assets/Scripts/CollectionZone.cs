using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    public List<GameObject> logPlacementPositions;
    private int _curLogPlacementPosition = 0;
    
    

    private void Start()
    {
        foreach (var log in logPlacementPositions)
        {
            log.SetActive(false);
            //var meshCollider = log.GetComponent<MeshCollider>();
            //Destroy(meshCollider);
            //meshCollider.excludeLayers = LayerMask.GetMask("Nothing");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LogBundle"))
        {
            var numLogs = other.gameObject.GetComponent<LogBundle>().numLogs;
            AddLogsToCollection(numLogs);
            Destroy(other.gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
    }

    private void AddLogsToCollection(int amount)
    {
        if (_curLogPlacementPosition + amount > logPlacementPositions.Count)
            amount = logPlacementPositions.Count - _curLogPlacementPosition;

        var lastActiveLogPosition = _curLogPlacementPosition;
        for (var i = _curLogPlacementPosition; i < _curLogPlacementPosition + amount; ++i)
        {
            logPlacementPositions[i].SetActive(true);
            var meshCollider = logPlacementPositions[i].GetComponentInChildren<MeshCollider>();
            meshCollider.excludeLayers = ~0;
            lastActiveLogPosition = i;
        }

        _curLogPlacementPosition = lastActiveLogPosition;
    }
}

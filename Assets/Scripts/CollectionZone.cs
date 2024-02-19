using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionZone : MonoBehaviour
{
    public List<GameObject> logPlacementPositions;
    private int _curLogPlacementPosition = 0;
    public int numLogsAvailable;

    private void Start()
    {
        foreach (var log in logPlacementPositions)
        {
            log.SetActive(false);
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

    private void AddLogsToCollection(int amount)
    {
        if (_curLogPlacementPosition + amount > logPlacementPositions.Count)
            amount = logPlacementPositions.Count - _curLogPlacementPosition;

        numLogsAvailable += amount;
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

    public int RemoveAvailableLogs()
    {
        foreach (var log in logPlacementPositions)
            log.SetActive(false);
        
        var availableLogs = numLogsAvailable;
        _curLogPlacementPosition = 0;
        numLogsAvailable = 0;
        return availableLogs;
    }
}

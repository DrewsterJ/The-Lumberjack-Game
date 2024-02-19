using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberDepositZone : MonoBehaviour
{
    public GameObject logCollectionZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ExtractLogs()
    {
        var numLogs = logCollectionZone.GetComponent<CollectionZone>().RemoveAvailableLogs();
        return numLogs;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlankConversionBlock"))
        {
            var conversionBlock = other.GetComponent<ConversionBlock>();
            var lumberProcessingFacility = conversionBlock.lumberProcessingFacility;
            lumberProcessingFacility.AddProcessedPlanks(1);
            Destroy(gameObject);
        }
    }
}

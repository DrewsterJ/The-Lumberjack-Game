using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessLog : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(DestroyObjectCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LogConversionBlock"))
        {
            var conversionBlock = other.GetComponent<ConversionBlock>();
            var lumberProcessingFacility = conversionBlock.lumberProcessingFacility;
            lumberProcessingFacility.AddPlanks(3);
            Destroy(gameObject);
        }
    }

    /*IEnumerator DestroyObjectCoroutine()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }*/
}

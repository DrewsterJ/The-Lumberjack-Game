using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberProcessingFacility : MonoBehaviour
{
    public Animator depositButtonAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DepositLogs()
    {
        depositButtonAnim.Play("Default");
        depositButtonAnim.SetTrigger("ButtonPushed");
    }
}

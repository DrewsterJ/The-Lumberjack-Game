using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    public int minLumberAmount = 5;
    public int maxLumberAmount = 20;
    private int _lumber;

    private void Start()
    {
        _lumber = Random.Range(minLumberAmount, maxLumberAmount);
        
        Debug.Assert(_lumber > 0);
    }

    public int ExtractLumber(int requestedLumber)
    {
        var player = GameObject.FindWithTag("Player");

        if (_lumber - requestedLumber < 0)
            requestedLumber = _lumber;

        _lumber -= requestedLumber;

        return requestedLumber;
    }

    public void KillTreeIfHasNoLumber()
    {
        if (_lumber == 0)
            Kill();
    }

    private void Kill()
    {
        _lumber = 0;
        Destroy(gameObject);
    }
}

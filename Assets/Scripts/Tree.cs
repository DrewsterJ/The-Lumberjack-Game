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
    
    /*
     * CutTreeDown()
     * 1. Tree falls (as an animation?)
     * 2. Drop 2 - 3 packs of logs
     * 3. Emit a small smoke particle effect
     * 4. After a small amount of time, make the tree disappear
     */

    private void Kill()
    {
        _lumber = 0;
        Destroy(gameObject);
    }
}

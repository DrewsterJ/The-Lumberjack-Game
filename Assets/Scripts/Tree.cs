using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    private int minLumberAmount = 1;
    private int maxLumberAmount = 4;
    public int _health = 20;
    public int _lumber;

    public GameObject logBundlePrefab;

    private void Start()
    {
        _lumber = Random.Range(minLumberAmount, maxLumberAmount);
        
        Debug.Assert(_lumber > 0);
    }

    /*public int ExtractLumber(int requestedLumber)
    {
        var player = GameObject.FindWithTag("Player");

        if (_lumber - requestedLumber < 0)
            requestedLumber = _lumber;

        _lumber -= requestedLumber;

        return requestedLumber;
    }*/

    public void CutTreeDown()
    {
        var forwardPosition = transform.position + transform.forward * 0.30f;
        var backwardPosition = transform.position + -transform.forward * 0.30f;
        var leftPosition = transform.position + -transform.right * 0.30f;
        var rightPosition = transform.position + transform.right * 0.30f;
        
        List<Vector3> lumberSpawnPositions = new List<Vector3>(){forwardPosition, backwardPosition, leftPosition, rightPosition};
        
        for (var i = 0; i < _lumber; ++i)
        {
            Debug.Assert(lumberSpawnPositions.Count > 0);
            var randIndex = Random.Range(0, lumberSpawnPositions.Count - 1);
            var lumberSpawnPosition = lumberSpawnPositions[randIndex];
            var randomRotation = Random.Range(0f, 360f);
            var lumberSpawnRotation = new Quaternion(0.0f, randomRotation, 0.0f, 0.0f);
            lumberSpawnPositions.Remove(lumberSpawnPosition);
            var logBundle = Instantiate(logBundlePrefab, lumberSpawnPosition, lumberSpawnRotation);
        }
    }
    
    /*
     * CutTreeDown()
     * 1. Tree falls (as an animation?)
     * 2. Drop 2 - 3 packs of logs
     * 3. Emit a small smoke particle effect
     * 4. After a small amount of time, make the tree disappear
     */

    public void DamageTree(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
            Kill();
    }

    private void Kill()
    {
        _health = 0;
        
        CutTreeDown();
        
        _lumber = 0;
        
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LumberProcessingFacility : MonoBehaviour
{
    public Animator depositButtonAnim;
    public GameObject lumberDepositZone;

    public GameObject processedLogPrefab;
    public GameObject processedLogSpawnPos;


    public GameObject plankPrefab;
    public GameObject plankSpawnPos;
    private Coroutine _logSpawnCoroutine = null;
    private Coroutine _plankSpawnCoroutine = null;

    private int _numLogsToSpawn = 0;
    private int _numPlanks;
    private int _numProcessedPlanks;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(depositButtonAnim != null && !depositButtonAnim.IsDestroyed());
        Debug.Assert(lumberDepositZone != null && !lumberDepositZone.IsDestroyed());
    }

    public void DepositLogs()
    {
        depositButtonAnim.Play("Default");
        depositButtonAnim.SetTrigger("ButtonPushed");
        var logsToDeposit = lumberDepositZone.GetComponent<LumberDepositZone>().ExtractLogs();

        if (logsToDeposit == 0)
            return;

        _numLogsToSpawn += logsToDeposit;

        if (_logSpawnCoroutine == null)
            _logSpawnCoroutine = StartCoroutine(LogSpawnCoroutine());
    }

    public void AddPlanks(int numPlanks)
    {
        if (numPlanks == 0)
            return;

        _numPlanks += numPlanks;
        if (_plankSpawnCoroutine == null)
            _plankSpawnCoroutine = StartCoroutine(PlankSpawnCoroutine());
    }

    IEnumerator LogSpawnCoroutine()
    {
        while (_numLogsToSpawn > 0)
        {
            SpawnLog();
            --_numLogsToSpawn;
            yield return new WaitForSeconds(5);
        }

        _logSpawnCoroutine = null;
    }

    IEnumerator PlankSpawnCoroutine()
    {
        while (_numPlanks > 0)
        {
            SpawnPlank();
            --_numPlanks;
            yield return new WaitForSeconds(0.75f);
        }

        _plankSpawnCoroutine = null;
    }


    void SpawnLog()
    {
        var newLog = Instantiate(processedLogPrefab, processedLogSpawnPos.transform.position,
            processedLogSpawnPos.transform.rotation, transform);
    }
    
    void SpawnPlank()
    {
        var newPlank = Instantiate(plankPrefab, plankSpawnPos.transform.position,
            plankSpawnPos.transform.rotation, transform);
    }

    public void AddProcessedPlanks(int amt)
    {
        _numProcessedPlanks += amt;
    }
}

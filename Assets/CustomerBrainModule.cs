using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerBrainModule : MonoBehaviour
{
    public CustomerData data;

    public List<BrainStateBase> stateList = new List<BrainStateBase>();

    public BrainStateBase startState;
    public float startWait = 0.5f;
    private void Awake()
    {
        stateList = GetComponentsInChildren<BrainStateBase>().ToList();
        InitStates();
        StartCoroutine(WaitBeforeStart());
    }

    IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(startWait);
        startState.Activate();
    }

    void InitStates()
    {
        foreach (var v in stateList) v.Init();
    }
}

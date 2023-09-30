using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class CustomerBrainModule : MonoBehaviour
{



    public CustUrge chill;
    public CustUrge thirst;
    CustomerHoldModule holdModule;
    public CustUrge dominantUrge;

    public float drinkThirstRatio = 2;
    CustomerMouthModule mouthModule;

    private void Awake()
    {
        mouthModule = GetComponent<CustomerMouthModule>();
        mouthModule.SwallowEvent += MouthModule_SwallowEvent;
        thirst.EnterThresholdEvent += Thirst_EnterThresholdEvent;
        thirst.ExitThresholdEvent += Thirst_ExitThresholdEvent;
        holdModule = GetComponent<CustomerHoldModule>();
        thirst.SetUrge(10);
    }

    private void MouthModule_SwallowEvent(float obj)
    {
        thirst.ChangeUrgeAmount(-drinkThirstRatio * obj);
    }

    private void Thirst_ExitThresholdEvent(UrgeThreshold obj)
    {
        if (obj.thresholdName == "CanDrink")
        {
            Debug.Log("canDrink threshold exited");
            dominantUrge = chill;
            holdModule.SetDominantUrge("chill");
        }
    }

    private void Thirst_EnterThresholdEvent(UrgeThreshold obj)
    {
        if (obj.thresholdName == "CanDrink")
        {
            Debug.Log("canDrink threshold reached");
            dominantUrge = thirst;
            holdModule.SetDominantUrge("drink");
        }
    }





        

    /*

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
    */
}

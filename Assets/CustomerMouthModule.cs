using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMouthModule : MonoBehaviour
{
    public enum MouthState
    {
        Idle,
        Drinking,
        Swallowing
    }

    public event Action<MouthState> ChangeMouthStateEvent = (x) => { };
    public MouthState mouthState;
    public float mouthSize = 10;
    public float drinkSpeed = 6;
    public float swallowDur = 3;
    public float curMouthContents;

    DrinkModule curDrinkModule;

    float startSwallowTime;

    private void Awake()
    {
        GetComponent<CustomerHoldModule>().StartDrinkingEvent += CustomerMouthModule_StartDrinkingEvent;
        curMouthContents = 0;
    }

    private void CustomerMouthModule_StartDrinkingEvent(DrinkModule obj)
    {
        if (mouthState == MouthState.Idle)
        {
            curDrinkModule = obj;
            SetStateDrinking();
        }   
    }

    private void SetStateDrinking()
    {
        SetState(MouthState.Drinking);
    }

    private void SetStateIdle()
    {
        SetState(MouthState.Idle);
    }

    private void SetStateSwallowing()
    {
        startSwallowTime = Time.time;
        SetState(MouthState.Swallowing);
    }

    private void FixedUpdate()
    {
        if (mouthState == MouthState.Drinking)
        {
            if (curDrinkModule.cm.curConAm > 0 && curMouthContents < mouthSize)
            {
                curMouthContents += drinkSpeed * Time.fixedDeltaTime;
                curDrinkModule.Drink(drinkSpeed * Time.fixedDeltaTime);
            }
            else
            {
                SetStateSwallowing();
            }    
        }
        else if (mouthState == MouthState.Swallowing)
        {
            if (Time.time - startSwallowTime > swallowDur)
            {
                curMouthContents = 0;
                SetStateIdle();
            }
        }
    }

    void SetState(MouthState s)
    {
        ChangeMouthStateEvent(s);
        mouthState = s;
    }
}

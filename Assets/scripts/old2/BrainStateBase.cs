using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainStateBase : MonoBehaviour
{
    protected CustomerBase baseModule;
    protected CustomerSpeechModule speech;
    protected CustomerBrainModule brain;
    public bool active = false;

    

    public virtual void Init()
    {
        baseModule = GetComponentInParent<CustomerBase>();
        baseModule.TouchStateChangeEvent += Cb_TouchStateChangeEvent;
        brain = GetComponentInParent<CustomerBrainModule>();
        speech = GetComponentInParent<CustomerSpeechModule>();
        active = false;
    }

    protected virtual void Cb_TouchStateChangeEvent(CustomerBase.CustomerTouchState obj)
    {
       
    }

    public virtual void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }



}

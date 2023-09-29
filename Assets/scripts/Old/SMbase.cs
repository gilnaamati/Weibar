using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SMbase : MonoBehaviour
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public SMdata curData;

    protected virtual void Awake()
    {
        curData = GetComponent<SMdata>();
    }

    public State state = State.Running;
   public bool started = false;

    public State UpdateState()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();

}

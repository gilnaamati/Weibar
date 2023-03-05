using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourTarget : MonoBehaviour
{
    public static event Action<PourTarget> CursorEnterEvent = (x) => { };
    public static event Action<PourTarget> CursorExitEvent = (x) => { };
    // Start is called before the first frame update

    public void OnHandEnter()
    {
        CursorEnterEvent(this);
    }

    public void OnHandExit()
    {
        CursorExitEvent(this);
    }

    public void OnMouseEnter()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
       // CursorEnterEvent(this);
    }

    public void OnMouseExit()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
       // CursorExitEvent(this);
    }

}

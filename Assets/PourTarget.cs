using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourTarget : MonoBehaviour
{
    public static event Action<Transform> CursorEnterEvent = (x) => { };
    public static event Action<Transform> CursorExitEvent = (x) => { };
    public Transform pourMouth;
    // Start is called before the first frame update

    public void OnMouseEnter()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        CursorEnterEvent(pourMouth);
    }

    public void OnMouseExit()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        CursorExitEvent(pourMouth);
    }

}

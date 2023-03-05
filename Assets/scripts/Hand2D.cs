using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInParent<PourTarget>().OnHandEnter();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponentInParent<PourTarget>().OnHandExit();
    }


    void Update()
    {
        transform.position = MouseData2D.Inst.mouseWorldPos;
    }
}

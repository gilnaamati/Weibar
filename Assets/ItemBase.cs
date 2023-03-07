using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public enum ItemTouchState
    {
       NoHover,
       Hover,
       TopHover
    }

    public ItemTouchState itemTouchState;
    
    Rigidbody2D rb;
    public GameObject debugMarker;
    public static event Action<ItemBase> HoverEnterEvent = (x) => { };
    public static event Action<ItemBase> HoverExitEvent = (x) => { };


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStateNoHover();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HoverEnterEvent(this);
        //debugMarker.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HoverExitEvent(this);
      //  debugMarker.GetComponent<SpriteRenderer>().color = Color.white;   
    }

    public void SetStateNoHover()
    {
        debugMarker.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetStateHover()
    {
        debugMarker.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void SetStateTopHover()
    {
        debugMarker.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    




}

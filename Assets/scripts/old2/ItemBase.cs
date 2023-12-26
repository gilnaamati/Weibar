using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour, IHoverableOld
{
    public enum ItemTouchState
    {
       NoHover,
       Hover,
       TopHover
    }

    public ItemTouchState itemTouchState;
    
    
    public GameObject debugMarker;

    

    public static event Action<ItemBase> HoverEnterEvent = (x) => { };
    public static event Action<ItemBase> HoverExitEvent = (x) => { };

    
    public GameObject GetGameobject()
    {
        return gameObject;
    }

    public GameObject hoverGameObject
    {
        get { return gameObject; }
    }
    public HoverManagerOld.HoverLayer hoverLayer => hLayer;

    public HoverManagerOld.HoverLayer hLayer;
    private void Awake()
    {
        
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
        itemTouchState = ItemTouchState.NoHover;
        debugMarker.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetStateHover()
    {
        itemTouchState = ItemTouchState.Hover;
        debugMarker.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void SetStateTopHover()
    {
        itemTouchState = ItemTouchState.TopHover;
        debugMarker.GetComponent<SpriteRenderer>().color = Color.blue;
    }

}

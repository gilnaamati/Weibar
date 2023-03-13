using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerBase : MonoBehaviour
{
    public static event Action<CustomerBase> HoverEnterEvent = (x) => { };
    public static event Action<CustomerBase> HoverExitEvent = (x) => { };
    public event Action<CustomerTouchState> TouchStateChangeEvent = (x) => { };
    public enum CustomerTouchState
    {
        NoHover,
        Hover,
        TopHover
    }

    public CustomerTouchState customerTouchState;
    public TextMeshPro debugText;
    private void Awake()
    {
        SetStateNoHover();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HoverEnterEvent(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HoverExitEvent(this);
    }

    public void SetStateNoHover()
    {
        customerTouchState = CustomerTouchState.NoHover;
        TouchStateChangeEvent(CustomerTouchState.NoHover);
        debugText.text = "NoHover";
    }

    public void SetStateHover()
    {
        customerTouchState = CustomerTouchState.Hover;
        TouchStateChangeEvent(CustomerTouchState.Hover);
        debugText.text = "Hover";
    }

    public void SetStateTopHover()
    {
        customerTouchState = CustomerTouchState.TopHover;
        TouchStateChangeEvent(CustomerTouchState.TopHover);
        debugText.text = "TopHover";
    }



}

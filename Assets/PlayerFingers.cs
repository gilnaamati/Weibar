using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFingers : MonoBehaviour
{
    private PlayerHand hand;

    public Transform HeldItemHolder;
    
    
    private void Awake()
    {
        InputListener.ButtonPressedEvent += InputListenerOnButtonPressedEvent;
        hand = GetComponent<PlayerHand>();
        
    }

    private void InputListenerOnButtonPressedEvent()
    {
           
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

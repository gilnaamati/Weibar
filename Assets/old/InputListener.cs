using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using MouseButton = UnityEngine.UIElements.MouseButton;

public class InputListener : MonoBehaviour
{
    public static event Action ButtonPressedEvent = () => { };
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ButtonPressedEvent();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;





public class IinteractableHolder : MonoBehaviour
{
    private List<IInteractable> _iinteractableList = new List<IInteractable>();
    private void Awake()
    {
        _iinteractableList = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToList();
        
      
    }
}

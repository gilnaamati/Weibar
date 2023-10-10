using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IInteractable
{
    public void StartInteraction(IInteractable i);
    public void EndInteraction(IInteractable i);
}

public class IinteractableHolder : MonoBehaviour
{
    
}

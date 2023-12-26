using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
  

    public void NewIinteractableTouching(IInteractable interactable);
    public void NewInteractableNotTouching(IInteractable interactable);

}

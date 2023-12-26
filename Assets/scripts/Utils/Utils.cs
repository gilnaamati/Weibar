using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
   public static void Toggle2DColliders(GameObject tar, bool on)
    {
        var l = tar.GetComponentsInChildren<Collider2D > ();
        foreach (var v in l) v.enabled = on;
    }

   public static List<IInteractable> NewInteractableTouching(IInteractable interactable, List<IInteractable> interactableList)
    {
        if (!interactableList.Contains(interactable)) interactableList.Add(interactable);
        return interactableList;
    }
   
   public static List<IInteractable> NewInteractableNotTouching(IInteractable interactable, List<IInteractable> interactableList)
   {
       if (interactableList.Contains(interactable)) interactableList.Remove(interactable);
       return interactableList;
   }
   
  

}

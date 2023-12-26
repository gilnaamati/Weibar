using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable 
{
   public void SetMovePosition(Vector3 position);
   public Vector3 GetMovePosition();
   int GetLayerOrder();

   public void SetHeld();
   public void SetNotHeld();
}

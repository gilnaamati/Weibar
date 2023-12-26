using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPourable
{
   public void StartPouring(IPourIntoable pourIntoable);
   public void StopPouring();
}

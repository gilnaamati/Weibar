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


}

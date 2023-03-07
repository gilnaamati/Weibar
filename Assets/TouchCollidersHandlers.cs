using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCollidersHandlers : MonoBehaviour
{
    // Start is called before the first frame update


    public void ToggleColliders(bool on)
    {
        var l = GetComponentsInChildren<Collider2D>();
        foreach (var v in l) v.enabled = on;
    }
}

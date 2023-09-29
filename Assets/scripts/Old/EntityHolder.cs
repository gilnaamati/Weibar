using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityHolder : MonoBehaviour
{
    public List<PosMarker> posMarkerList = new List<PosMarker>();

    private void Awake()
    {
        posMarkerList = GetComponentsInChildren<PosMarker>().ToList();
    }

    
}

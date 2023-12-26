using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(UrgeEngine))]
public class UrgeEngineInterface : MonoBehaviour
{
    public bool autoUpdate = false;

    private void Update()
    {
        if (autoUpdate)
            GetComponent<UrgeEngine>().UpdateMeters();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/CustomerData")]
[System.Serializable]

public class CustomerData : ScriptableObject
{
    [Header("Vars")]
    public float textWaitPerChar = 0.1f;

    [Header("Seeking Attention")]
    public List<string> Callouts = new List<string>();

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Liquid")]
[System.Serializable]
public class LiquidData : ScriptableObject
{
    public string name;
    public Color liquidColor = Color.yellow;
    public float alcoholRatio = 0.05f;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(menuName = "Config/GameData")]
[System.Serializable]
public class GameData : ScriptableObject
{
    
    
    public List<LiquidData> LiquidDataList = new List<LiquidData>();

    
}

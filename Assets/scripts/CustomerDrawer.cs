using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Config/CustomerDrawer")]
[System.Serializable]
public class CustomerDrawer : ScriptableObject
{
    public string DrawerName;
    public string DrawerKey;
    public List<string> DialogueList = new List<string>();
    public List<string> DrawerActivationList = new List<string>();
}

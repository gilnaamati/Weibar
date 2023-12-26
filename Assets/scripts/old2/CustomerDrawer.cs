using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Config/CustomerDrawer")]
[System.Serializable]
public class CustomerDrawer : ScriptableObject
{
    public string dialog;
    public List<string> drawerOpenKeyList;
    public List<string> drawerCloseKeyList;
    public List<CustomerDrawer> drawerActivationList = new List<CustomerDrawer>();
    public List<string> callWhileOpenList;
    public Vector2 waitBetweenCallsRange = new Vector2(2, 5);
}

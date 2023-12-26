using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IHoverableOld
{
    
    public GameObject hoverGameObject { get; }
    public HoverManagerOld.HoverLayer hoverLayer { get; }
}

public class HoverManagerOld : MonoBehaviour
{
    public enum HoverLayer // This is the layer that the object is on
    {
        Customer = 200,
        Object = 100,
        None = 0
    }

    public List<IHoverableOld> hoverableList = new List<IHoverableOld>();
    public List<GameObject> hoverableGameObjectList = new List<GameObject>();
    
    private void Awake()
    {
        hoverableList = FindObjectsOfType<MonoBehaviour>().OfType<IHoverableOld>().ToList();
        foreach (var v in hoverableList)
        {
            hoverableGameObjectList.Add(v.hoverGameObject);
        }
    }
}

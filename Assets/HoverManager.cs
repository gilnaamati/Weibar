using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IHoverable
{
    
    public GameObject hoverGameObject { get; }
    public HoverManager.HoverLayer hoverLayer { get; }
}

public class HoverManager : MonoBehaviour
{
    public enum HoverLayer // This is the layer that the object is on
    {
        Customer = 200,
        Object = 100,
        None = 0
    }

    public List<IHoverable> hoverableList = new List<IHoverable>();
    public List<GameObject> hoverableGameObjectList = new List<GameObject>();
    
    private void Awake()
    {
        hoverableList = FindObjectsOfType<MonoBehaviour>().OfType<IHoverable>().ToList();
        foreach (var v in hoverableList)
        {
            hoverableGameObjectList.Add(v.hoverGameObject);
        }
    }
}

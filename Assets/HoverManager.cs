using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    private List<IHoverable> _hoverableList = new List<IHoverable>();
    private void Awake()
    {
        _hoverableList = FindObjectsOfType<MonoBehaviour>().OfType<IHoverable>().ToList();
        
        for (var i = 0; i < _hoverableList.Count; i++)
        {
            _hoverableList[i].SetLayerOrder(i);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoverable
{
    public void SetLayerOrder(int _layerOrder);
    public int GetLayerOrder();
}

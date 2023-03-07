using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVisuals : MonoBehaviour
{
    SpriteRenderer sr;
    public int sortInd;
    private void Awake()
    {
       
    }

    public void SetSortingLayerIndex (int i)
    {
        GetComponentInChildren<SpriteRenderer>();
        sortInd = i;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
    }


}

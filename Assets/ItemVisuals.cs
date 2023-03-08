using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVisuals : MonoBehaviour
{
    SpriteRenderer sr;
    public int sortInd;
    public SpriteRenderer mainRenderer;
    private void Awake()
    {
       
    }

    public void SetSortingLayerIndex (int i)
    {

        sortInd = i;
        mainRenderer.sortingOrder = i;
    }


}

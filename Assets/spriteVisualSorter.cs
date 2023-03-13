using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteVisualSorter : MonoBehaviour
{
    SpriteRenderer sr;
    public int sortInd;
    public SpriteRenderer mainRenderer;

    public void SetSortingLayerIndex (int i)
    {
        sortInd = i;
        mainRenderer.sortingOrder = i;
    }


}

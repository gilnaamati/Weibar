using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskHandler : MonoBehaviour
{
    public SpriteRenderer LiquidRenderer;

    public SpriteMask liquidMask;

    public int maskSortingIndex = 0;

    public void SetMask(int maskLayer_)
    {
     
        maskSortingIndex = maskLayer_;
        
        LiquidRenderer.sortingOrder = maskSortingIndex;
        liquidMask.frontSortingOrder = maskSortingIndex;
        liquidMask.backSortingOrder = maskSortingIndex-1;
       
    }

}

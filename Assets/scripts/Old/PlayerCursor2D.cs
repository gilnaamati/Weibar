using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCursor2D : MonoBehaviour, IInteractable
{
    [SerializeField]
    public int touchingCount = 0;
 
    List<IInteractable> _interactableTouchingList = new List<IInteractable>();
    public IMovable heldItem;
    public Vector3 heldItemOffset;
    private void Awake()
    {
        MouseData2D.MouseDownEvent += MouseData2D_MouseDownEvent;
        MouseData2D.MouseUpEvent += MouseData2D_MouseUpEvent; 
    }
    
    private void MouseData2D_MouseUpEvent()
    {
        if (heldItem!= null) heldItem.SetNotHeld();
        heldItem = null;
    }

    private void MouseData2D_MouseDownEvent()
    {
        List<IMovable> movableList = new List<IMovable>();
        foreach (var interactable in _interactableTouchingList)
        {
            if (interactable is IMovable)
            {
                movableList.Add(interactable as IMovable);
            }
        }
        if (movableList.Count > 0)
        {
            heldItem = movableList.First(x=>x.GetLayerOrder() == movableList.Max(y=>y.GetLayerOrder()));
            heldItemOffset = heldItem.GetMovePosition() - MouseData2D.Inst.mouseWorldPos;
            heldItem.SetHeld();
        }
        else
        {
            heldItem = null;
        }
    }
    
    void Update()
    {
        transform.position = MouseData2D.Inst.mouseWorldPos;
        if (heldItem != null)
        heldItem.SetMovePosition(transform.position + heldItemOffset);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var interactable = col.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            NewIinteractableTouching(interactable);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        var interactable = col.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            NewInteractableNotTouching(interactable);
        }
    }

    public void NewIinteractableTouching(IInteractable interactable)
    {
        _interactableTouchingList = Utils.NewInteractableTouching( interactable, _interactableTouchingList);
        touchingCount = _interactableTouchingList.Count;
    }

    public void NewInteractableNotTouching(IInteractable interactable)
    {
        _interactableTouchingList = Utils.NewInteractableNotTouching( interactable, _interactableTouchingList);
        touchingCount = _interactableTouchingList.Count;
    }
}

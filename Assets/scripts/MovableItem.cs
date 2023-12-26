using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableItem : MonoBehaviour, IHoverable, IInteractable, IMovable
{
    private int _layerOrder = 0;
    protected List<IInteractable> _interactableTouchingList = new List<IInteractable>();
    
    protected bool _held = false;

    public GameObject physicalCollider;
    public virtual void SetLayerOrder(int i)
    {
        this._layerOrder = i;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
    }

    public Vector3 GetMovePosition()
    {
        return transform.position;
    }

    public int GetLayerOrder()
    {
        return _layerOrder;
    }

    public void SetHeld()
    {
        physicalCollider.SetActive(false);
        _held = true;
    }

    public void SetNotHeld()
    {
        physicalCollider.SetActive(true);
        _held = false;
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
    
    public virtual void NewIinteractableTouching(IInteractable interactable)
    {
        _interactableTouchingList = Utils.NewInteractableTouching( interactable, _interactableTouchingList);
    }

    public virtual void NewInteractableNotTouching(IInteractable interactable)
    {
        _interactableTouchingList = Utils.NewInteractableNotTouching( interactable, _interactableTouchingList);
    }

    public void SetMovePosition(Vector3 position)
    {
       transform.position = position;
    }
}

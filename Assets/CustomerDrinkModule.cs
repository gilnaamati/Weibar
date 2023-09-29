using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDrinkModule : MonoBehaviour
{
    public Transform CustomerHand;
    public PickupModule offeredItem;
    public PickupModule ownedDrink;
    public PickupModule heldDrink;

    float waitBeforeFirstSiptime = 1;
 

    public void SetOwnedDrink (PickupModule d)
    {
        ownedDrink = d;
        Invoke("PickupOwnedDrink", waitBeforeFirstSiptime);
        
    }

    public void PickupOwnedDrink()
    {
        ownedDrink.SetStateHeldByCustomer(this);
    }
     
}

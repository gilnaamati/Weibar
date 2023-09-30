using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkModule : MonoBehaviour
{
    public ContentModule cm;
    PickupModule pm;
    CustomerHoldModule curCustomer;
    CustomerMouthModule curMouth;

    private void Awake()
    {
        cm = GetComponent<ContentModule>();
        pm = GetComponent<PickupModule>();
        pm.SetStateHeldByCustomerEvent += Pm_SetStateHeldByCustomerEvent;
        pm.SetStateIdleEvent += Pm_SetStateIdleEvent;
    }

    private void Pm_SetStateHeldByCustomerEvent(CustomerHoldModule obj)
    {
        curCustomer = obj;
        curCustomer.ChangeStateEvent += CurCustomer_ChangeStateEvent;
        curMouth = curCustomer.GetComponent<CustomerMouthModule>();
    }

    public void Drink(float amount)
    {
        cm.RemoveContents(amount);
    }


    private void CurCustomer_ChangeStateEvent(CustomerHoldModule.HandState obj)
    {
        
    }

    private void Pm_SetStateIdleEvent()
    {
      
    }

  
}

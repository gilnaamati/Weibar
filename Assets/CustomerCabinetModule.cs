using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerCabinetModule : MonoBehaviour
{
   public List<CustomerDrawer> activeDrawerList = new List<CustomerDrawer>();

   public CustomerDrawer currentlyOpenDrawer;

   private CustomerSpeechModule speechModule;

   private CustomerBase customerBase;
   private void Awake()
   {
       Hand2D.HandDownEvent += Hand2DOnHandDownEvent;
       speechModule = GetComponent<CustomerSpeechModule>();
       customerBase = GetComponent<CustomerBase>();
   }

   private void Hand2DOnHandDownEvent(string obj)
   {
       if (customerBase.customerTouchState == CustomerBase.CustomerTouchState.TopHover)
       {
           var l = activeDrawerList.Where(x => x.drawerOpenKeyList.Contains(obj)).ToList();
           if (l.Count > 0)
           {
               OpenDrawer(l.GetRandom());
           }
       }
       
      
   }

   public void OpenDrawer(CustomerDrawer drawer)
   {
       activeDrawerList.Remove(drawer);
       currentlyOpenDrawer = drawer;
       speechModule.SetSpeech(currentlyOpenDrawer.dialog);
   }
}

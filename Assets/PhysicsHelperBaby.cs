using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHelperBaby : MonoBehaviour
{
   private Rigidbody rb;

   public float maxV = 10;
   
   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
   }
   
   private void FixedUpdate()
   {
      if (rb.velocity.magnitude > maxV)
      {
         rb.velocity = rb.velocity.normalized * maxV;
      }
   }
}

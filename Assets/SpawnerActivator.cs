using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SpawnerActivator : MonoBehaviour
{
   public bool spawn = false;

   private void Update()
   {
      if (!Application.isPlaying)
      {
         if (spawn)
         {
            spawn = false;
            GetComponent<InsideCubeSpawner>().Spawn();
         }
      }
   }
}

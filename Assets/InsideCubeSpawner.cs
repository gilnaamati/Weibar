using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InsideCubeSpawner : MonoBehaviour
{
   public GameObject spawnObject;
   public Vector3 spawnCount;
   public Transform cloneArea;
   Transform spawnParent;
   public void Spawn()
   {
      Vector3 spawnSpacing = new Vector3(cloneArea.localScale.x / (spawnCount.x), cloneArea.localScale.y / (spawnCount.y),
         cloneArea.localScale.z / (spawnCount.z));
      Vector3 spawnStart = cloneArea.position - cloneArea.localScale / 2 + spawnSpacing / 2;
      
      var newParent = new GameObject("spawnParent");
      newParent.transform.parent = transform;
      if (spawnParent != null) DestroyImmediate(spawnParent.gameObject);
      spawnParent = newParent.transform;
 
      for (int i = 0; i < spawnCount.x; i++)
      {
         for (int j = 0; j < spawnCount.y; j++)
         {
            for (int k = 0; k < spawnCount.z; k++)
            {
               var p = PrefabUtility.InstantiatePrefab(spawnObject, spawnParent) as GameObject;
               p.transform.position = spawnStart + new Vector3(i * spawnSpacing.x, j * spawnSpacing.y, k * spawnSpacing.z);
            }
         }
      }
   }
}

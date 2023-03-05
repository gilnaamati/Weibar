using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
[ExecuteInEditMode]
public class CellGridOperator : MonoBehaviour
{


    public bool gen = false;
   
  

    // Update is called once per frame
    void Update()
    {
        if (gen)
        {
            gen = false;
            var v = GetComponent<CellGrid>();
            if (v != null) v.PopulateGrid();
        }
    }
}

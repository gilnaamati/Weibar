using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourTargetModule : MonoBehaviour
{
    ContentModule cm;

    private void Awake()
    {
        cm = GetComponent<ContentModule>();
    }

    // Start is called before the first frame update
    public void RecieveLiquids(float amount)
    {
        cm.ChangeContentAmount(amount);
    }
}

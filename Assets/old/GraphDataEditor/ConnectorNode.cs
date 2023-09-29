using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorNode : BaseNode
{
    public override void Init()
    {
        base.Init();
        CreateInputPort("in");
        CreateOutputPort("out");
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;




public abstract class BaseNode : ScriptableObject
{
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
   // protected BaseNodeView nodeView;
   [HideInInspector] public List<BasePortData> inputPortList = new List<BasePortData>();
   [HideInInspector] public List<BasePortData> outputPortList = new List<BasePortData>();
   public virtual void Init()
    {
       // nodeView = _nodeView;
    }
    
    public virtual void CreateInputPort (string portName)
    {
       // nodeView.CreateInputPort(portName);
        inputPortList.Add(new BasePortData
        {
            portName = portName
        });
    }

    public virtual void CreateOutputPort(string portName)
    {
       // nodeView.CreateOutputPort(portName);
        outputPortList.Add(new BasePortData
        {
            portName = portName
        });
    }
    
    public virtual void UpdateField(string fieldName, string fieldValue)
    {
       //get a list of fields in this class
       var v = this.GetType().GetField(fieldName);
       //if the value is a string
       if (v.FieldType == typeof(string)) v.SetValue(this, fieldValue.ToString());
       if (v.FieldType == typeof(float)) v.SetValue(this, float.Parse(fieldValue));
         if (v.FieldType == typeof(int)) v.SetValue(this, int.Parse(fieldValue));
       //get the field that matches the fieldName

       //set the value of the field to the fieldValue





    }
    


}

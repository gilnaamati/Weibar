using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PosMarker : MonoBehaviour
{
    public string markerName;

    public List<PosMarker> proximityList = new List<PosMarker>();

    public float proximityDist = 3;
    EntityHolder entityHolder;
    private void Awake()
    {
        entityHolder = GetComponentInParent<EntityHolder>();
    }

    public void UpdateProximityList()
    {
        proximityList.Clear();
        foreach (var v in entityHolder.posMarkerList)
        {
            if (v!= this)
            {
                if (Vector3.Distance(transform.position, v.transform.position) <= proximityDist)
                {
                    proximityList.Add(v);
                }    
            }
        }
    }

    public PosMarker GetClosestPosMarker(string posMarkerName)
    {
        var l = entityHolder.posMarkerList.Where(x => x.markerName == posMarkerName).ToList();
        if (l.Count > 0)
        {
           return l.OrderByDescending(x => Vector3.Distance(transform.position, x.transform.position)).Last();
        }
        return this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScroller : MonoBehaviour
{
    public Vector2 scrollMargin = new Vector2(0.1f, 0.1f);
    public Vector2 scrollSpeed = new Vector2(1,1);
    public Vector2 curSpeed;
    public Vector2 curSpeedTar;
    public float speedLerp = 30;
    public AnimationCurve marginCurve;


    void Update()
    {
        var m = MouseData2D.Inst.mouseVPpos;

        curSpeedTar = Vector2.zero;

        if (m.x> 0 && m.x < scrollMargin.x) 
        {     
            curSpeedTar.x = -marginCurve.Evaluate(1 - (m.x / scrollMargin.x)) * scrollSpeed.x;
        }
        if (m.x < 1 && m.x > 1 - scrollMargin.x)
        {
            curSpeedTar.x = marginCurve.Evaluate((m.x + scrollMargin.x - 1) / scrollMargin.x) * scrollSpeed.x;
        }
        if (m.y > 0 && m.y < scrollMargin.y)
        {
            curSpeedTar.y = -marginCurve.Evaluate(1 - (m.y / scrollMargin.y)) * scrollSpeed.y;
        }
        if (m.y < 1 && m.y > 1 - scrollMargin.y)
        {
            curSpeedTar.y = marginCurve.Evaluate((m.y + scrollMargin.y - 1) / scrollMargin.y) * scrollSpeed.y;
        }


    }

    private void FixedUpdate()
    {
        curSpeed = Vector2.Lerp(curSpeed, curSpeedTar, speedLerp * Time.fixedDeltaTime);
        transform.position += (Vector3)curSpeed;
    }
}

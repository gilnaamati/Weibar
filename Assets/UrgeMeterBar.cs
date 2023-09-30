using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrgeMeterBar : MonoBehaviour
{

    public GameObject thresholdVisuals;
    public Transform bgBar;
    public Transform fgBar;
    public string letter;
    public Color foregroundCol;
    public Color backgroundCol;
    public Transform holder;

    private void Awake()
    {
        SetColors(foregroundCol, backgroundCol);
    }

    public void SetColors(Color fg, Color bg)
    {

        foregroundCol = fg;
        backgroundCol = bg;
        fgBar.GetComponentInChildren<SpriteRenderer>().color = fg;
        bgBar.GetComponentInChildren<SpriteRenderer>().color = bg;
    }

    public void SetLetter(string s)
    {
        letter = s;
        GetComponentInChildren<TextMesh>().text = s;
    }

    public void SetBar(float f)
    {
        var s = fgBar.localScale;
        s.y = f;
        fgBar.localScale = s;
    }

    public void SetLine(float f, Color c)
    {
        var t = Instantiate(thresholdVisuals, holder);
        t.gameObject.SetActive(true);
        t.transform.localPosition = new Vector3(0, f, 0);
        t.GetComponentInChildren<SpriteRenderer>().color = c;
    }

}

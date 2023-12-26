using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class UrgeMeterBar : MonoBehaviour
{

    public GameObject thresholdVisuals;
    public Transform bgBar;
    public Transform fgBar;
    public string letter;
    public Color foregroundCol;
    public Color backgroundCol;
    public Color letterCol;
    public Transform holder;
    [Range(0.0f, 1.0f)]
    public float barValue; 
    private void Awake()
    {
        InitMeter();
    }

    public void InitMeter()
    {
        SetColors(foregroundCol, backgroundCol);
        SetLetter(letter, letterCol);
        SetBar(barValue);
    }

    public void SetColors(Color fg, Color bg)
    {
        foregroundCol = fg;
        backgroundCol = bg;
        fgBar.GetComponentInChildren<SpriteRenderer>().color = fg;
        bgBar.GetComponentInChildren<SpriteRenderer>().color = bg; 
    }

    public void SetLetter(string s, Color c)
    {
        letter = s;
        GetComponentInChildren<TextMeshPro>().text = s;
        GetComponentInChildren<TextMeshPro>().color = c;
    }

    public void SetBar(float f)
    {
        barValue = f;
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

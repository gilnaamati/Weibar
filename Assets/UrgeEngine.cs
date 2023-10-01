using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UrgeEngine : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float sub;
    [Range(0.0f, 100.0f)]
    public float joy;
    [Range(0.0f, 100.0f)]
    public float cmf;
    [Range(0.0f, 100.0f)]
    public float crv;
    [Range(0.0f, 100.0f)]
    public float adc;

    public float genMod = 1;

    public float joySubFac;
    public float joyCrvFac;
    public float joyCmfFac;

    public float crvAdcFac;
    public float crvJoyFac;

    public float cmfJoyFac;
    public float cmfCrvFac;

    public float adcFac;
    public float adcCmfFac;

    public float subMax;
    public float subDur = 1;
    public AnimationCurve SubCurve;

    float hitTime;

    public bool hit;

    

    public UrgeMeterBar joyMeter, crvMeter, cmfMeter, adcMeter, subMeter;

    private void Awake()
    {
        hitTime = Time.time - subDur;
    }

    public void UpdateMeters()
    {
        subMeter.SetBar(sub / 100);
        joyMeter.SetBar(joy / 100);
        crvMeter.SetBar(crv / 100);
        cmfMeter.SetBar(cmf / 100);
        adcMeter.SetBar(adc / 100);
    }

    private void Update()
    {
        if (hit == true)
        {
            hitTime = Time.time;
            hit = false;
        }
    }

    private void FixedUpdate()
    {
      

        StepValues();
    }

    void StepValues()
    {
        sub = SubCurve.Evaluate(Mathf.Clamp01((Time.time - hitTime) / subDur)) * subMax;
        sub = Mathf.Clamp(sub, 0, 100);
        

        joy += sub * joySubFac * crv * joyCrvFac*Time.fixedDeltaTime * genMod - cmf*joyCmfFac * Time.fixedDeltaTime * genMod;
        joy = Mathf.Clamp(joy, 0, 100);
       

        crv += adc * crvAdcFac * Time.fixedDeltaTime * genMod - joy * crvJoyFac * Time.fixedDeltaTime * genMod;
        crv = Mathf.Clamp(crv, 0, 100);
       

        cmf += joy * cmfJoyFac * Time.fixedDeltaTime * genMod - crv * cmfCrvFac * Time.fixedDeltaTime * genMod;
        cmf = Mathf.Clamp(cmf, 0, 100);
        

        adc += adcFac * Time.fixedDeltaTime * genMod - cmf * adcCmfFac * Time.fixedDeltaTime * genMod;
        adc = Mathf.Clamp(adc, 0, 100);
       
    }
}

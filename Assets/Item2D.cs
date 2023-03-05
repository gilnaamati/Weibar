using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Item2D : MonoBehaviour
{
    public enum ItemState
    {
        Idle,
        Held,
        Pour,
        Trans
    }

    public ItemState itemState;
    public ItemState nextItemState;
    public float dragLerp = 20;
    public float rotLerp = 5;
    public Vector2 rotRange = new Vector2(60f, 160f);
    public float maxContents = 750f;
    public float curContents = 750f;
    public float PourRate = 50f;
    public float minDropDist = 2;
    public float pourRotationDur = 0.5f;
    public float straightRotationDur = 0.1f;
    public AnimationCurve pourRotationCurve;
    public AnimationCurve BottlePourCurve;
    public Transform liquid;
    public Transform image;
    public Transform imageRotTar;
    public Transform handle;  
    public Transform liquidHinge;  
    public List<Transform> bottleCornerList = new List<Transform>();

    Transform curPourTar;
    TextMeshPro contentTM;
    SpriteRenderer sr;
    Rigidbody2D rb;
    bool newTransCoroutineStarted = false;
    Vector3 tarPos;
    float pourDirection;

    private void Awake()
    {
        contentTM = GetComponentInChildren<TextMeshPro>();
        contentTM.text = maxContents.ToString();
        sr = liquid.gameObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        PourTarget.CursorEnterEvent += PourTarget_CursorEnterEvent;
        PourTarget.CursorExitEvent += PourTarget_CursorExitEvent;
    }

    private void PourTarget_CursorEnterEvent(Transform obj)
    {
        if (itemState == ItemState.Held)
        {
            curPourTar = obj;
            StartCoroutine(SetStateTrans(ItemState.Pour, pourRotationDur));
        }
    }

    private void PourTarget_CursorExitEvent(Transform obj)
    {
        if (itemState != ItemState.Idle)
        {
            StartCoroutine(SetStateTrans(ItemState.Held, straightRotationDur));
        }
    }

    public void OnMouseDown()
    {
        if (itemState == ItemState.Idle)
            SetStateHeld();
    }

    public void OnMouseUp()
    {
        if (itemState != ItemState.Idle)
            StartCoroutine(SetStateTrans(ItemState.Idle, straightRotationDur));
    }

    private void Update()
    {
       switch (itemState)
        {
            case ItemState.Held:
                sr.color = Color.blue;
                tarPos = MouseData2D.Inst.mouseWorldPos - handle.localPosition;     
                break;
            case ItemState.Pour:
                sr.color = Color.yellow;  
                float r = ( rotRange.x + (rotRange.y - rotRange.x) * BottlePourCurve.Evaluate((1 - (curContents / maxContents)))) * pourDirection;
                tarPos = MouseData2D.Inst.mouseWorldPos - handle.localPosition;
                imageRotTar.localEulerAngles = new Vector3(0, 0,r);
                curContents = Mathf.Max(0, curContents - PourRate * Time.deltaTime);
                break;
            case ItemState.Trans:
                sr.color = Color.black;
                tarPos = MouseData2D.Inst.mouseWorldPos - handle.localPosition;
                break;
        }
    }

    private void FixedUpdate()
    {
        CalculateLiquid();

        image.rotation = Quaternion.Lerp(image.rotation, imageRotTar.rotation, rotLerp * Time.fixedDeltaTime);
        switch (itemState)
        {
            case ItemState.Held:
                transform.position = Vector3.Lerp(transform.position, tarPos, dragLerp * Time.fixedDeltaTime);
                break;
            case ItemState.Pour:
              
                transform.position = Vector3.Lerp(transform.position, tarPos, dragLerp * Time.fixedDeltaTime);
                contentTM.text = Mathf.Floor(curContents).ToString();
                break;
            case ItemState.Idle:  
                break;
            case ItemState.Trans:       
                transform.position = Vector3.Lerp(transform.position, tarPos, dragLerp * Time.fixedDeltaTime);
                break;
        }
    }

    void CalculateLiquid()
    {
        float b = bottleCornerList.Min(x => x.position.y);
        float t = bottleCornerList.Max(x => x.position.y);
        float r = Mathf.Lerp(b,t, curContents / maxContents);

        liquidHinge.position = new Vector3(liquidHinge.position.x,
           r, liquidHinge.position.z);
        liquid.position = liquidHinge.position;
    }

    void SetStateHeld()
    {
        Utils.Toggle2DColliders(gameObject, false);
        rb.bodyType = RigidbodyType2D.Kinematic;
        imageRotTar.localEulerAngles = new Vector3(0, 0, 0);
        itemState = ItemState.Held;     
    }

    void SetStatePour()
    {
        itemState = ItemState.Pour;
    }

    void SetStateIdle()
    {
        Utils.Toggle2DColliders(gameObject, true);
        rb.bodyType = RigidbodyType2D.Dynamic;
        imageRotTar.localEulerAngles = new Vector3(0, 0, 0);
        sr.color = Color.white ;
        itemState = ItemState.Idle;
    }

    IEnumerator SetStateTrans(ItemState _nextState, float dur)
    {
        if (itemState == ItemState.Trans)
        {
            newTransCoroutineStarted = true;
            nextItemState = _nextState;
            yield break;
        }
        else newTransCoroutineStarted = false;
        nextItemState = _nextState;
        if (itemState == ItemState.Held && nextItemState == ItemState.Pour)
        {
            itemState = ItemState.Trans;
            pourDirection = -Mathf.Sign(curPourTar.position.x - MouseData2D.Inst.mouseWorldPos.x);

            float targetRot = (rotRange.x + (rotRange.y - rotRange.x) * BottlePourCurve.Evaluate((1 - (curContents / maxContents)))) * pourDirection;
            float startRot = imageRotTar.localEulerAngles.z;
            for (float i = 0; i < dur; i += Time.deltaTime)
            {
                if (newTransCoroutineStarted)
                {
                    newTransCoroutineStarted = false;
                    if (nextItemState == ItemState.Held)
                    {
                        SetStatePour();
                        StartCoroutine(SetStateTrans(ItemState.Held, i));
                    }
                    else if ( nextItemState == ItemState.Idle)
                    {
                        SetStatePour();
                        StartCoroutine(SetStateTrans(ItemState.Idle, i));
                    }
                    yield break;
                }
                var rotZ = Mathf.LerpAngle(startRot, targetRot, pourRotationCurve.Evaluate(i / dur));
                imageRotTar.localEulerAngles = new Vector3(0, 0, rotZ);
                yield return null;
            }
            SetStatePour();
            
        }
        else if (itemState == ItemState.Pour && _nextState == ItemState.Held)
        {
            itemState = ItemState.Trans;
            float targetRot = 0;
            float startRot = imageRotTar.localEulerAngles.z;

            for (float i = 0; i < dur; i += Time.deltaTime)
            {
                if (newTransCoroutineStarted)
                {
                    newTransCoroutineStarted = false;
                    if (nextItemState == ItemState.Pour) //reverse change back to pour
                    {
                        SetStateHeld();
                        StartCoroutine(SetStateTrans(ItemState.Pour, i));
                    }
                    else if (nextItemState == ItemState.Idle) //continue change but to Idle instead of held
                    {
                        SetStatePour();
                        StartCoroutine(SetStateTrans(ItemState.Idle, dur - i));
                    }
                    yield break;
                }

                var rotZ = Mathf.LerpAngle(startRot, targetRot, pourRotationCurve.Evaluate(i / dur));
                imageRotTar.localEulerAngles = new Vector3(0, 0, rotZ);
                yield return null;
            }
            SetStateHeld();
        }
        else if (itemState == ItemState.Pour && _nextState == ItemState.Idle)
        {
            itemState = ItemState.Trans;
            float targetRot = 0;
            float startRot = imageRotTar.localEulerAngles.z;
            Vector3 startPos = transform.position;
            float x = Mathf.Max(Mathf.Abs(transform.position.x - curPourTar.transform.position.x), minDropDist);
            Vector3 endPos = new Vector3(x*Mathf.Sign(transform.position.x - curPourTar.transform.position.x), startPos.y, startPos.z);

            for (float i = 0; i < dur; i += Time.deltaTime)
            {
                var rotZ = Mathf.LerpAngle(startRot, targetRot, pourRotationCurve.Evaluate(i / dur));
                tarPos = Vector3.Lerp(startPos, endPos, i / straightRotationDur);
                imageRotTar.localEulerAngles = new Vector3(0, 0, rotZ);
                yield return null;
            }
            SetStateIdle();
        }
        else if (itemState == ItemState.Held && _nextState == ItemState.Idle)
        {
            SetStateIdle();
        }

        yield return null;
    }

}


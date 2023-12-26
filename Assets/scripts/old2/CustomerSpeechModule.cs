using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;


public class CustomerSpeechModule : MonoBehaviour
{
    public enum TalkState
    {
        Talking,
        Silent
    }

    public TalkState talkState;

    public TextMeshPro speechText;
    TextAnimator textAnimator;
    TextAnimatorPlayer textAnimatorPlayer;
    public SpriteRenderer speechBubble;
    public Vector2 bubbleMargins = new Vector2(0.2f, 0.2f);
    float lastTextTime;
    float lastTextDur;

    public Vector2 minBubbleSize = new Vector2(2, 1);

    public Transform speechVisuals;

    private CustomerBase customerBase;

    private void Awake()
    {
        customerBase = GetComponent<CustomerBase>();
        textAnimator = speechText.GetComponent<TextAnimator>();
        textAnimatorPlayer = speechText.GetComponent<TextAnimatorPlayer>();
       // SetStateSilent();
    }

    public void EntireTextShowed()
    {
        StartCoroutine(DisappearText(speechText.text.Length * customerBase.data.textWaitPerChar));
    }

    public void EntireTextDisappeared()
    {
        Debug.Log("f2");
        speechVisuals.gameObject.SetActive(false);
    }

    IEnumerator DisappearText(float wait)
    {
        yield return new WaitForSeconds(1);
        textAnimatorPlayer.StartDisappearingText();
        yield return null;
    }

    public void SetSpeech(string speech)
    {
        textAnimatorPlayer.ShowText(speech);
        //speechText.text = speech;
        SetStateTalking();
    }

    void SetStateTalking()
    {
        lastTextDur = (float)speechText.text.Length * customerBase.data.textWaitPerChar;
        lastTextTime = Time.time;
        talkState = TalkState.Talking;
        speechVisuals.gameObject.SetActive(true);
        UpdateBubbleVisuals();
    }

    void SetStateSilent()
    {
        talkState = TalkState.Silent;
        SetSpeech("");
        MinimizeVisuals();
        speechVisuals.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (talkState == TalkState.Talking)
        {
            UpdateBubbleVisuals();
            if (Time.time - lastTextTime > lastTextDur)
            {
             //   SetStateSilent();
            }
        }
    }

    void MinimizeVisuals()
    {
        speechBubble.size = minBubbleSize;
    }

    void UpdateBubbleVisuals()
    {
      
        speechBubble.gameObject.SetActive(true);
        speechBubble.size = new Vector2(speechText.renderedWidth + bubbleMargins.x * 2, speechText.renderedHeight + bubbleMargins.y * 2);
        var x = speechText.rectTransform.anchoredPosition.x - speechText.rectTransform.sizeDelta.x * 0.5f + speechBubble.size.x * 0.5f - bubbleMargins.x * 0.5f;
        var y = speechText.rectTransform.anchoredPosition.y + speechText.rectTransform.sizeDelta.y * 0.5f - speechBubble.size.y * 0.5f + bubbleMargins.y * 0.5f;
        speechBubble.transform.localPosition = new Vector3(x, y, 0);
    }
}

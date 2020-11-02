using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DinoText : MonoBehaviour
{
    private TextMeshPro TrexTMP;

    private TextMeshPro TriceratopsTMP;

    [SerializeField]
    private string[] TRexText;

    [SerializeField]
    private string[] TriceratopsText;

    [SerializeField]
    private float instructionTime;

    [SerializeField]
    private GameObject YESButton;

    [SerializeField]
    private GameObject NOButton;

    [SerializeField]
    private AudioSource BGSound;

    private TextMeshPro currentSpeaker;
    private TextMeshPro nextSpeaker;

    private Animator TRexSpeachBubble;

    private Animator TriceratopsSpeachBubble;

    private Animator currentAnimator;
    private Animator nextAnimator;

    private int count = 0;
    private bool enableToTouch = false;
    private bool ifFirstTap = true;

    void Update() 
    {
        if(Input.touchCount > 0 && enableToTouch)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended)
            {
                StartCoroutine(NextDialogue());
            }
        }
            
    }

    IEnumerator NextDialogue()
    {
        if(ifFirstTap == true)
        {
            yield return null;
            ifFirstTap = false;
        }

        else
        {
            nextAnimator.SetTrigger("CloseText");
        }
        
        enableToTouch = false;
        yield return new WaitForSeconds(.3f);
        currentAnimator.SetTrigger("StopTyping");

        if(currentSpeaker == TriceratopsTMP)
        {
            TriceratopsTMP.text = TriceratopsText[count];
        }

        else
        {
            TrexTMP.text = TRexText[count];

            if(count == 11)
            {
                yield return new WaitForSeconds(5f);
                StartCoroutine(ShowAnswers());
                yield break;
            }

            count++;
        }

        Switch();

        yield return new WaitForSeconds(2.5f);
         
        currentAnimator.SetTrigger("StartTyping");
        yield return new WaitForSeconds(1f);
        enableToTouch = true;
        yield break;
    }

    private void Switch()
    {
        Animator bufferAnimator;
        TextMeshPro bufferSpeaker;

        bufferAnimator = currentAnimator;
        currentAnimator = nextAnimator;
        nextAnimator = bufferAnimator;

        bufferSpeaker = currentSpeaker;
        currentSpeaker = nextSpeaker;
        nextSpeaker = bufferSpeaker;
    }

    public void StartDialogue()
    {
        TrexTMP = GameObject.FindGameObjectWithTag("TRexTMP").GetComponent<TextMeshPro>();
        TRexSpeachBubble = GameObject.FindGameObjectWithTag("TRexAnimator").GetComponent<Animator>();

        TriceratopsTMP = GameObject.FindGameObjectWithTag("TriceratopsTMP").GetComponent<TextMeshPro>();
        TriceratopsSpeachBubble = GameObject.FindGameObjectWithTag("TriceratopsAnimator").GetComponent<Animator>();

        currentSpeaker = TriceratopsTMP;
        nextSpeaker = TrexTMP;

        currentAnimator = TriceratopsSpeachBubble;
        nextAnimator = TRexSpeachBubble;

        currentAnimator.SetTrigger("StartTyping");
        BGSound.Play();
        enableToTouch = true;
    }

    private IEnumerator ShowAnswers()
    {
        yield return null;
        YESButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(.3f);
        NOButton.gameObject.SetActive(true);
        yield break;
    }

    public void WrongAnswer()
    {
        TrexTMP.text = "WRONG ANSWER!";
    }

    public void HeSaidYes()
    {
        TrexTMP.text = "HE SAID YES!!!";
    }
}
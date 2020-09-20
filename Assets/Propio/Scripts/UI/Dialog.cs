using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    //size de dialogues es la cantidad de distintos dialogos que habra
    //y en cada posicion indica la cantidad de sentences que lo forman
    public int[] dialogues;
    public string[] sentences;
    private int indexDialogue, indexSentence;
    public float typingSpeed;
    public GameObject continueButton;
    public bool isTextActive;

    private void Update()
    {
        if (indexSentence > -1)
        {
            int count = 0;
            for (int i = 0; i < indexDialogue; i++)
            {
                count += dialogues[i];
            }
            if (textDisplay.text == sentences[count+indexSentence])
            {
                continueButton.SetActive(true);
            }
        }
        if (textDisplay.enabled == true)
        {
            isTextActive = true;
            Time.timeScale = 0f;
        }
        else
        {
            isTextActive = false;
            Time.timeScale = 1f;
        }
    }

    private void Start()
    {
        //StartCoroutine(Type());
        textDisplay.enabled = false;
        indexSentence = -1;
        indexDialogue = 0;
    }

    IEnumerator Type()
    {
        int count = 0;
        for (int i=0; i < indexDialogue; i++)
        {
            count += dialogues[i];
        }
        foreach (char letter in sentences[count + indexSentence].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }


    public void NextSentence()
    {
        textDisplay.enabled = true;
        continueButton.SetActive(false);



        if (indexSentence < dialogues[indexDialogue] - 1)
        {
            indexSentence++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            indexDialogue++;
            indexSentence = -1;
            continueButton.SetActive(false);
            textDisplay.enabled = false;
        }
    }

    public void ClearTextDisplay()
    {
        textDisplay.text = "";
        continueButton.SetActive(false);
        textDisplay.enabled = false;
    }

}

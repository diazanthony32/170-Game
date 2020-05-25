using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class infoMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isPaused = false;
    public GameObject preventClick;
    public TutorialManagement tutorialManagement;

    public int rotateCount = 0;
    public TextMeshProUGUI rotateCountText;
    public GameObject rotateLearnButton;

    bool buttonActive = false;


    //public bool popUI = false;

    public GameObject[] popUps;

    private void Update()
    {
        rotateCountText.text = "<color=yellow>Rotations</color>: " + rotateCount + " / 3";
        if (rotateCount >= 3 && !buttonActive) {
            print("nice");

            rotateLearnButton.GetComponentInChildren<Button>().interactable = true;
            LeanTween.alphaCanvas(rotateLearnButton.GetComponent<CanvasGroup>(), 1f, 0.0f);
            rotateLearnButton.GetComponent<CanvasGroup>().interactable = true;
            rotateLearnButton.GetComponent<TweenController>().Pulse();

            buttonActive = true;
        }
    }

    public void Resume() 
    {
        gameObject.SetActive(false);
        preventClick.SetActive(false);

        //Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause() 
    {
        gameObject.SetActive(true);
        //preventClick.SetActive(true);

        tutorialManagement.timeStopped = true;

        //Time.timeScale = 0f;
        isPaused = true;

    }

    public void Welcome() {
        popUps[0].GetComponent<TweenController>().PopInUIInfo(this);
        //preventClick.SetActive(true);

    }
    public void LearnRotate()
    {
        popUps[1].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }

    public void LearnTowers()
    {
        popUps[2].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }

}

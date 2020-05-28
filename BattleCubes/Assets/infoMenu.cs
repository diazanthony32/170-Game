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
    public GameObject towerLearnButton;

    bool rotateButtonActive = false;
    bool towerButtonActive = false;



    //public bool popUI = false;

    public GameObject[] popUps;

    private void Update()
    {
        rotateCountText.text = "<color=yellow>Rotations</color>: " + rotateCount + " / 3";
        if (rotateCount >= 3 && !rotateButtonActive) {
            print("nice");

            rotateLearnButton.GetComponentInChildren<Button>().interactable = true;
            LeanTween.alphaCanvas(rotateLearnButton.GetComponent<CanvasGroup>(), 1f, 0.0f);
            rotateLearnButton.GetComponent<CanvasGroup>().interactable = true;
            rotateLearnButton.GetComponent<TweenController>().Pulse();

            rotateButtonActive = true;
        }

        if (tutorialManagement.towerCount == 3 && !towerButtonActive)
        {
            print("nice");

            towerLearnButton.GetComponentInChildren<Button>().interactable = true;
            LeanTween.alphaCanvas(towerLearnButton.GetComponent<CanvasGroup>(), 1f, 0.0f);
            towerLearnButton.GetComponent<CanvasGroup>().interactable = true;
            towerLearnButton.GetComponent<TweenController>().Pulse();

            towerButtonActive = true;
        }
        else if (tutorialManagement.towerCount < 3 && towerButtonActive)
        {
            print("not nice");

            towerLearnButton.GetComponentInChildren<Button>().interactable = false;
            LeanTween.alphaCanvas(towerLearnButton.GetComponent<CanvasGroup>(), 0.5f, 0.0f);
            towerLearnButton.GetComponent<CanvasGroup>().interactable = false;
            towerLearnButton.GetComponent<TweenController>().CancelHighlight();

            towerButtonActive = false;
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

    public void NiceRotation()
    {
        popUps[2].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void TowersInfo()
    {
        popUps[3].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void TowersPlacement()
    {
        popUps[4].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }

}

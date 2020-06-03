using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
    public GameObject unitLearnButton;

    public GameObject tutorialFinishLearnButton;


    bool rotateButtonActive = false;
    bool towerButtonActive = false;
    bool unitButtonActive = false;

    bool finishButtonActive = false;

    bool finalSpot = false;




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


        if (tutorialManagement.unitCount == 4 && !unitButtonActive)
        {
            print("nice");

            unitLearnButton.GetComponentInChildren<Button>().interactable = true;
            LeanTween.alphaCanvas(unitLearnButton.GetComponent<CanvasGroup>(), 1f, 0.0f);
            unitLearnButton.GetComponent<CanvasGroup>().interactable = true;
            unitLearnButton.GetComponent<TweenController>().Pulse();

            unitButtonActive = true;
        }
        else if (tutorialManagement.unitCount < 4 && unitButtonActive)
        {
            print("not nice");

            unitLearnButton.GetComponentInChildren<Button>().interactable = false;
            LeanTween.alphaCanvas(unitLearnButton.GetComponent<CanvasGroup>(), 0.5f, 0.0f);
            unitLearnButton.GetComponent<CanvasGroup>().interactable = false;
            unitLearnButton.GetComponent<TweenController>().CancelHighlight();

            unitButtonActive = false;
        }

        CheckRotation();
        //if (finalSpot) {
            //print("Im in the final Position!!!");
        //}

        if (finalSpot && !finishButtonActive)
        {
            print("nice");

            tutorialFinishLearnButton.GetComponentInChildren<Button>().interactable = true;
            LeanTween.alphaCanvas(tutorialFinishLearnButton.GetComponent<CanvasGroup>(), 1f, 0.0f);
            tutorialFinishLearnButton.GetComponent<CanvasGroup>().interactable = true;
            tutorialFinishLearnButton.GetComponent<TweenController>().Pulse();

            finishButtonActive = true;
        }
        else if (!finalSpot && finishButtonActive)
        {
            print("not nice");

            tutorialFinishLearnButton.GetComponentInChildren<Button>().interactable = false;
            LeanTween.alphaCanvas(tutorialFinishLearnButton.GetComponent<CanvasGroup>(), 0.5f, 0.0f);
            tutorialFinishLearnButton.GetComponent<CanvasGroup>().interactable = false;
            tutorialFinishLearnButton.GetComponent<TweenController>().CancelHighlight();

            finishButtonActive = false;
        }


    }

    void CheckRotation()
    {

        finalSpot = false;

        List<Transform> checkSpots = new List<Transform>();
        //Array checkSpotsbools;

        bool[] checkSpotsbools = new bool[] { false, false, false };

        Transform playerTargetting = tutorialManagement.playerCubePosition.transform.GetChild(1).GetChild(2);

        Transform centerTopPlane = playerTargetting.GetChild(1).GetChild(4);
        Transform centerRightPlane = playerTargetting.GetChild(4).GetChild(4);
        Transform centerLeftPlane = playerTargetting.GetChild(4).GetChild(1);
        //print(centerTopPlane.name + " " + centerTopPlane.parent.name);

        checkSpots.Add(centerTopPlane);
        checkSpots.Add(centerRightPlane);
        checkSpots.Add(centerLeftPlane);

        for (int j = 0; j < checkSpots.Count; j++) {

            RaycastHit[] hits;
            hits = Physics.RaycastAll(checkSpots[j].transform.position, -checkSpots[j].transform.up, 0.05f);

            for (int i = 0; i < hits.Length; i++)
            {
                //stored info from the enemy's unitPlane underneath
                RaycastHit hit = hits[i];
                var hitPlane = hit.transform.gameObject;

                //print(hitPlane.name);
                //this is where we filter out the weird self hits
                if (hitPlane.tag == "unitSquare")
                {
                    if (hitPlane.transform.childCount > 0 && hitPlane.transform.GetChild(0).GetComponent<TutorialUnitInformation>())
                    {
                        //finalSpot = true;
                        checkSpotsbools[j] = true;
                    }
                    else {
                        checkSpotsbools[j] = false;
                    }

                    //checkSpotsbools.Add(false);
                    //finalSpot = false;
                }
            }
        }
        print(checkSpotsbools[0] + " " + checkSpotsbools[1] + " " + checkSpotsbools[2]);
        if (checkSpotsbools[0].Equals(true) && checkSpotsbools[1].Equals(true) && checkSpotsbools[2].Equals(true)) {
            finalSpot = true;
        }
        //checkSpotsbools.Clear();

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
        preventClick.SetActive(true);

    }
    public void TowersInfo()
    {
        popUps[3].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(true);

    }
    public void TowersPlacement()
    {
        tutorialManagement.AssignPlacementLocations();
        popUps[4].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void NiceTowersPlacement()
    {
        //tutorialManagement.AssignPlacementLocations();
        popUps[5].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void UnitInfo()
    {
        //tutorialManagement.AssignPlacementLocations();
        popUps[6].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void UnitPlacement()
    {
        tutorialManagement.AssignUnitPlacementLocations();
        popUps[7].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void NiceUnitPlacement()
    {
        //tutorialManagement.AssignUnitPlacementLocations();
        popUps[8].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void FinishSetup()
    {
        //tutorialManagement.AssignUnitPlacementLocations();
        popUps[9].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }
    public void ShowEnemyCube()
    {
        
        //tutorialManagement.AssignUnitPlacementLocations();
        tutorialManagement.remainingTime = 0;
        tutorialManagement.timeStopped = false;
        //tutorialManagement.remainingTime = 0;

        //popUps[10].GetComponent<TweenController>().PopInUIInfo(this);
        preventClick.SetActive(false);

    }

}

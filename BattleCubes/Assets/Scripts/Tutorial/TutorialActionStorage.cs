using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialActionStorage : MonoBehaviour
{
    [SerializeField] TutorialRotationByFinger rotationByFinger;
    [SerializeField] TutorialManagement gameManager;
    [SerializeField] TutorialAttackHandler attackHandler;
    List<string[]> actionList = null;
    [Space(10)]
    [SerializeField] GameObject readyButtonHighlight = null;
    [SerializeField] GameObject preventClick = null;

    Button readyButton = null;
    bool isHighlighted = false;


    void Start()
    {
        actionList = new List<string[]>();
        if (transform.tag == "playerActionList")
        {
            readyButton = readyButtonHighlight.GetComponentInChildren<Button>();
        }

        //for (int i = 0; i < 3; i++) {

        //    actionList.Add(new string[] {"Thing" + i});
        //} 

    }

    void Update()
    {
        //if (actionList.Count > 0 && transform.tag == "playerActionList") {
            
            if (readyButton != null && transform.tag == "playerActionList")
            {

                if (actionList.Count > 0 && !isHighlighted)
                {
                    //readyButton.interactable = true;
                    //LeanTween.alphaCanvas(readyButtonHighlight.GetComponent<CanvasGroup>(), 1f, 0.0f);
                    //readyButtonHighlight.GetComponent<CanvasGroup>().interactable = true;
                    readyButtonHighlight.GetComponent<TweenController>().Pulse();

                    isHighlighted = true;

                }
                else if (actionList.Count <= 0 && isHighlighted)
                {
                    //readyButton.interactable = false;
                    //LeanTween.alphaCanvas(readyButtonHighlight.GetComponent<CanvasGroup>(), 0.25f, 0.0f);
                    //readyButtonHighlight.GetComponent<CanvasGroup>().interactable = false;
                    readyButtonHighlight.GetComponent<TweenController>().CancelPulseHighlight();

                    isHighlighted = false;

                }
            }
        //}
    }

    public void StoreAction(string[] array) {
        if (actionList.Count < 5) {
            actionList.Add(array);

            if (array[0] == "rotate") {
                transform.GetChild(actionList.Count - 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BC_UI_Turn");
            }
            if (array[0] == "attack") {
                transform.GetChild(actionList.Count - 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BC_UI_Attack");
            }
            if (actionList.Count - 1 > 0) {
                transform.GetChild(actionList.Count - 2).GetComponent<TweenController>().CancelPulseHighlight();
            }
            transform.GetChild(actionList.Count - 1).GetComponent<TweenController>().PulseHighlight();
        }
    }
    public void RemoveAction(int index) {
        print("action list size: " + actionList.Count);
        if (!gameManager.IsCubeTweening()) {
            if (index == actionList.Count) {
                string action = actionList[index - 1][0];

                if (action == "rotate") {
                    StartCoroutine(preventFromClicking());
                    
                    rotationByFinger.GetRotateCube().LerpToPlannedPos();
                    gameManager.AddActionPoints(3);
                }
                if (action == "attack") {
                    TutorialUnitInformation unitInfo = attackHandler.GetAttackUnit("Host", actionList[index - 1][1]);
                    gameManager.AddActionPoints(unitInfo.attackCost);
                }

                actionList.RemoveAt(index - 1);
                transform.GetChild(index - 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BattleCubesLogo");
                transform.GetChild(index - 1).GetComponent<TweenController>().CancelPulseHighlight();

                if (actionList.Count - 1 >= 0) {
                    transform.GetChild(actionList.Count - 1).GetComponent<TweenController>().PulseHighlight();
                }
            }
        }

        print("action list size after: " + actionList.Count);
        
    }

    public void resetUIPulses(){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<TweenController>().CancelPulseHighlight();
        }
    }

    public string[][] PrepareActionListForSend() {
        string[][] array = new string[actionList.Count][];

        for (int i = 0; i < array.Length; i++) {
            array[i] = actionList[i];

            //for (int j = 0; j < array[i].Length; j++)
            //    print(array[i][j]);

            //print("\n");
        }

        return array;
    }

    //set
    public void SetActionListArray(string[][] array) {
        for (int i = 0; i < array.Length; i++) {
            StoreAction(array[i]);

            //for (int j = 0; j < array[i].Length; j++)
            //    print(array[i][j]);

            //print("\n");
        }
    }

    //get
    public int GetActionListCount() {
        return actionList.Count;
    }
    public void ClearActionList() {//it gets called after setup. client only.. weird

        for (int i = 0; i < 5; i++) {
            transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BattleCubesLogo");
            transform.GetChild(i).GetComponent<TweenController>().CancelPulseHighlight();
        }
        if (actionList != null) {
            if (actionList.Count != 0)
                actionList.Clear();
        }
    }
    public string[] GetAt(int val) {
        if (actionList != null) {
            if (actionList.Count != 0)
                return actionList[val];
        }
        // print("ERROR");
        return new string[0];
    }

    IEnumerator preventFromClicking() {


        preventClick.SetActive(true);
        attackHandler.ResetChooseAttackHandlers();

        yield return new WaitForSeconds(0.6f);

        preventClick.SetActive(false);

    }
}

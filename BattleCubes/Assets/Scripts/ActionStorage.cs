using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionStorage : MonoBehaviour
{
    [SerializeField] RotationByFinger rotationByFinger;
    [SerializeField] GameManager gameManager;
    List<string[]> actionList = null;

    void Start()
    {
        actionList = new List<string[]>();

        //for (int i = 0; i < 3; i++) {

        //    actionList.Add(new string[] {"Thing" + i});
        //} 
        
    }

    void Update()
    {
        
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
        if (index == actionList.Count) {
            string action = actionList[index - 1][0];

            if (action == "rotate") { gameManager.AddActionPoints(3); }
            if (action == "attack") { /*gameManager.AddActionPoints(3);*/ }

            actionList.RemoveAt(index - 1);
            transform.GetChild(index - 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BattleCubesLogo");
            transform.GetChild(index - 1).GetComponent<TweenController>().CancelPulseHighlight();

            if (actionList.Count - 1 >= 0) {
                transform.GetChild(actionList.Count - 1).GetComponent<TweenController>().PulseHighlight();
            }

            rotationByFinger.GetRotateCube().LerpToPlannedPos();
        }
        print("action list size after: " + actionList.Count);
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
        return new string[0];
    }
}

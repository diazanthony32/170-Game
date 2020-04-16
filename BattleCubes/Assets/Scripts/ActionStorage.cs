using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionStorage : MonoBehaviour
{
    List<string[]> actionList;

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
            actionList.RemoveAt(index - 1);
            transform.GetChild(index - 1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BattleCubesLogo");
            transform.GetChild(index - 1).GetComponent<TweenController>().CancelPulseHighlight();

            if (actionList.Count - 1 >= 0) {
                transform.GetChild(actionList.Count - 1).GetComponent<TweenController>().PulseHighlight();
            }
        }
        print("action list size after: " + actionList.Count);
    }

    //get
    public int GetActionListCount() {
        return actionList.Count;
    }
    public void ClearActionList() {

        for (int i = 0; i < 5; i++) {
            transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BattleCubesLogo");
            transform.GetChild(i).GetComponent<TweenController>().CancelPulseHighlight();
        }
        actionList.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStorage : MonoBehaviour
{
    List<string[]> actionList;

    void Start()
    {
        actionList = new List<string[]>();

        for (int i = 0; i < 3; i++) {

            actionList.Add(new string[] {"Thing" + i});
        } 
        
    }

    void Update()
    {
        
    }

    public void StoreAction(string[] array) {
        if (actionList.Count < 5) {
            actionList.Add(array);
        }
    }
    public void RemoveAction(int index) {
        print("action list size: " + actionList.Count);
        if (index == actionList.Count) {
            actionList.RemoveAt(index - 1);
        }
        print("action list size after: " + actionList.Count);
    }
}

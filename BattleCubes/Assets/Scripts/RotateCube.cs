/*Written by:   Alejandro Landaverde
 *Date Started: 01/16/2020
  
 *Contributors: Anthony Diaz
 
 *Issues:

 *To-Do:
    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RotateCube : MonoBehaviour {
    float speed = 2.5f;
    bool start = false;
    bool lerping;
    public bool snapBackStarted = false;
    public bool lerpToPlanned = false;
    Vector3 rotDir;
    float counter = 0;
    string arrow = null;

    public Quaternion basePos;
    Stack<Quaternion> plannedStack;

    string newArrowName;
    string oldArrowName;

    bool storeInfo = false;
    bool stackPush = false;

    // Start is called before the first frame update
    void Start() {
        basePos = transform.rotation;
        plannedStack = new Stack<Quaternion>();
    }

    // cube is continously checking to be rotated
    void Update() {
        if (!string.IsNullOrEmpty(arrow) && !LeanTween.isTweening(gameObject)) {
            CheckInput(arrow);
        }
        if (!rotDir.Equals( Vector3.zero)) {
            //print(rotDir);
            DoTweenRotation();
            rotDir = Vector3.zero;
        }
        if (!LeanTween.isTweening(gameObject)) {
            start = false;
        }
    }

    //checks which arrow was clicked
    private void CheckInput(string s) {
        //------- X
        if (s == "turn_L_up") {
            if (!start) {
                rotDir = Vector3.back;
                start = true;
            }
        }
        else if (s == "turn_L_down") {
            if (!start) {
                rotDir = Vector3.forward;
                start = true;
            }
        }
        //------- Y
        else if (s == "turn_R_up") {
            if (!start) {
                rotDir = Vector3.right;
                start = true;
            }
        }
        else if (s == "turn_R_down") {
            if (!start) {
                rotDir = Vector3.left;
                start = true;
            }
        }
        //--------- Z
        else if (s == "turn_left") {
            if (!start) {
                rotDir = Vector3.up;
                start = true;
            }
        }
        else {
            if (s == "turn_right") {
                if (!start) {
                    rotDir = Vector3.down;
                    start = true;
                }
            }
        }
        arrow = null;
        //resets the arrow clicked to null
    }
    void DoTweenRotation() {
        GetComponent<TweenController>().Rotate(rotDir);
    }
    void DoRotation() {
        float tmp;
        float overshot;

        if (counter >= 90) {
            overshot = counter - 90;
            transform.RotateAround(transform.position, rotDir, -overshot);
            counter = 0;

            start = false;
            if (stackPush) {
                plannedStack.Push(transform.rotation);
                stackPush = false;
            }
        }
        else {
            if (start) {
                tmp = 90.0f * Time.deltaTime * speed;
                counter += tmp;
                transform.RotateAround(transform.position, rotDir, tmp);
            }
        }

        if (snapBackStarted && !lerpToPlanned) {
            SnapTo(basePos);
            //print("snapping");
        }
        if (lerpToPlanned && !snapBackStarted) {
            LerpToPlannedPos();
        }
    }

    // assigns the clicked arrow to the variable s to be used in Update function
    public void AssignDirection(string s) {
        if (!string.IsNullOrEmpty(s) && !lerping) {
            arrow = s;
        }
    }
    public void RequestRotation(string s) {
        AssignDirection(s);
        //print("assigning direction: " + s);
    }

    // assigns the clicked arrow to the variable s to be used in Update function
    public void ArrowConfirmed() {
        if (!string.IsNullOrEmpty(newArrowName)) arrow = newArrowName;

        GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
        GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
        GameObject targetArrow = turnDirections.transform.Find(newArrowName).gameObject;

        var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();

        newArrowName = null;
    }

    public void ResetTurn() {

        if (newArrowName != null) {

            //resets the arrow highlight
            GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
            GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
            GameObject targetArrow = turnDirections.transform.Find(newArrowName).gameObject;

            var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();
        }

        newArrowName = null;
        oldArrowName = null;
    }

    public void RotationStoreB(string s) {
        List<string> rotationList = new List<string> { s };

        ResetTurn();
    }
    void SnapTo(Quaternion pos) {
        lerping = Quaternion.Angle(transform.rotation, pos) > 0.2f;
        if (lerping) {

            transform.rotation = Quaternion.Lerp(transform.rotation, pos, 0.1f);
        }
        else {
            snapBackStarted = false;
            lerpToPlanned = false;
        }
    }
    void LerpToPlannedPos() {
        if (plannedStack.Count > 0) {
            SnapTo(plannedStack.Peek());
        }
        else {
            SnapTo(basePos);
        }
        print("lerp");
    }
    public void SetBasePos() {
        basePos = transform.rotation;
    }
    public void PopOutOfPlannedStack() {
        if (plannedStack.Count > 0) {
            plannedStack.Pop();
        }
        print("popout");
    }
    public void ClearPlannedStack() {
        if (plannedStack.Count > 0) {
            plannedStack.Clear();
        }
        print("clear");
    }
    public void SaveBasePos() {
        if (plannedStack.Count > 0) {
            basePos = plannedStack.Peek();
        }
        print("save");
    }
}

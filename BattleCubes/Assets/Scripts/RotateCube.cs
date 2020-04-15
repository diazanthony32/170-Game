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
    //int saveArrow = 0;
    public bool snapBackStarted = false;
    public bool lerpToPlanned = false;
    Vector3 rotDir;
    float counter = 0;
    string arrow = null;

    public Quaternion basePos;
    //public List<Quaternion> plannedPos;
    Stack<Quaternion> plannedStack;
    //objectClicker oc;

    string newArrowName;
    string oldArrowName;

    bool storeInfo = false;
    bool stackPush = false;

    // Start is called before the first frame update
    void Start() {
        //Quaternion quat = transform.rotation;
        //print("x: " + quat.x + "y: " + quat.y + "z: " + quat.z + "w: " + quat.w);
        //oc = transform.parent.GetComponent<objectClicker>();
        basePos = transform.rotation;
        plannedStack = new Stack<Quaternion>();
    }

    // cube is continously checking to be rotated
    void Update() {
        if (!string.IsNullOrEmpty(arrow) && !LeanTween.isTweening(gameObject)) {
            CheckInput(arrow);
        }
        if (!rotDir.Equals( Vector3.zero)) {
            print(rotDir);
            DoTweenRotation();
            rotDir = Vector3.zero;
        }
        if (!LeanTween.isTweening(gameObject)) {
            start = false;
        }

        //if(newArrowName != null){
        //    //oc.turnConfirm.GameObject.SetActive(true);
        //    oc.turnConfirm.GetComponent<Button>().interactable = true;
        //}

        //DoRotation();
    }

    //checks which arrow was clicked
    private void CheckInput(string s) {
        //if (storeInfo && !start && oc.actionList.Count < 5 && oc.actionPoints >= 3) {
        //    RotationStoreB(s);
        //    stackPush = true;
        //}

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

            //if (saveArrow > 0) {
            //    basePos = transform.rotation;
            //    saveArrow--;
            //}
            start = false;
            //print(stackPush);
            if (stackPush) {
                //print("Saved Rot To the stacku!!!!!!!!!!!!!");
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
        print("assigning direction: " + s);
        //if (oc.gameManager.State > 0) {
        //    if (oc.actionPoints >= 3 && oc.actionList.Count < 5) {
        //        AssignArrow(s);
        //    }
        //}
        //else {
        //    AssignArrow(s);
        //}
    }

    // assigns the clicked arrow to the variable s to be used in Update function
    public void ArrowConfirmed() {
        if (!string.IsNullOrEmpty(newArrowName)) arrow = newArrowName;

        GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
        GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
        GameObject targetArrow = turnDirections.transform.Find(newArrowName).gameObject;

        var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();

        //targetArrowRenderer.sharedMaterial = oc.arrowMaterial[0];

        //oc.actionPoints -= 1;

        newArrowName = null;
    }

    public void HighlightArrow(string s) {

        newArrowName = s;

        if (oldArrowName != null && !(newArrowName == oldArrowName)) {
            GameObject oldUnmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
            GameObject oldTurnDirections = oldUnmoveables.transform.Find("Turn_Directions").gameObject;
            GameObject oldTargetArrow = oldTurnDirections.transform.Find(oldArrowName).gameObject;

            var oldTargetArrowRenderer = oldTargetArrow.GetComponentInChildren<Renderer>();

            //oldTargetArrowRenderer.sharedMaterial = oc.arrowMaterial[0];

        }

        GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
        GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
        GameObject targetArrow = turnDirections.transform.Find(s).gameObject;

        var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();

        print(targetArrowRenderer.sharedMaterial);
        //print(oc.arrowMaterial[1]);

        //targetArrowRenderer.sharedMaterial = oc.arrowMaterial[1];
        print("Confirm Turn?");

        oldArrowName = newArrowName;

        //saveArrow++;
    }

    public void ResetTurn() {

        if (newArrowName != null) {

            //resets the arrow highlight
            GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
            GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
            GameObject targetArrow = turnDirections.transform.Find(newArrowName).gameObject;

            var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();

            //targetArrowRenderer.sharedMaterial = oc.arrowMaterial[0];
        }

        newArrowName = null;
        oldArrowName = null;
    }

    // adds the action to the action list
    public void rotationStore() {

        //List<string> rotationList = new List<string>();

        //rotationList.Add(newArrowName);
        //oc.actionList.Add(rotationList);
        //oc.gameManager.TraverseActionList(rotationList);

        //ResetTurn();

        //oc.SetAttackSideDefault();
        //oc.SetTurnSideDefault();

        //oc.actionPoints -= 3;
    }
    public void RotationStoreB(string s) {
        List<string> rotationList = new List<string> { s };

        //oc.actionList.Add(rotationList);

        //print("---------ADDING ROTATION ACTION----------");
        //oc.gameManager.TraverseActionList(rotationList);

        ResetTurn();

        //oc.SetAttackSideDefault();
        //oc.SetTurnSideDefault();
        //oc.actionPoints -= 3;
    }
    void SnapTo(Quaternion pos) {
        lerping = Quaternion.Angle(transform.rotation, pos) > 0.2f;
        if (lerping) {
            //print("lerpiong: " + lerping);
            //print(basePos);
            //print(transform.rotation);

            //print(Quaternion.Angle(transform.rotation, basePos));

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
    //public void SetStoreInfo(bool val) {
    //    if (oc.actionList.Count < 5) {
    //        storeInfo = val;
    //    }
    //}
    //public void SetSnapBack(bool val) {
    //    if (oc.gameManager.State > 0) {
    //        snapBackStarted = val;
    //    }
    //}
    //public void SetSnapToPlanned(bool val) {
    //    if (oc.gameManager.State > 0) {
    //        lerpToPlanned = val;
    //    }
    //}
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

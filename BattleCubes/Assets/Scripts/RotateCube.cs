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
using UnityEngine.SceneManagement;

public class RotateCube : MonoBehaviour {
    float speed = 2.5f;
    bool start = false;
    bool lerping;
    public bool snapBackStarted = false;
    public bool lerpToPlanned = false;
    //Vector3 rotDir;
    float counter = 0;
    string arrow = null;

    public Quaternion basePos;
    Stack<string> plannedStack;

    string newArrowName;
    string oldArrowName;

    GameManager gameManager;
    bool storeInfo = false;
    bool stackPush = false;

    // Start is called before the first frame update
    void Start() {
        basePos = transform.rotation;
        plannedStack = new Stack<string>();

        if (SceneManager.GetActiveScene().buildIndex == 1) {
            gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        }
    }

    // cube is continously checking to be rotated
    void Update() {
        //if (!string.IsNullOrEmpty(arrow) && !LeanTween.isTweening(gameObject)) {
        //    CheckInput(arrow);
        //}
        //if (!rotDir.Equals( Vector3.zero)) {
        //    //print(rotDir);
        //    DoTweenRotation();
        //    rotDir = Vector3.zero;
        //}
        //if (!LeanTween.isTweening(gameObject)) {
        //    start = false;
        //}
    }

    //checks which arrow was clicked
    private Vector3 GetVecDirFromStringDir(string s) {
        //------- X
        if (s == "turn_L_up") {
            return Vector3.back;
        }
        else if (s == "turn_L_down") {
            return Vector3.forward;
        }
        //------- Y
        else if (s == "turn_R_up") {
            return Vector3.right;
        }
        else if (s == "turn_R_down") {
            return Vector3.left;
        }
        //--------- Z
        else if (s == "turn_left") {
            return Vector3.up;
        }
        else if (s == "turn_right") {
            return Vector3.down;
        }
        else {
            return Vector3.zero;
        }
    }
    //void DoRotation() {
    //    float tmp;
    //    float overshot;

    //    if (counter >= 90) {
    //        overshot = counter - 90;
    //        transform.RotateAround(transform.position, rotDir, -overshot);
    //        counter = 0;

    //        start = false;
    //        if (stackPush) {
    //            plannedStack.Push(transform.rotation);
    //            stackPush = false;
    //        }
    //    }
    //    else {
    //        if (start) {
    //            tmp = 90.0f * Time.deltaTime * speed;
    //            counter += tmp;
    //            transform.RotateAround(transform.position, rotDir, tmp);
    //        }
    //    }

    //    if (snapBackStarted && !lerpToPlanned) {
    //        SnapTo(basePos);
    //        //print("snapping");
    //    }
    //    if (lerpToPlanned && !snapBackStarted) {
    //        LerpToPlannedPos();
    //    }
    //}

    // assigns the clicked arrow to the variable s to be used in Update function
    public void AssignDirection(string s) {
        
        //if (!string.IsNullOrEmpty(s) && !lerping) {
        //    arrow = s;
        //}
    }
    public void RequestRotation(string s) {
        if (!LeanTween.isTweening(gameObject)) {
            DoTweenRotation(s);
        }
        //AssignDirection(s);
        //print("assigning direction: " + s);
    }

    // assigns the clicked arrow to the variable s to be used in Update function
    //public void ArrowConfirmed() {
    //    if (!string.IsNullOrEmpty(newArrowName)) arrow = newArrowName;

    //    GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
    //    GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
    //    GameObject targetArrow = turnDirections.transform.Find(newArrowName).gameObject;

    //    var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();

    //    newArrowName = null;
    //}

    //public void ResetTurn() {

    //    if (newArrowName != null) {

    //        //resets the arrow highlight
    //        GameObject unmoveables = transform.parent.gameObject.transform.Find("Unmoveables").gameObject;
    //        GameObject turnDirections = unmoveables.transform.Find("Turn_Directions").gameObject;
    //        GameObject targetArrow = turnDirections.transform.Find(newArrowName).gameObject;

    //        var targetArrowRenderer = targetArrow.GetComponentInChildren<Renderer>();
    //    }

    //    newArrowName = null;
    //    oldArrowName = null;
    //}

    //public void RotationStoreB(string s) {
    //    List<string> rotationList = new List<string> { s };

    //    ResetTurn();
    //}
    //void SnapTo(Quaternion pos) {
    //    lerping = Quaternion.Angle(transform.rotation, pos) > 0.2f;
    //    if (lerping) {

    //        transform.rotation = Quaternion.Lerp(transform.rotation, pos, 0.1f);
    //    }
    //    else {
    //        snapBackStarted = false;
    //        lerpToPlanned = false;
    //    }
    //}

    void DoTweenRotation(string s) {
        Vector3 vecDirection = GetVecDirFromStringDir(s);

        if (SceneManager.GetActiveScene().buildIndex == 1) {
            if (gameManager.GetState() == gameManager.PLAN) {
                GetComponent<TweenController>().Rotate(vecDirection);
                plannedStack.Push(s);
                gameManager.AddAction(new string[] { "rotate", s }, 3);
            }
            else {
                GetComponent<TweenController>().Rotate(vecDirection);
            }
        }
        else {
            GetComponent<TweenController>().Rotate(vecDirection);
        }
    }

    public void LerpToPlannedPos() {
        if (plannedStack.Count > 0) {
            //print("lerp to: " + plannedStack.Peek());

            GetComponent<TweenController>().Rotate(GetVecDirFromStringDir(GetOpositeRotation(plannedStack.Pop())));

            //plannedStack.Pop();
            //if (plannedStack.Count > 0) {
            //    //gameObject.GetComponent<TweenController>().RotateBack(plannedStack.Peek());
            //    GetComponent<TweenController>().Rotate();
            //}
            //else {
            //    gameObject.GetComponent<TweenController>().RotateBack(basePos);
            //}
        }
        else {
            gameObject.GetComponent<TweenController>().RotateBack(basePos);
            print("lerp to base");
        }
    }
    public void SnapToBaseRotation() {
        gameObject.GetComponent<TweenController>().RotateBack(basePos);
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
    //public void SaveBasePos() {
    //    if (plannedStack.Count > 0) {
    //        basePos = plannedStack.Peek();
    //    }
    //    print("save");
    //}
    //public void PushToPlannedStack(string rotation) {
    //    plannedStack.Push(rotation);
    //}

    string GetOpositeRotation(string s) {
        //------- X
        if (s == "turn_L_up") {
            return "turn_L_down";
        }
        else if (s == "turn_L_down") {
            return "turn_L_up";
        }
        //------- Y
        else if (s == "turn_R_up") {
            return "turn_R_down";
        }
        else if (s == "turn_R_down") {
            return "turn_R_up";
        }
        //--------- Z
        else if (s == "turn_left") {
            return "turn_right";
        }
        else if (s == "turn_right") {
            return "turn_left";
        }
        else {
            return "";
        }
    }
}

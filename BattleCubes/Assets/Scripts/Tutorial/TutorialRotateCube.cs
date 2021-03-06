﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialRotateCube : MonoBehaviour {
    Quaternion basePos;
    Stack<string> plannedStack;
    TutorialManagement gameManager;
    infoMenu infoMenu;

    void Start() {
        basePos = transform.rotation;
        plannedStack = new Stack<string>();

        if (SceneManager.GetActiveScene().buildIndex == 2) {
            gameManager = GameObject.FindGameObjectWithTag("tutorialManager").GetComponent<TutorialManagement>();
            infoMenu = GameObject.FindGameObjectWithTag("infoMenu").GetComponent<infoMenu>();

        }
    }

    public void RequestRotation(string s) {
        if (!LeanTween.isTweening(gameObject)) {
            DoTweenRotation(s);
            if (infoMenu) {
                infoMenu.rotateCount++;
            }
        }
    }

    void DoTweenRotation(string s) {
        Vector3 vecDirection = GetVecDirFromStringDir(s);

        if (SceneManager.GetActiveScene().buildIndex == 2) {
            if (gameManager.GetState() == gameManager.PLAN) {
                if (s == "turn_left") {
                    gameManager.popUps[3].GetComponent<TweenController>().HideUI();

                    GetComponent<TweenController>().Rotate(vecDirection);
                    plannedStack.Push(s);
                    gameManager.AddAction(new string[] { "rotate", s }, 3);
                    gameManager.popUps[2].GetComponent<TweenController>().PopInUI();
                }
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
            GetComponent<TweenController>().Rotate(GetVecDirFromStringDir(GetOpositeRotation(plannedStack.Pop())));
        }
        else {
            gameObject.GetComponent<TweenController>().RotateBack(basePos);
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
    }
    public void ClearPlannedStack() {
        if (plannedStack.Count > 0) {
            plannedStack.Clear();
        }
    }

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

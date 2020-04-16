﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RotationByFinger : MonoBehaviour {
    [SerializeField] GameObject cubeLocation;
    RotateCube rotateCube;
    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;

    bool rotAllowed = false;
    bool horizontal;

    public void Start() {
        rotateCube = cubeLocation.transform.GetChild(0).GetComponent<RotateCube>();

        if (SceneManager.GetActiveScene().name == "MainMenu") {
            rotAllowed = true;
        }
    }

    void Update() {
    }
    public void AttemptRotate(Vector2 final, Vector2 initial, int index) {
        if (rotAllowed) {
            posDelta = final - initial;
            RotateBySteps(final, initial, index);
        }
    }
    float AbsIt(float val) {
        return (val > 0) ? val : -val;
    }
    public void ChangeCube(GameObject cube) {
        rotateCube = cube.GetComponent<RotateCube>();
    }
    void RotateBySteps(Vector2 final, Vector2 initial, int index) {
        horizontal = AbsIt(posDelta.x) > AbsIt(posDelta.y);

        if (horizontal) {
            if (final.x > initial.x) {
                rotateCube.RequestRotation("turn_right");
            }
            else {
                rotateCube.RequestRotation("turn_left");
            }
        }
        else {
            if (index > 0) {
                if (final.y > initial.y) {
                    rotateCube.RequestRotation("turn_R_up");
                }
                else {
                    rotateCube.RequestRotation("turn_R_down");
                }
            }
            else {
                if (final.y > initial.y) {
                    rotateCube.RequestRotation("turn_L_up");
                }
                else {
                    rotateCube.RequestRotation("turn_L_down");
                }
            }
        }
    }
    public Vector3 GetPosDelta() {
        return posDelta;
    }
    public Vector3 GetPrevPos() {
        return prevPos;
    }
    public void SetPosDelta(Vector3 val) {
        posDelta = val;
    }
    public void SetPrevPos(Vector3 val) {
        posDelta = val;
    }
    public void SetRotAllowed(bool val) {
        rotAllowed = val;
    }
}
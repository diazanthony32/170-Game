using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialRotationByFinger : MonoBehaviour {
    [SerializeField] bool isMainMenu;
    [SerializeField] bool isSetup;
    [SerializeField] bool isMainGameScene;
    [Space(10)]
    [SerializeField] GameObject cubeLocation;
    [SerializeField] TutorialActionStorage actionStorage;
    [SerializeField] TutorialManagement gameManager;
    TutorialRotateCube rotateCube;
    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;

    readonly int MINDRAG = 125;

    bool rotAllowed = false;
    bool horizontal;

    public void Start() {
        rotateCube = cubeLocation.transform.GetComponentInChildren<TutorialRotateCube>();
        print(rotateCube.transform.name);

        if (isMainMenu) {
            rotAllowed = true;
        }
    }

    void Update() {
    }
    public void AttemptRotate(Vector2 final, Vector2 initial, int index) {
        if (rotAllowed) {

            if (isMainGameScene) {
                if (actionStorage.GetActionListCount() < 5 && gameManager.GetActionPoints() >= 3) {
                    posDelta = final - initial;
                    //print(posDelta.magnitude);
                    if (posDelta.magnitude >= MINDRAG) {
                        DetermineDirection(final, initial, index);
                    }
                }
            }
            else {
                if (isMainMenu || isSetup) {
                    posDelta = final - initial;
                    //print(posDelta.magnitude);
                    if (posDelta.magnitude >= MINDRAG) {
                        DetermineDirection(final, initial, index);
                    }
                }
            }
            
        }
    }
    float AbsIt(float val) {
        return (val > 0) ? val : -val;
    }
    public void ChangeCube(GameObject cube) {
        rotateCube = cube.GetComponentInChildren<TutorialRotateCube>();
    }
    void DetermineDirection(Vector2 final, Vector2 initial, int index) {
        string[] actionArray;

        horizontal = AbsIt(posDelta.x) > AbsIt(posDelta.y);

        if (horizontal) {
            if (final.x > initial.x) {
                rotateCube.RequestRotation("turn_right");
                actionArray = new string[] {"rotate", "turn_right"};
            }
            else {
                rotateCube.RequestRotation("turn_left");
                actionArray = new string[] { "rotate", "turn_left" };
                //actionStorage.StoreAction(new string[] { "rotate", "turn_left" });
            }
        }
        else {
            if (index > 0) {
                if (final.y > initial.y) {
                    rotateCube.RequestRotation("turn_R_up");
                    actionArray = new string[] { "rotate", "turn_R_up" };

                    //actionStorage.StoreAction(new string[] { "rotate", "turn_R_up" });
                }
                else {
                    rotateCube.RequestRotation("turn_R_down");
                    actionArray = new string[] { "rotate", "turn_R_down" };

                    //actionStorage.StoreAction(new string[] { "rotate", "turn_R_down" });
                }
            }
            else {
                if (final.y > initial.y) {
                    rotateCube.RequestRotation("turn_L_up");
                    actionArray = new string[] { "rotate", "turn_L_up" };

                    //actionStorage.StoreAction(new string[] { "rotate", "turn_L_up" });
                }
                else {
                    rotateCube.RequestRotation("turn_L_down");
                    actionArray = new string[] { "rotate", "turn_L_down" };

                    //actionStorage.StoreAction(new string[] { "rotate", "turn_L_down" });
                }
            }
        }
        //if (isMainGameScene && !LeanTween.isTweening(rotateCube.gameObject)) {
        //    actionStorage.StoreAction(actionArray);
        //    gameManager.AddActionPoints(-3);
        //}
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
    public TutorialRotateCube GetRotateCube() {
        return rotateCube;
    }
}

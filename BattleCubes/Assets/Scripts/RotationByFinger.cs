using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationByFinger : MonoBehaviour {
    [SerializeField] GameObject cubeLocation;
    RotateCube rotateCube;
    [SerializeField] Swiper leftSwiper;
    [SerializeField] Swiper rightSwiper;
    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;

    readonly Swiper[] swipers = new Swiper[2];
    bool rotAllowed = true;
    bool horizontal;

    public void Start() {
        //swipers[0] = transform.Find("leftSwiper").GetComponent<Swiper>();//transform.GetChild(0).GetComponent<Swiper>();
        //swipers[1] = transform.Find("rightSwiper").GetComponent<Swiper>();//transform.GetChild(0).GetComponent<Swiper>();

        swipers[0] = leftSwiper;
        swipers[1] = rightSwiper;

        rotateCube = cubeLocation.transform.GetChild(0).GetComponent<RotateCube>();
        //rotateCube = cubeLocation.transform.GetChild(0).GetComponent<RotateCube>();
        //if (swipers[0] != null) {
        //}
        //else {
        //    print("OH NO no swipers");
        //}
        //if (swipers[1] != null) {
        //}
        //else {
        //    print("OH NO no swipers");
        //}
    }

    void Update() {
        if (rotAllowed) {
            if (swipers[0].GetMouseDown()) {
                //do stuff to left
               // print("swiping left");
                TrackMouse(0);
            }
            if (swipers[1].GetMouseDown()) {
                //do stuff to right
                //print("swiping right");

                TrackMouse(1);
            }
        }
    }
    void TrackMouse(int s) {
        posDelta = (swipers[s].GetClickPressed()) ? Vector3.zero : Input.mousePosition - prevPos;
        swipers[s].setClickPressed(false);//change to setter


        RotateBySteps(s);

        prevPos = Input.mousePosition;
    }
    float AbsIt(float val) {
        return (val > 0) ? val : -val;
    }
    public void SetRotAllowed(bool val) {
        rotAllowed = val;
    }
    public void ChangeCube(GameObject cube) {
        rotateCube = cube.GetComponent<RotateCube>();
    }
    void RotateBySteps(int s) {
        horizontal = AbsIt(posDelta.x) > AbsIt(posDelta.y);

        if (horizontal) {
            if (prevPos.x < Input.mousePosition.x) {
                rotateCube.RequestRotation("turn_right");
            }
            else {
                rotateCube.RequestRotation("turn_left");
            }
        }
        if (AbsIt(posDelta.x) < AbsIt(posDelta.y)) {
            if (prevPos.y < Input.mousePosition.y) {
                if (s > 0) {
                    rotateCube.RequestRotation("turn_R_up");
                }
                else {
                    rotateCube.RequestRotation("turn_L_up");
                }
            }
            else {
                if (s > 0) {
                    rotateCube.RequestRotation("turn_R_down");
                }
                else {
                    rotateCube.RequestRotation("turn_L_down");
                }
            }
        }
    }
}

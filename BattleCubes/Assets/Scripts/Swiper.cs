using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Swiper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    bool mouseDown = false;
    bool clickPressed = true;

    public void OnPointerDown(PointerEventData eventData) {
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        mouseDown = false;
        clickPressed = true;
    }
    //get
    public bool GetClickPressed() {
        return clickPressed;
    }
    public bool GetMouseDown() {
        return mouseDown;
    }
    //set
    public void setMouseDown(bool val) {
        mouseDown = val;
    }
    public void setClickPressed(bool val) {
        clickPressed = val;
    }
}

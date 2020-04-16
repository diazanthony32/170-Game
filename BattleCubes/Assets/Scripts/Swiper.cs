using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Swiper : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [SerializeField] int index = 0;
    RotationByFinger rotationController;
    Vector2 final;
    Vector2 initial;

    void Start() {
        rotationController = gameObject.GetComponentInParent<RotationByFinger>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        initial = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData) {
        final = eventData.position;
        
        rotationController.AttemptRotate(final, initial, index);
    }
}

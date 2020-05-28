using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TutorialSwiper : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [SerializeField] int index = 0;
    TutorialRotationByFinger rotationController;
    Vector2 final;
    Vector2 initial;

    void Start() {
        rotationController = gameObject.GetComponentInParent<TutorialRotationByFinger>();
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

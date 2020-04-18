using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DragNDropHandler : MonoBehaviour, IDragHandler , IBeginDragHandler, IEndDragHandler
{

	[SerializeField] GameObject unitPrefab;
	UnitInformation unitInformation;

	TweenController tweenController;

	GameObject unitIconParent;
	Image unitImage;
	TextMeshProUGUI unitName;

    // Start is called before the first frame update
    void Start(){

    	unitInformation = unitPrefab.GetComponent<UnitInformation>();
    	tweenController = GetComponent<TweenController>();

    	unitIconParent = transform.GetChild(0).GetChild(0).gameObject;

    	unitImage = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
    	unitImage.sprite = unitInformation.unitImage;

    	unitName = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    	unitName.text = unitInformation.unitName + " \n " + "Cost to Place: "+ unitInformation.unitSpawnCost;

        
    }

    // Update is called once per frame
    void Update(){
        
    }

    // runs when the player clicks an attack to do
	// public void OnPointerClick(PointerEventData eventData)
	// {
	// 	print("Can Interact?");

	// }

    // changes the color of the unitPlacer button when a unit starts a drag
	public void OnBeginDrag(PointerEventData eventData){
		print("Can Interact?");
		tweenController.Highlight();
	}

    // updates continously while a player is dragging
	public void OnDrag(PointerEventData eventData){
		// updates the units image to the players position
		unitImage.transform.position = eventData.position;

	}

	// only runs once player stops dragging
	public void OnEndDrag(PointerEventData eventData){
    	unitImage.transform.position = unitIconParent.transform.position;
    	tweenController.CancelPulseHighlight();
    }
}

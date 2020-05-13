using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DragNDropHandler : MonoBehaviour, IDragHandler , IBeginDragHandler, IEndDragHandler
{
	[SerializeField] string unitFolder;
	GameObject unitPrefab;
	GameManager gameManager;

	InfoSender infoSender;

	UnitInformation unitInformation;

	TweenController tweenController;

	GameObject unitIconParent;
	Image unitImage;
	TextMeshProUGUI unitName;

	TextMeshProUGUI unitCost;

	bool active = true;

	[SerializeField] GameObject readyButtonHighlight = null;
	Button readyButton = null;

	bool isActive = false;

	Color m_MouseOverColor = Color.yellow;

	//This stores the GameObject’s original color
	Color planeOriginalColor;

	//Get the GameObject’s mesh renderer to access the GameObject’s material and color
	MeshRenderer m_Renderer;

	List<Transform> oldPlanes;

	// Start is called before the first frame update
	void Start(){

    	// unitPrefab = Resources.Load<GameObject>(enemyCubeInfo[0] + "/" + enemyCubeInfo[1] + "/Units/" + unitArray[0] + "/Prefab");

    	string[] cubeInfo = {PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor")};

    	unitPrefab = Resources.Load<GameObject>("Themes/" + cubeInfo[0] + "/Units/"+ unitFolder + "/Prefab");

    	gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

    	infoSender = GameObject.FindGameObjectWithTag("infoSender").GetComponent<InfoSender>();

    	unitInformation = unitPrefab.GetComponent<UnitInformation>();
    	tweenController = GetComponent<TweenController>();

    	unitIconParent = transform.GetChild(0).GetChild(0).gameObject;

    	unitImage = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
    	unitImage.sprite = unitInformation.unitImage;

		readyButton = readyButtonHighlight.GetComponentInChildren<Button>();

		oldPlanes = new List<Transform>();

		if (!unitInformation.isTower)
		{
			//print(transform.parent.GetChild(1).GetChild(1).GetChild(2).name);
			unitCost = transform.parent.GetChild(1).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
			//print(transform.parent.name);
			unitCost.text = unitInformation.unitSpawnCost.ToString();
		}
		//unitName = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

		//if(unitInformation.isTower){
		//unitName.text = unitInformation.unitName /*+ " \n <color=yellow>" + "Need to Place: 3"*/;
		//}
		//else{
		//unitName.text = unitInformation.unitName /*+ " \n <color=yellow>" + "Cost to Place: "+ unitInformation.unitSpawnCost*/;
		//}
		// unitName.text = unitInformation.unitName + " \n " + "Cost to Place: "+ unitInformation.unitSpawnCost;


	}

    // Update is called once per frame
    void Update(){

    	if(!unitInformation.isTower){
    		if((gameManager.remainingUnitPoints < unitInformation.unitSpawnCost) && active){
	        	LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0.25f, 0.4f);
	        	active = false;
	        }
	        else if((gameManager.remainingUnitPoints >= unitInformation.unitSpawnCost) && !active){
	        	LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 1f, 0.4f);
	        	active = true;
	        }
    	}
    	else if(unitInformation.isTower){
    		if((gameManager.towerCount == 3) && active){
	        	LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0.25f, 0.4f);
	        	active = false;
	        }
	        else if((gameManager.towerCount < 3) && !active){
	        	LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 1f, 0.4f);
	        	active = true;
	        }
    	}

    	if(readyButton != null){

    		if((gameManager.remainingUnitPoints == 0) && (gameManager.towerCount == 3) && !isActive)
			{
    			readyButton.interactable = true;
				LeanTween.alphaCanvas(readyButtonHighlight.GetComponent<CanvasGroup>(), 1f, 0.0f);
				readyButtonHighlight.GetComponent<CanvasGroup>().interactable = true;
				readyButtonHighlight.GetComponent<TweenController>().Pulse();

				isActive = true;

			}
			else if(((gameManager.remainingUnitPoints != 0) || (gameManager.towerCount != 3)) && isActive)
			{
    			readyButton.interactable = false;
				LeanTween.alphaCanvas(readyButtonHighlight.GetComponent<CanvasGroup>(), 0.25f, 0.0f);
				readyButtonHighlight.GetComponent<CanvasGroup>().interactable = false;
				readyButtonHighlight.GetComponent<TweenController>().CancelHighlight();

				isActive = false;

			}
		}

		if (unitInformation.isTower) {
			//unitName.text = ag /*+ " \n <color=yellow>" + "Need to Place: " + (3 - gameManager.towerCount)*/;
			GameObject.FindGameObjectWithTag("towerText").GetComponent<TextMeshProUGUI>().text = "Towers Remaining: <color=yellow>" + (3 - gameManager.towerCount);
		}
    	
    }

    // runs when the player clicks an attack to do
	// public void OnPointerClick(PointerEventData eventData)
	// {
	// 	print("Can Interact?");

	// }

    // changes the color of the unitPlacer button when a unit starts a drag
	public void OnBeginDrag(PointerEventData eventData){
		if(active){
			tweenController.Highlight();
		}
	}

    // updates continously while a player is dragging
	public void OnDrag(PointerEventData eventData){
		// updates the units image to the players position
		if (active)
		{
			unitImage.transform.position = eventData.position;

			var tempColor = unitImage.color;
			tempColor.a = 0.5f;
			unitImage.color = tempColor;

			//if (transform.parent.gameObject.tag == "PlayerCubePosition")
			//{

				RaycastHit[] hits;
				hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 5.0f);

				if (hits.Length != 0)
				{

					for (int i = 0; i < hits.Length; i++)
					{
						RaycastHit hitPlane = hits[i];

						if (hitPlane.transform.gameObject.tag == "unitSquare")
						{
							if (oldPlanes.Count < 1)
							{
								// Change the color of the GameObject to red when the mouse is over GameObject
								planeOriginalColor = hitPlane.transform.GetComponent<MeshRenderer>().material.color;
								hitPlane.transform.GetComponent<MeshRenderer>().material.color = m_MouseOverColor;
								// m_OriginalColor = hitPlane.transform.GetComponent<MeshRenderer>().material.color;

								oldPlanes.Add(hitPlane.transform);
							}

							break;
						}
						else
						{
							// Reset the color of the GameObject back to normal
							//hitPlane.GetComponent<MeshRenderer>() = m_OriginalColor;
							for (int j = 0; j < oldPlanes.Count; j++)
							{
								Transform plane = oldPlanes[j];
								plane.GetComponent<MeshRenderer>().material.color = planeOriginalColor;

								oldPlanes.RemoveAt(j);
							}
							//hitPlane.transform.GetComponent<MeshRenderer>().material.color = Color.blue;
						}

					}

				}
				else
				{
					// Reset the color of the GameObject back to normal
					//hitPlane.GetComponent<MeshRenderer>() = m_OriginalColor;
					for (int j = 0; j < oldPlanes.Count; j++)
					{
						Transform plane = oldPlanes[j];
						plane.GetComponent<MeshRenderer>().material.color = planeOriginalColor;

						oldPlanes.RemoveAt(j);
					}
					//hitPlane.transform.GetComponent<MeshRenderer>().material.color = Color.blue;
				}

			//}
		}

	}

	// only runs once player stops dragging
	public void OnEndDrag(PointerEventData eventData){

		for (int j = 0; j < oldPlanes.Count; j++)
		{
			Transform plane = oldPlanes[j];
			plane.GetComponent<MeshRenderer>().material.color = planeOriginalColor;

			oldPlanes.RemoveAt(j);
		}

		if (active){
	    	unitImage.transform.position = unitIconParent.transform.position;
	    	tweenController.CancelPulseHighlight();

			var tempColor = unitImage.color;
			tempColor.a = 1.0f;
			unitImage.color = tempColor;

			if (gameManager.GetState() == gameManager.SETUP && gameManager.remainingUnitPoints >= unitInformation.unitSpawnCost){

	    		RaycastHit[] hits;
				hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 5.0f);

				for (int i = 0; i < hits.Length; i++){
					RaycastHit hitPlane = hits[i];
					
					if(!unitInformation.isTower && hitPlane.transform.gameObject.tag == "unitSquare" && hitPlane.transform.childCount < 1)
					{
						print("player hovered over a target");

						var unit = Instantiate(unitPrefab);

						unit.transform.position = hitPlane.transform.position;
						unit.transform.rotation = hitPlane.transform.rotation;

						var rand = Random.Range(0, 4);

						//unit.transform.Translate(0.0f, 0.0f, 0.0f);
						unit.transform.Rotate(0.0f,(rand * 90.0f), 0.0f);
						// Set unit as a child of the unitPlane
						unit.transform.SetParent(hitPlane.transform);
						//gameManager.remainingUnitPoints -= unitInformation.unitSpawnCost;
						gameManager.AddUnitPoints(-unitInformation.unitSpawnCost);
						// gameManager.unitCount++;
						gameManager.AddUnitCount(1);

						infoSender.SendUnitPlacement(new string[]{unitInformation.folder, hitPlane.transform.parent.name, hitPlane.transform.name});

						break;
					}
					else if(unitInformation.isTower && hitPlane.transform.gameObject.tag == "unitSquare" && hitPlane.transform.childCount < 1 && gameManager.towerCount < 3)
					{

						// the variable responible for allowing the Tower to be Placed on a face
						bool safeToPlace = true;

						// checks if there is another tower on the same face of where the tower wants to be placed
						// if so, the tower placement is invalid
						for(int y = 0; y < hitPlane.transform.parent.childCount; y++) {

							var plane = hitPlane.transform.parent.GetChild(y);

							if(plane.gameObject.transform.childCount > 0 && plane.gameObject.CompareTag("unitSquare"))
							{
								for(int x = 0 ; x < plane.gameObject.transform.childCount ; x++ ){

									if((plane.transform.GetChild(x).gameObject.GetComponent<UnitInformation>().isTower)){
										safeToPlace = false;
									}

								}

							}

						}

						if(safeToPlace){

							print("player hovered over a target");

							var unit = Instantiate(unitPrefab);

							unit.transform.position = hitPlane.transform.position;
							unit.transform.rotation = hitPlane.transform.rotation;

							var rand = Random.Range(0, 4);

							//unit.transform.Translate(0.0f, 0.0f, 0.0f);
							unit.transform.Rotate(0.0f,(rand * 90.0f), 0.0f);
							// Set unit as a child of the unitPlane
							unit.transform.SetParent(hitPlane.transform);

							gameManager.AddTowerCount(1);

							infoSender.SendUnitPlacement(new string[]{unitInformation.folder, hitPlane.transform.parent.name, hitPlane.transform.name});

							//unitName.text = unitInformation.unitName + " \n <color=yellow>" + "Need to Place: " + (3-gameManager.towerCount);

						}

						break;
					}
					else{
						print("player did not hover over a target");
					}
				}
	    	}
    	}
    }
}

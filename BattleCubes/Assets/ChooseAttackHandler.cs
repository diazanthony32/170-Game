using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class ChooseAttackHandler : MonoBehaviour, IPointerDownHandler
{

	[SerializeField] string unitFolder;

	public Button ConfirmButton;

	//[SerializeField] Gradient colorQuad;
	GameObject unitPrefab;
	GameManager gameManager;

	InfoSender infoSender;

	UnitInformation unitInformation;

	TweenController tweenController;

	GameObject unitIconParent;
	Image unitImage;
	TextMeshProUGUI unitName;

	string[] enemyCubeInfo;

	public bool active = true;

	public bool isSelected = false;

	public bool attackAllowed = false;

	string[] attackArray = null;

	AttackHandler attackHandler;

	List<GameObject> oldTargets = new List<GameObject>();

	GameObject selectedPlane = null;

    // Start is called before the first frame update
    void Start()
    {
        // unitPrefab = Resources.Load<GameObject>(enemyCubeInfo[0] + "/" + enemyCubeInfo[1] + "/Units/" + unitArray[0] + "/Prefab");

        attackHandler = transform.parent.GetComponent<AttackHandler>();

    	string[] cubeInfo = {PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor")};

    	unitPrefab = Resources.Load<GameObject>(cubeInfo[0] + "/" + cubeInfo[1] + "/Units/" + unitFolder + "/Prefab");

    	gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

    	infoSender = GameObject.FindGameObjectWithTag("infoSender").GetComponent<InfoSender>();

    	unitInformation = unitPrefab.GetComponent<UnitInformation>();
    	tweenController = GetComponent<TweenController>();

    	unitIconParent = transform.GetChild(0).GetChild(0).gameObject;

    	unitImage = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
    	unitImage.sprite = unitInformation.unitImage;

    	unitName = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

    	unitName.text = unitInformation.attackName + " \n <color=yellow>" + "Cost to Use: " + unitInformation.attackCost;
    }

    //Update is called once per frame
    void Update()
    {
		if (gameManager.attackList[Convert.ToInt32(unitFolder)-1][0] == unitInformation.attackName && gameManager.attackList[Convert.ToInt32(unitFolder) - 1][1] == "true" && attackAllowed == false)
		{
			attackAllowed = true;
			LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 1f, 0.5f);
			gameObject.GetComponent<CanvasGroup>().interactable = true;

		}
		else if(gameManager.attackList[Convert.ToInt32(unitFolder) - 1][0] == unitInformation.attackName && gameManager.attackList[Convert.ToInt32(unitFolder) - 1][1] == "false" && attackAllowed == true)
		{
			attackAllowed = false;
			LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0.5f, 0.5f);
			gameObject.GetComponent<CanvasGroup>().interactable = false;
		}

		if (Input.GetMouseButtonDown(0) && attackAllowed == true){

        	if(active && isSelected){

			 	RaycastHit[] hits;
				hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 5.0f);
	        	
	    		for (int x = 0; x < hits.Length; x++){
					
					RaycastHit hitPlane = hits[x];

					//print(hitPlane.transform.gameObject.name);
					
					if(hitPlane.transform.gameObject.tag == "target" && unitInformation.targetSystem == "TankAttack" && selectedPlane != hitPlane.transform.gameObject)
					{
						ResetHighlights();

						//print("we In");

						if(hitPlane.transform.name == "1"){
							//gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(hitPlane.transform.parent.name).Find(hitPlane.transform.name);
							HighlightPlane(hitPlane.transform.parent.name, "1");
							HighlightPlane(hitPlane.transform.parent.name, "2");
							HighlightPlane(hitPlane.transform.parent.name, "4");
							HighlightPlane(hitPlane.transform.parent.name, "5");

						}
						else if(hitPlane.transform.name == "3"){
							HighlightPlane(hitPlane.transform.parent.name, "2");
							HighlightPlane(hitPlane.transform.parent.name, "3");
							HighlightPlane(hitPlane.transform.parent.name, "5");
							HighlightPlane(hitPlane.transform.parent.name, "6");
						}
						else if(hitPlane.transform.name == "7"){
							HighlightPlane(hitPlane.transform.parent.name, "4");
							HighlightPlane(hitPlane.transform.parent.name, "5");
							HighlightPlane(hitPlane.transform.parent.name, "7");
							HighlightPlane(hitPlane.transform.parent.name, "8");
						}
						else if(hitPlane.transform.name == "9"){
							HighlightPlane(hitPlane.transform.parent.name, "5");
							HighlightPlane(hitPlane.transform.parent.name, "6");
							HighlightPlane(hitPlane.transform.parent.name, "8");
							HighlightPlane(hitPlane.transform.parent.name, "9");
						}
						
						selectedPlane = hitPlane.transform.gameObject;

						attackArray = new string[]{"attack", unitInformation.attackName, hitPlane.transform.parent.name, hitPlane.transform.name};

						attackHandler.attackArray = attackArray;
						attackHandler.attackCost = unitInformation.attackCost;
						//print(unitInformation.targetSystem + " "+ unitInformation.attackName + ": " + hitPlane.transform.parent.name +", "+ hitPlane.transform.name);
						//hitPlane.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;

			        	ConfirmButton.interactable = true;
					}

					else if(hitPlane.transform.gameObject.tag == "target" && unitInformation.targetSystem == "SingleAttack" && selectedPlane != hitPlane.transform.gameObject){
						ResetHighlights();
						HighlightPlane(hitPlane.transform.parent.name, hitPlane.transform.name);

						selectedPlane = hitPlane.transform.gameObject;
						attackArray = new string[]{"attack", unitInformation.attackName, hitPlane.transform.parent.name, hitPlane.transform.name};

						attackHandler.attackArray = attackArray;
						attackHandler.attackCost = unitInformation.attackCost;

						ConfirmButton.interactable = true;
					}
				}
	        	
	        }
        }

        if(!isSelected && oldTargets.Count > 0 ){
        	ResetHighlights();
        }
    }

    void HighlightPlane(string parentName, string i){
    	GameObject plane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(parentName).Find(i).gameObject;
    	plane.GetComponent<MeshRenderer>().material.color = Color.yellow;
    	oldTargets.Add(plane);
    }

    public void ResetHighlights(){
    	foreach(GameObject plane in oldTargets){
    		var tempColor = plane.GetComponent<MeshRenderer>().material.color;
    		tempColor.a = 0f;
    		plane.GetComponent<MeshRenderer>().material.color = tempColor;
    		// oldTargets.Remove(plane);
    	}

    	oldTargets.Clear();
    	selectedPlane = null;

    	//isSelected = false;

    	attackHandler.attackArray = null;
		attackHandler.attackCost = -1;

		ConfirmButton.interactable = false;

    }

 //    public void OnMouseDown()
	// {
	// 	if(active && isSelected){

	// 		print("oifoawomd");

	// 	 	RaycastHit[] hits;
	// 		hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 10.0f);
        	
 //    		for (int x = 0; x < hits.Length; x++){
				
	// 			RaycastHit hitPlane = hits[x];

	// 			print(hitPlane.transform.gameObject.name);
				
	// 			if(hitPlane.transform.gameObject.tag == "target")
	// 			{
	// 				print(unitInformation.targetSystem + ": " + hitPlane.transform.parent.name +", "+ hitPlane.transform.name);
	// 			}
	// 		}
        	
 //        }
	// }

    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        //Debug.Log(name + "Game Object Click in Progress");

        if(active && !isSelected && attackAllowed){

        	ResetHighlights();

			for(int i = 0; i < transform.parent.childCount; i++){
				// selected = false;
				transform.parent.GetChild(i).GetComponent<ChooseAttackHandler>().isSelected = false;
				transform.parent.GetChild(i).GetComponent<TweenController>().CancelPulseHighlight();
			}

			tweenController.Highlight();
			isSelected = true;

			//print("Selected Attack: " + unitInformation.attackName);

			for(int j = 0; j < gameManager.enemyCubePosition.transform.GetChild(1).childCount-1; j++){
				gameManager.enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(false);
				// targetsystems.SetActive(false);
			}

			gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);
			// targetsystem.SetActive(true);

		}

    }

}

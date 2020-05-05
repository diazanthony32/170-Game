using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInformation : MonoBehaviour
{
    /*

	1.) This Script is added to a prefab of a game-unit so they can be "disabled"/
		become hidden once touching and remaining inside of the Hidden_Bounds Boxes.

		Once out the Bounds of the Boxes, it gets "enabled"/becomes visible.


	2.) This Script is added to a prefab of a game-unit so they can check if the player is allowed to attack
	    once touching and remaining inside of the Hidden_Bounds Boxes.

		Once out the Bounds of the Boxes, attack gets "disabled" unless there is another unit inside
		the "AttackCheck box", that will override it.


	3.) This script hold all the important information on the Unit



	MAKE SURE THE UNIT HAS A COLLIDER

	*/

	// contains all the important Unit Info ----
	public string folder;
	[Space(10)]
	public string unitName;
	public int unitSpawnCost;
	public float unitHealth;
	public float unitCurrentHealth;
	[Space(10)]
	public string attackName;
	public int attackCost;
	public int attackDmg;
	[Space(10)]
	public Sprite unitImage;
	[Space(10)]
	public AudioClip unitSpawnNoise = null;
	public AudioClip unitIdleNoise = null;
	public AudioClip unitHitNoise = null;
	public AudioClip unitDeathNoise = null;
	public AudioClip unitAttackNoise = null;
	public AudioClip towerShieldSfx = null;

	[Space(10)]
	public string targetSystem;
	[Space(10)]
	public GameObject AttackParticle = null;
	public bool shakeScreen = false;

	//this gets the render of the prefab its attached to
	Renderer unitRenderer;

	AudioSource unitAudioSource;
	Animator unitAnimator;
	Image unitHealthBar;

	//-------

	GameManager gameManager;
	InfoSender infoSender;

	// contains the unit health ratio
	float oldheathRatio;

	// sends info to other player
	public bool isTower = false;
	bool isVulnerable = true;

	GameObject parentPlane;

	bool pointerDown;
	float pointerDownTimer;
	float requiredHoldTime = 0.5f;

	bool executed = false;
	LayerMask mask;

	// runs once the unit is created inside of the DragNDropUnits Script
	void Start(){

		mask = LayerMask.GetMask("unit");

		gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
		infoSender = GameObject.FindGameObjectWithTag("infoSender").GetComponent<InfoSender>();

		unitAudioSource = GetComponent<AudioSource>();
		// unitAnimator = GetComponent<Animator>();
		//unitAudioSource.volume = 0.1f;

		unitRenderer = transform.Find("UnitScaler").GetComponentInChildren<Renderer>();
		// oc = this.transform.root.GetComponent<objectClicker>();
		// parentPlane = this.transform.parent.gameObject;

		unitRenderer = GetComponentInChildren<MeshRenderer>();

		unitHealthBar = GetComponentInChildren<Image>();

		if(isTower){
			isVulnerable = false;
		}

		// oc.unitPoints -= unitCost;

		unitCurrentHealth = unitHealth;
		// gameObject.GetComponentInChildren<Renderer>().material.SetFloat("Vector1_A2DEF370", 0.0f);
	}

	void Update(){
		if(pointerDown){
			pointerDownTimer += Time.deltaTime;

			RaycastHit[] hits;
			hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 5.0f, mask);

			for (int i = 0; i < hits.Length; i++){
				RaycastHit hitPlane = hits[i];
				//print(hits[i].transform.gameObject.name);
				
				if(hitPlane.transform.gameObject == gameObject)
				{
					if(pointerDownTimer > requiredHoldTime && !executed){
						//print("Removing Unit");
						executed = true;

						if(gameManager.GetState() == gameManager.SETUP){
							Destroy(gameObject);
							infoSender.RemoveUnitPlacement(new string[] {gameObject.transform.parent.parent.name, gameObject.transform.parent.name});
							//gameManager.remainingUnitPoints += unitSpawnCost;
							gameManager.AddUnitPoints(unitSpawnCost);

							if (isTower){
								gameManager.AddTowerCount(-1);
							}
							else if(!isTower){
								gameManager.AddUnitCount(-1);
							}
						}

						//print("can attack");
						for (int j = 0; j < gameManager.attackList.Count; j++)
						{

							if (gameManager.attackList[j][0] == attackName)
							{
								gameManager.attackList[j][1] = "false";
								//print(gameManager.attackList[j][0] + " : " + gameManager.attackList[j][1]);

							}
						}

					}
					break;
				}
				else{
					//reset timer if not hovering over the unit
					pointerDownTimer = 0;
				}
			}
			if (hits.Length == 0) {
				//reset timer if not hovering over the unit
				pointerDownTimer = 0;
			}
		}

		unitHealthBar.fillAmount = unitCurrentHealth / unitHealth;
	}

	public void TakeDamage(int damageAmount){
		
		if(isTower){
			CheckIfVulnerable();
		}
		//CheckIfVulnerable();

		if(isVulnerable){
			
			unitCurrentHealth -= damageAmount;
			//audioSource.PlayOneShot(hitEnemySfx);

			// audioSource.PlayOneShot(unitBehavior.unitHitNoise);
			if(unitCurrentHealth > 0){

				if(unitHitNoise != null){
					unitAudioSource.PlayOneShot(unitHitNoise);
					//unitAnimator.SetTrigger("Hit");
					
					if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
					{
						Handheld.Vibrate();
					}
				}
				
			}
			else if(unitCurrentHealth <= 0){

				if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
				{
					Handheld.Vibrate();
				}

				StartCoroutine(Die());
			}

		}
		else if(!isVulnerable && isTower){

			if(towerShieldSfx != null){
				unitAudioSource.PlayOneShot(towerShieldSfx);
			}
		}

	}

	void CheckIfVulnerable(){
		bool safe = false;

		for(int i = 0; i < transform.parent.parent.childCount; i++) {

			var plane = transform.parent.parent.GetChild(i);

			if(plane.gameObject.transform.childCount > 0){
				for(int x = 0 ; x < plane.gameObject.transform.childCount ; x++ ){

					if(!(plane.transform.GetChild(x).gameObject.GetComponent<UnitInformation>().isTower)){

						isVulnerable = false;
						safe = true;
					}

				}

			}
		}

		if(!safe){
			isVulnerable = true;
		}
	}

	// runs when the player clicks an attack to do
	public void OnMouseDown()
	{
		pointerDown = true;

	}

	// runs when the player clicks an attack to do
	public void OnMouseUp()
	{
		pointerDown = false;
		pointerDownTimer = 0;
		
	}

	void OnTriggerEnter(Collider trigger)
	{
		if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition") {
			
			if (trigger.CompareTag("attackChecker"))
			{
				//print("can attack");
				for (int i = 0; i < gameManager.attackList.Count; i++){

					if (gameManager.attackList[i][0] == attackName)
					{
						gameManager.attackList[i][1] = "true";
						//print(gameManager.attackList[i][0] + " : " + gameManager.attackList[i][1]);

					}
				}
			}
		}

		if (trigger.CompareTag("hideUnit"))
		{
			unitRenderer.enabled = false;
		}

	}

	void OnTriggerStay(Collider trigger)
	{
		if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
		{

			if (trigger.CompareTag("attackChecker"))
			{
				//print("can attack");
				for (int i = 0; i < gameManager.attackList.Count; i++) {

					if (gameManager.attackList[i][0] == attackName)
					{
						gameManager.attackList[i][1] = "true";
						//print(gameManager.attackList[i][0] + " : " + gameManager.attackList[i][1]);

					}
				}
			}
		}

		if (trigger.CompareTag("hideUnit"))
		{
			unitRenderer.enabled = false;
		}
	}

	void OnTriggerExit(Collider trigger)
	{
		if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
		{

			if (trigger.CompareTag("attackChecker"))
			{
				//print("can attack");
				for (int i = 0; i < gameManager.attackList.Count; i++) {

					if (gameManager.attackList[i][0] == attackName)
					{
						gameManager.attackList[i][1] = "false";
						//print(gameManager.attackList[i][0] + " : " + gameManager.attackList[i][1]);

					}
				}
			}
		}
		
		if (trigger.CompareTag("hideUnit"))
		{
			unitRenderer.enabled = true;
		}
	}

	IEnumerator Die() {

		unitAudioSource.PlayOneShot(unitDeathNoise);

		//unitBehavior.unitAudioSource.PlayOneShot(unitBehavior.unitDeathNoise);
		//unitBehavior.unitAnimator.SetTrigger("Death");
		//print("Im dead :(");

		yield return new WaitForSeconds(1.0f);
		//print(transform.parent.parent.parent.parent.gameObject.tag);

		if(transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition"){
    		if(isTower){
				gameManager.AddTowerCount(-1);
			}
			else{
				gameManager.AddUnitCount(-1);
			}
		}
		else if(transform.parent.parent.parent.parent.gameObject.tag == "EnemyCubePosition"){
    		if(isTower){
				gameManager.AddEnemyTowerCount(-1);
			}
			else{
				gameManager.AddEnemyUnitCount(-1);
			}
		}

		Destroy(gameObject);

		//print("can attack");
		for (int i = 0; i < gameManager.attackList.Count; i++)
		{

			if (gameManager.attackList[i][0] == attackName)
			{
				gameManager.attackList[i][1] = "false";
				//print(gameManager.attackList[i][0] + " : " + gameManager.attackList[i][1]);

			}
		}

	}
}

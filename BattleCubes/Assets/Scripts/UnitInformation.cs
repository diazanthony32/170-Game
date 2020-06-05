using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
	public Sprite attackImage;
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
    public GameObject SpawnParticle = null;
    public GameObject shieldParticle = null;
    public GameObject shieldBustParticle = null;
    public GameObject deathParticle = null;

    //this gets the render of the prefab its attached to
    public Renderer unitRenderer;

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
	public bool isVulnerable = true;

	GameObject parentPlane;

	bool pointerDown;
	float pointerDownTimer;
	float requiredHoldTime = 0.25f;

	bool executed = false;
	LayerMask mask;

	// runs once the unit is created inside of the DragNDropUnits Script
	void Start(){

		mask = LayerMask.GetMask("unit");

		if (SceneManager.GetActiveScene().buildIndex != 0)
		{
			gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
		}

		if (SceneManager.GetActiveScene().buildIndex != 0)
		{
			infoSender = GameObject.FindGameObjectWithTag("infoSender").GetComponent<InfoSender>();
		}

		unitAudioSource = GetComponent<AudioSource>();
		unitAnimator = transform.Find("UnitScaler").GetComponentInChildren<Animator>();
		//unitAudioSource.volume = 0.1f;

		//unitRenderer = transform.Find("UnitScaler").GetComponentInChildren<SkinnedMeshRenderer>();
		// oc = this.transform.root.GetComponent<objectClicker>();
		// parentPlane = this.transform.parent.gameObject;

		//unitRenderer = GetComponentInChildren<MeshRenderer>();

		unitHealthBar = GetComponentInChildren<Image>();

		if (isTower)
		{
			isVulnerable = false;
			//unitRenderer = transform.Find("UnitScaler").GetComponentInChildren<MeshRenderer>();
		}
		//else {
			//unitRenderer = transform.Find("UnitScaler").GetComponentInChildren<SkinnedMeshRenderer>();
		//}

		if (unitAudioSource && unitAnimator && transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition" && SceneManager.GetActiveScene().buildIndex != 0)
		{
			unitAudioSource.PlayOneShot(unitSpawnNoise);
			unitAnimator.SetTrigger("spawn");

		}

		// oc.unitPoints -= unitCost;

		unitCurrentHealth = unitHealth;
		// gameObject.GetComponentInChildren<Renderer>().material.SetFloat("Vector1_A2DEF370", 0.0f);
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));
		}

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
					if(pointerDownTimer > requiredHoldTime && !executed && SceneManager.GetActiveScene().buildIndex != 0)
					{
						//print("Removing Unit");
						executed = true;

						if(gameManager.GetState() == gameManager.SETUP && !gameManager.readies[0]){
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

		if (SceneManager.GetActiveScene().buildIndex != 0) 
		{
			unitHealthBar.fillAmount = unitCurrentHealth / unitHealth;
		}

	}

	public void TakeDamage(int damageAmount, float impactDelay){
		
		if(isTower && !isVulnerable){
			CheckIfVulnerable();
		}
		//CheckIfVulnerable();

		if(isVulnerable){
            StartCoroutine(DelayImpact(damageAmount, impactDelay));
		}
		else if(!isVulnerable && isTower){

            if (unitAudioSource)
            {
				if (towerShieldSfx != null)
				{
					unitAudioSource.PlayOneShot(towerShieldSfx);
				}
			}
		}

	}

	public bool CheckIfVulnerable(){
		bool safe = false;

		for(int i = 0; i < transform.parent.parent.childCount; i++) {

			var plane = transform.parent.parent.GetChild(i);

			if(plane.gameObject.transform.childCount > 0){
				for(int x = 0 ; x < plane.gameObject.transform.childCount ; x++ ){

					if(plane.CompareTag("unitSquare") && plane.transform.GetChild(x).gameObject.GetComponent<UnitInformation>()){

						if (!(plane.transform.GetChild(x).gameObject.GetComponent<UnitInformation>().isTower)) {

							isVulnerable = false;
							safe = true;
						}
					}

				}

			}
		}

		if(!safe){
			isVulnerable = true;
		}
        return isVulnerable;
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
		if (SceneManager.GetActiveScene().buildIndex != 0)
		{
			if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
			{

				if (trigger.CompareTag("attackChecker"))
				{
					//print("can attack");
					for (int i = 0; i < gameManager.attackList.Count; i++)
					{

						if (gameManager.attackList[i][0] == attackName)
						{
							gameManager.attackList[i][1] = "true";
							//print(gameManager.attackList[i][0] + " : " + gameManager.attackList[i][1]);

						}
					}
				}
			}
		}

		if (trigger.CompareTag("hideUnit"))
		{
			unitRenderer.enabled = false;
		}

	}

	public void DoAttackAnimation() {

		if (unitAudioSource)
		{
			unitAudioSource.PlayOneShot(unitAttackNoise);
		}

		if (unitAnimator)
		{
			unitAnimator.SetTrigger("attack");
		}
	}

	void OnTriggerStay(Collider trigger)
	{
		if (SceneManager.GetActiveScene().buildIndex != 0 && transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
		{

			if (SceneManager.GetActiveScene().buildIndex != 0) {
				if (trigger.CompareTag("attackChecker"))
				{
					//print("can attack");
					for (int i = 0; i < gameManager.attackList.Count; i++)
					{

						if (gameManager.attackList[i][0] == attackName)
						{
							gameManager.attackList[i][1] = "true";
							//print(gameManager.attackList[i][0] + " : " + gameManager.attackList[i][1]);

						}
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
			if (SceneManager.GetActiveScene().buildIndex != 0)
			{
				if (trigger.CompareTag("attackChecker"))
				{
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
		}
		
		if (trigger.CompareTag("hideUnit"))
		{
			unitRenderer.enabled = true;
		}
	}

	IEnumerator Die() {

		if (unitAudioSource)
		{
			unitAudioSource.PlayOneShot(unitDeathNoise);
		}

		if (unitAnimator)
		{
			unitAnimator.SetTrigger("death");
		}

		//unitBehavior.unitAudioSource.PlayOneShot(unitBehavior.unitDeathNoise);
		//unitBehavior.unitAnimator.SetTrigger("Death");
		//print("Im dead :(");

		yield return new WaitForSeconds(1.5f);
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
        gameManager.UpdateShields();
    }
    IEnumerator DelayImpact(int damageAmount, float impactDelay) {
        yield return new WaitForSeconds(impactDelay);

        unitCurrentHealth -= damageAmount;
        gameManager.CreateFloatingText(("-" + damageAmount.ToString()), transform.parent, "attack");

        transform.parent.parent.parent.GetComponent<CubeInformation>().StartImpact();
        //audioSource.PlayOneShot(hitEnemySfx);

        Handheld.Vibrate();

        // audioSource.PlayOneShot(unitBehavior.unitHitNoise);
        if (unitCurrentHealth > 0) {

            if (unitHitNoise != null) {

                if (unitAudioSource) {
                    //unitAudioSource.PlayOneShot(unitDeathNoise);
                    unitAudioSource.PlayOneShot(unitHitNoise);

                }

                //unitAudioSource.PlayOneShot(unitHitNoise);
                //unitAnimator.SetTrigger("Hit");

                //if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
                //{
                //Handheld.Vibrate();
                //}
            }

            if (unitAnimator) {
                unitAnimator.SetTrigger("hit");
            }

        }
        else if (unitCurrentHealth <= 0) {

            //if (transform.parent.parent.parent.parent.gameObject.tag == "PlayerCubePosition")
            //{
            //Handheld.Vibrate();
            //}

            StartCoroutine(Die());
        }
    }
    
    private void OnDestroy() {
        if (!isTower) {
            if (gameManager) {
                gameManager.UpdateShields();
            }
        }
    }

    public void ReColorUnit(string player , string theme, string color) 
	{
		

		for (int i = 0 ; i < unitRenderer.materials.Length ; i++) {
			if (theme == "Demon") 
			{
				if (unitName == "TankBoi")
				{
					if (i == 0)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Back"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Back").shader;

                        //print("set body color");
                    }
                    else if (i == 1)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Knee"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Knee").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/BoneParts");

                        //print("set bone colors");
                    }
                    else if (i == 2)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Shoulder"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Shoulder").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Horns");

                        //print("set bone colors");
                    }
                    else if (i == 3)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hip"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hip").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

                        //print("set bone colors");
                    }
					else if (i == 4)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Arm"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Arm").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 5)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Chest"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Chest").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 6)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Spine"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Spine").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 7)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Body"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Body").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 8)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 9 || i == 12)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hole"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hole").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 10)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Horn"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Horn").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else if (i == 11)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Claw"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Claw").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

						//print("set bone colors");
					}
					else
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/ERROR"));
						//print("set bone colors");
					}
				}
				else if (unitName == "SniperBoi")
				{
					if (i == 0 || i == 2)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hole"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hole").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");

                        //print("set body color");
                    }
                    else if (i == 1)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head").shader;
                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/BoneParts");

                        //print("set body color");
                    }
                    else if (i == 4)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Body"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Body").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Body");

                        //print("set body color");
                    }
                    else
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/ERROR"));
						//print("set bone colors");
					}
				}
				else if (unitName == "SingleBoi")
				{
					if (i == 0 || i == 8)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/BoneParts");
                        //print("set body color");
                    }
                    else if (i == 1)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Body"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Body").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Body");

                        //print("set body color");
                    }
                    else if (i == 2 || i == 3)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Horns");

                        //print("set body color");
                    }
                    else if (i == 4)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Horn"));
                        unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Horn").shader;

                        //unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");
                        //print("set body color");
                    }
					else if (i == 5 || i == 7)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hole"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Hole").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");
						//print("set body color");
					}
					else if (i == 6 )
					{
						//eyebrows
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Shoulder"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Shoulder").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");
						//print("set body color");
					}
					else
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/ERROR"));
						//print("set bone colors");
					}
				}
				else if (unitName == "TowerBoi")
				{
					if (i == 3)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Head").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/BoneParts");
						//print("set body color");
					}
					else if (i == 1)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Chain"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Chain").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Body");

						//print("set body color");
					}
					else if (i == 0)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/SHIELD"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/SHIELD").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Body");

						//print("set body color");
					}
					else if (i == 4)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/MainBody"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/MainBody").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Horns");

						//print("set body color");
					}
					else if (i == 2)
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Gem"));
						unitRenderer.materials[i].shader = Resources.Load<Material>("Themes/Demon/Colors/" + color + "/Gem").shader;

						//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/Holes");
						//print("set body color");
					}
					else
					{
						unitRenderer.materials[i].CopyPropertiesFromMaterial(Resources.Load<Material>("Themes/Demon/Colors/" + color + "/ERROR"));
						//print("set bone colors");
					}
				}
			}
			//unitRenderer.materials[i] = Resources.Load<Material>("Themes/Demons/Colors/" + PlayerPrefs.GetString("CubeColor") + "Body");
		}

	}
}

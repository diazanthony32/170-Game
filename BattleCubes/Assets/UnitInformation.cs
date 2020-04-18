using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public string unitName;
	public int unitSpawnCost;
	public int unitHealth;
	int unitCurrentHealth;

	public string attackName;
	public int attackCost;
	public int attackDmg;

	public Sprite unitImage;

	public AudioClip unitSpawnNoise;
	public AudioClip unitIdleNoise;
	public AudioClip unitHitNoise;
	public AudioClip unitDeathNoise;
	public AudioClip unitAttackNoise;

	//this gets the render of the prefab its attached to
	Renderer unitRenderer;

	AudioSource unitAudioSource;
	Animator unitAnimator;

	//-------

	// contains the unit health ratio
	float oldheathRatio;

	// sends info to other player
	[SerializeField] bool isTower = false;
	bool isVulnerable = true;

	GameObject parentPlane;

	// runs once the unit is created inside of the DragNDropUnits Script
	void Start(){

		// unitAudioSource = GetComponent<AudioSource>();
		// unitAnimator = GetComponent<Animator>();
		// //unitAudioSource.volume = 0.1f;

		// unitRenderer = transform.Find("UnitScaler").GetComponentInChildren<Renderer>();
		// oc = this.transform.root.GetComponent<objectClicker>();
		// parentPlane = this.transform.parent.gameObject;

		// if(!tower){
		// 	oc.currentNumUnits += 1;
		// }
		// else{
		// 	oc.towerCount += 1;
		// }

		// oc.unitPoints -= unitCost;

		// unitCurrentHealth = unitHealth;
		// gameObject.GetComponentInChildren<Renderer>().material.SetFloat("Vector1_A2DEF370", 0.0f);
	}

	public void TakeDamage(int damageAmount){

		// if(unitBehavior.isVulnerable){
		// 	unitBehavior.unitCurrentHealth -= attackDamage;
		// 	audioSource.PlayOneShot(hitEnemySfx);

		// 	// audioSource.PlayOneShot(unitBehavior.unitHitNoise);
		// 	if(unitBehavior.unitCurrentHealth >= 1){
		// 		unitBehavior.unitAudioSource.PlayOneShot(unitBehavior.unitHitNoise);
		// 		unitBehavior.unitAnimator.SetTrigger("Hit");
		// 	}

		// 	//Handheld.Vibrate();

		// 	GameObject character = Instantiate(oc.hitParticle);
		// 	var characterTransformer = character.GetComponent<Transform>();

		// 	characterTransformer.transform.position = hitUnitPlaneTransformer.transform.position;
		// 	characterTransformer.transform.rotation = hitUnitPlaneTransformer.transform.rotation;
		// }
		// else if(!unitBehavior.isVulnerable && unitBehavior.tower){
		// 	audioSource.PlayOneShot(towerShieldSfx);
		// }

		// if(unitCurrentHealth < 1)
		// {
		// 	StartCoroutine(Die());
		// 	// unitBehavior.unitAudioSource.PlayOneShot(unitBehavior.unitDeathNoise);
		// 	// Destroy(unitGameObject);
		// }

	}

	IEnumerator Die() {


  //   	unitBehavior.unitAudioSource.PlayOneShot(unitBehavior.unitDeathNoise);
  //   	unitBehavior.unitAnimator.SetTrigger("Death");

    	yield return new WaitForSeconds(1.0f);

		// Destroy(unitGameObject);

    }
}

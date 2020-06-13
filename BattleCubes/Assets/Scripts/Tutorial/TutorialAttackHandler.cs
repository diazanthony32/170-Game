using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TutorialAttackHandler : MonoBehaviour
{
    [SerializeField] TutorialManagement gameManager;
    public string[] attackArray = null;
    public int attackCost = -1;
    [Space(10)]
    [SerializeField] GameObject hideButton;
    [SerializeField] GameObject showButton;

    bool backShown = false;

    List<GameObject> hiddenFrontFaces = new List<GameObject>();

    List<GameObject> backTargets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        SetupBackTargetting();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StoreAttack() {
        if (attackArray != null && attackCost != -1) {
            gameManager.AddAction(attackArray, attackCost);
        }
        ResetChooseAttackHandlers();
    }

    public void ResetChooseAttackHandlers() {
        //if (PhotonNetwork.LocalPlayer.IsMasterClient) {
        for (int i = 0; i < transform.childCount; i++) {
            TutorialChooseAttackHandler chooseAttackHandler = transform.GetChild(i).GetChild(0).GetComponent<TutorialChooseAttackHandler>();
            chooseAttackHandler.ResetHighlights(true, true);
            chooseAttackHandler.isSelected = false;
            chooseAttackHandler.GetComponent<TweenController>().CancelPulseHighlight();

            //chooseAttackHandler.isSelected = false;
        }
        //}
    }

    public IEnumerator DoAttack(string player, string[] array) {

        //TurnOnTargetting();
        bool attackAllowed = false;

        attackAllowed = CheckForAttackUnit(player, array[1]);

        // if(player == "Host"){
        // 	UnitInformation unitInfo = GetAttackUnit(player, array[1]);
        // 	GameObject attackedPlane = GetTargettedPlane(player, array[2], array[3]);

        // 	if(attackedPlane.transform.childCount > 0){
        // 		print("You hit a Unit: " + attackedPlane.transform.GetChild(0).name);
        // 	}
        // 	else{
        // 		print("You missed...");
        // 	}
        // }
        // else if(player == "Client"){
        // 	UnitInformation unitInfo = GetAttackUnit(player, array[1]);
        // 	GameObject attackedPlane = GetTargettedPlane(player, array[2], array[3]);

        // 	if(attackedPlane.transform.childCount > 0){
        // 		print("You hit a Unit: " + attackedPlane.transform.GetChild(0).name);
        // 	}
        // 	else{
        // 		print("You missed...");
        // 	}
        // } 

        if (attackAllowed)
        {

            yield return new WaitForSeconds(0.75f);

            TutorialUnitInformation unitInfo = GetAttackUnit(player, array[1]);

            if (unitInfo.targetSystem == "SingleAttack")
            {
                GameObject attackedPlane;

                if (array.Length == 5)
                {
                    print("single Attack Back!");
                    attackedPlane = GetTargettedPlane(player, array[2], array[3], true);
                }
                else { 
                    attackedPlane = GetTargettedPlane(player, array[2], array[3], false);
                }

                if (unitInfo.AttackParticle != null)
                {
                    GameObject particle = Instantiate(unitInfo.AttackParticle);
                    particle.transform.position = attackedPlane.transform.position;
                    particle.transform.rotation = attackedPlane.transform.rotation;
                    particle.transform.SetParent(attackedPlane.transform);
                    Destroy(particle, 5f);
                }

                if (attackedPlane.transform.childCount > 0 && attackedPlane.transform.GetChild(0).GetComponent<TutorialUnitInformation>())
                {
                    print("You hit a Unit: " + attackedPlane.transform.GetChild(0).name);
                    TutorialUnitInformation hitUnitInfo = attackedPlane.transform.GetChild(0).GetComponent<TutorialUnitInformation>();
                    
                    //yield return new WaitForSeconds(0.5f);

                    hitUnitInfo.TakeDamage(unitInfo.attackDmg);
                }
                else
                {
                    print("You missed...");
                }
            }
            else if (unitInfo.targetSystem == "TankAttack")
            {
                GameObject[] attackedPlanes;
                if (array.Length == 5)
                {
                    print("Tank Attack Back!");
                    attackedPlanes = GetTankTargettedPlaneArray(player, array[2], array[3], true);
                }
                else {
                    print("Regular attack :(");
                    attackedPlanes = GetTankTargettedPlaneArray(player, array[2], array[3], false);
                }

                for (int i = 0; i < 4; i++)
                {
                    //print("Back particle?: " + attackedPlanes[i].name);
                    if (unitInfo.AttackParticle != null)
                    {
                        GameObject particle = Instantiate(unitInfo.AttackParticle);
                        particle.transform.position = attackedPlanes[i].transform.position;
                        particle.transform.rotation = attackedPlanes[i].transform.rotation;
                        particle.transform.SetParent(attackedPlanes[i].transform);
                        Destroy(particle, 5f);
                    }

                    if (attackedPlanes[i].transform.childCount > 0 && attackedPlanes[i].transform.GetChild(0).GetComponent<TutorialUnitInformation>())
                    {
                        print("You hit a Unit: " + attackedPlanes[i].transform.GetChild(0).name);
                        TutorialUnitInformation hitUnitInfo = attackedPlanes[i].transform.GetChild(0).GetComponent<TutorialUnitInformation>();

                        //yield return new WaitForSeconds(0.5f);

                        hitUnitInfo.TakeDamage(unitInfo.attackDmg);
                    }
                    else
                    {
                        print("You missed...");
                    }
                }
            }
        }

        //TurnOffTargetting();
        // attackArray = new string[]{"attack", unitInformation.attackName, hitPlane.transform.parent.name, hitPlane.transform.name};
        //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

    }

    public TutorialUnitInformation GetAttackUnit(string player, string attackName) {

        if (player == "Host") {

            // UnitInformation unitInfo;
            for (int i = 0; i <= 3; i++) {

                TutorialUnitInformation unitInfo;

                if (i == 0) {
                    unitInfo = Resources.Load<GameObject>("Themes/Tutorial/Units/Tower/Prefab").GetComponent<TutorialUnitInformation>();
                    //("Themes/" + gameManager.cubeInfo[0] + "/Units/Tower/Prefab")
                }
                else {
                    unitInfo = Resources.Load<GameObject>("Themes/Tutorial/Units/" + i + "/Prefab").GetComponent<TutorialUnitInformation>();
                    //("Themes/" + gameManager.cubeInfo[0] + "/Units/" + i + "/Prefab")
                }

                if (unitInfo != null && unitInfo.attackName == attackName) {
                    return unitInfo;
                }
            }
        }

        if (player == "Client") {
            //UnitInformation unitInfo;
            //print("Enemy Cube Info: " + gameManager.enemyCubeInfo[0] + " " + gameManager.enemyCubeInfo[1]);

            for (int i = 0; i <= 3; i++) {

                TutorialUnitInformation unitInfo = null;

                if (i == 0) {
                    unitInfo = Resources.Load<GameObject>("Themes/Tutorial/Units/Tower/Prefab").GetComponent<TutorialUnitInformation>();
                    //("Themes/" + gameManager.enemyCubeInfo[0] + "/Units/Tower/Prefab")
                }
                else {
                    unitInfo = Resources.Load<GameObject>("Themes/Tutorial/Units/" + i + "/Prefab").GetComponent<TutorialUnitInformation>();
                    //("Themes/" + gameManager.enemyCubeInfo[0] + "/Units/" + i + "/Prefab")
                }

                if (unitInfo != null && unitInfo.attackName == attackName) {
                    return unitInfo;
                }
            }
        }
        return null;
    }

    GameObject GetTargettedPlane(string player, string planeParent, string plane, bool backAttack) {

        // 	void HighlightPlane(string parentName, string i){
        // 	GameObject plane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(parentName).Find(i).gameObject;
        // 	plane.GetComponent<MeshRenderer>().material.color = Color.yellow;
        // 	oldTargets.Add(plane);
        // }
        GameObject targetPlane = null;
        if (player == "Host") {
            //if (backAttack)
            //{
                //print("back attack!!!!!!");
                //targetPlane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find("GameObject").Find(plane).gameObject;
                //print(targetPlane.name);
            //}
            //else 
            //{
                targetPlane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find(plane).gameObject;
            //}
        }
        else if (player == "Client") {
            //if (backAttack)
            //{
            //    targetPlane = gameManager.playerCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find("GameObject").Find(plane).gameObject;
            //}
            //else 
            //{
                targetPlane = gameManager.playerCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find(plane).gameObject;
            //}
            //targetPlane = gameManager.playerCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find(plane).gameObject;
        }

        RaycastHit[] hits;
        hits = Physics.RaycastAll(targetPlane.transform.position, -targetPlane.transform.up, 0.25f);

        for (int i = 0; i < hits.Length; i++)
        {
            //stored info from the enemy's unitPlane underneath
            RaycastHit hit = hits[i];
            var hitPlane = hit.transform.gameObject;

            //print(hitPlane.name);
            //this is where we filter out the weird self hits
            if (hitPlane.tag == "unitSquare")
            {
                print("found unit aquare to attack");
                return hitPlane.gameObject;
            }

        }
        print("nothin");
        return null;
    }

    GameObject[] GetTankTargettedPlaneArray(string player, string planeParent, string plane, bool backAttack) {

        // 	void HighlightPlane(string parentName, string i){
        // 	GameObject plane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(parentName).Find(i).gameObject;
        // 	plane.GetComponent<MeshRenderer>().material.color = Color.yellow;
        // 	oldTargets.Add(plane);
        // }
        GameObject targetPlane1 = null;
        GameObject targetPlane2 = null;
        GameObject targetPlane3 = null;
        GameObject targetPlane4 = null;

        if (plane == "1") {
            targetPlane1 = GetTargettedPlane(player, planeParent, "1", backAttack);
            targetPlane2 = GetTargettedPlane(player, planeParent, "2", backAttack);
            targetPlane3 = GetTargettedPlane(player, planeParent, "4", backAttack);
            targetPlane4 = GetTargettedPlane(player, planeParent, "5", backAttack);
        }
        else if (plane == "3") {
            targetPlane1 = GetTargettedPlane(player, planeParent, "2", backAttack);
            targetPlane2 = GetTargettedPlane(player, planeParent, "3", backAttack);
            targetPlane3 = GetTargettedPlane(player, planeParent, "5", backAttack);
            targetPlane4 = GetTargettedPlane(player, planeParent, "6", backAttack);
        }
        else if (plane == "7") {
            targetPlane1 = GetTargettedPlane(player, planeParent, "4", backAttack);
            targetPlane2 = GetTargettedPlane(player, planeParent, "5", backAttack);
            targetPlane3 = GetTargettedPlane(player, planeParent, "7", backAttack);
            targetPlane4 = GetTargettedPlane(player, planeParent, "8", backAttack);
        }
        else if (plane == "9") {
            targetPlane1 = GetTargettedPlane(player, planeParent, "5", backAttack);
            targetPlane2 = GetTargettedPlane(player, planeParent, "6", backAttack);
            targetPlane3 = GetTargettedPlane(player, planeParent, "8", backAttack);
            targetPlane4 = GetTargettedPlane(player, planeParent, "9", backAttack);
        }

        GameObject[] targetPlanes = new GameObject[] { targetPlane1, targetPlane2, targetPlane3, targetPlane4 };

        return targetPlanes;
    }

    public bool CheckForAttackUnit(string player, string attackName) {

        Transform attackOrigin = null;
        if (player == "Host")
        {
            attackOrigin = gameManager.playerCubePosition.transform.GetChild(1).Find("HighlightAttacks");
        }
        else if (player == "Client")
        {
            attackOrigin = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks");
        }

        List<GameObject> frontTargettingPlanes = new List<GameObject> { };

        frontTargettingPlanes.Add(attackOrigin.Find("TargettingPlanes2").GetChild(4).gameObject); //top of cube
        frontTargettingPlanes.Add(attackOrigin.Find("TargettingPlanes5").GetChild(4).gameObject); //left of cube
        frontTargettingPlanes.Add(attackOrigin.Find("TargettingPlanes1").GetChild(4).gameObject); //right of cube

        //int randomIndex = Random.Range(0, 3);

        for (int x = 0; x < frontTargettingPlanes.Count; x++) {

            RaycastHit[] hits;
            hits = Physics.RaycastAll(frontTargettingPlanes[x].transform.position, -frontTargettingPlanes[x].transform.up, 0.25f);

            for (int i = 0; i < hits.Length; i++)
            {
                //stored info from the enemy's unitPlane underneath
                RaycastHit hit = hits[i];
                var hitPlane = hit.transform.gameObject;

                //print(hitPlane.name);
                //this is where we filter out the weird self hits
                if (hitPlane.tag == "unitSquare")
                {
                    //return hitPlane.gameObject;

                    //checks the parent plane for all unitsquares
                    for (int y = 0; y < hitPlane.transform.parent.childCount; y++)
                    {
                        if (hitPlane.transform.parent.GetChild(y).childCount > 0) {

                            for (int z = 0; z < hitPlane.transform.parent.GetChild(y).childCount; z++)
                            {
                                TutorialUnitInformation unit = hitPlane.transform.parent.GetChild(y).GetChild(z).GetComponent<TutorialUnitInformation>();

                                if (hitPlane.transform.parent.GetChild(y).gameObject.CompareTag("unitSquare") && unit.attackName == attackName)
                                {
                                    // potentially randomize what unit does the attack ????????

                                    print("FOUND A UNIT");
                                    unit.DoAttackAnimation();

                                    frontTargettingPlanes.Clear();

                                    return true;
                                }
                            }

                        }
                    }

                }

            }
        }

        print("couldnt find unit");
        frontTargettingPlanes.Clear();
        return false;
    }


    public void HideEnemyFront()
    {

        Transform enemyCubeTargetting = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks");

        List<GameObject> targetFrontFaces = new List<GameObject>();

        targetFrontFaces.Add(enemyCubeTargetting.Find("TargettingPlanes2").GetChild(4).gameObject); //top of cube
        targetFrontFaces.Add(enemyCubeTargetting.Find("TargettingPlanes5").GetChild(4).gameObject); //left of cube
        targetFrontFaces.Add(enemyCubeTargetting.Find("TargettingPlanes1").GetChild(4).gameObject); //right of cube

        for (int x = 0; x < targetFrontFaces.Count; x++)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(targetFrontFaces[x].transform.position, -targetFrontFaces[x].transform.up, 0.25f);

            for (int i = 0; i < hits.Length; i++)
            {
                //stored info from the enemy's unitPlane underneath
                RaycastHit hit = hits[i];
                var hitPlane = hit.transform.gameObject;

                //print(hitPlane.name);
                //this is where we filter out the weird self hits
                if (hitPlane.tag == "unitSquare")
                {
                    print("hiding face");
                    hitPlane.transform.parent.gameObject.SetActive(false);
                    hiddenFrontFaces.Add(hitPlane.transform.parent.gameObject);

                }
            }

            targetFrontFaces[x].transform.parent.gameObject.SetActive(false);
            hiddenFrontFaces.Add(targetFrontFaces[x].transform.parent.gameObject);

        }

        //hides front single spot attack targetting
        Transform enemyCubeSingleTargetting = gameManager.enemyCubePosition.transform.GetChild(1).Find("SingleAttack");

        enemyCubeSingleTargetting.Find("TargettingPlanes1").gameObject.SetActive(false);
        hiddenFrontFaces.Add(enemyCubeSingleTargetting.Find("TargettingPlanes1").gameObject);
        enemyCubeSingleTargetting.Find("TargettingPlanes2").gameObject.SetActive(false);
        hiddenFrontFaces.Add(enemyCubeSingleTargetting.Find("TargettingPlanes2").gameObject);
        enemyCubeSingleTargetting.Find("TargettingPlanes5").gameObject.SetActive(false);
        hiddenFrontFaces.Add(enemyCubeSingleTargetting.Find("TargettingPlanes5").gameObject);


        //hides front tank attack targetting
        Transform enemyCubeTankTargetting = gameManager.enemyCubePosition.transform.GetChild(1).Find("TankAttack");

        enemyCubeTankTargetting.Find("TargettingPlanes1").gameObject.SetActive(false);
        hiddenFrontFaces.Add(enemyCubeTankTargetting.Find("TargettingPlanes1").gameObject);
        enemyCubeTankTargetting.Find("TargettingPlanes2").gameObject.SetActive(false);
        hiddenFrontFaces.Add(enemyCubeTankTargetting.Find("TargettingPlanes2").gameObject);
        enemyCubeTankTargetting.Find("TargettingPlanes5").gameObject.SetActive(false);
        hiddenFrontFaces.Add(enemyCubeTankTargetting.Find("TargettingPlanes5").gameObject);

    }

    public void ShowHidden()
    {
        print("showing front");
        for (int x = 0; x < hiddenFrontFaces.Count; x++)
        {
            hiddenFrontFaces[x].SetActive(true);
        }
        hiddenFrontFaces.Clear();
    }

    public void ShowBackAttack()
    {
        print("showing back");

        for (int x = 0; x < backTargets.Count; x++)
        {
            backTargets[x].SetActive(true);
            
            //if (backTargets[x].transform.Find("GameObject")) 
            //{
                //backTargets[x].transform.Find("GameObject").gameObject.SetActive(true);
            //}
        }
        //hiddenFrontFaces.Clear();
    }

    public void HideBackAttack()
    {
        print("hiding back");

        for (int x = 0; x < backTargets.Count; x++)
        {
            backTargets[x].SetActive(false);
        }
        //hiddenFrontFaces.Clear();
    }

    public void SetupBackTargetting() 
    {
        Transform enemyCubeHighlighting = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks");

        enemyCubeHighlighting.Find("TargettingPlanes3").gameObject.SetActive(false);
        backTargets.Add(enemyCubeHighlighting.Find("TargettingPlanes3").gameObject); //inside back left of cube

        enemyCubeHighlighting.Find("TargettingPlanes4").gameObject.SetActive(false);
        backTargets.Add(enemyCubeHighlighting.Find("TargettingPlanes4").gameObject); //inside bottom of cube

        enemyCubeHighlighting.Find("TargettingPlanes6").gameObject.SetActive(false);
        backTargets.Add(enemyCubeHighlighting.Find("TargettingPlanes6").gameObject); //inside back right of cube

        //-------------

        Transform enemyCubeSingleTargetting = gameManager.enemyCubePosition.transform.GetChild(1).Find("SingleAttack");

        enemyCubeSingleTargetting.Find("TargettingPlanes3").gameObject.SetActive(false);
        backTargets.Add(enemyCubeSingleTargetting.Find("TargettingPlanes3").gameObject);

        enemyCubeSingleTargetting.Find("TargettingPlanes4").gameObject.SetActive(false);
        backTargets.Add(enemyCubeSingleTargetting.Find("TargettingPlanes4").gameObject);

        enemyCubeSingleTargetting.Find("TargettingPlanes6").gameObject.SetActive(false);
        backTargets.Add(enemyCubeSingleTargetting.Find("TargettingPlanes6").gameObject);

        //---------------

        Transform enemyCubeTankTargetting = gameManager.enemyCubePosition.transform.GetChild(1).Find("TankAttack");

        enemyCubeTankTargetting.Find("TargettingPlanes3").gameObject.SetActive(false);
        backTargets.Add(enemyCubeTankTargetting.Find("TargettingPlanes3").gameObject);

        enemyCubeTankTargetting.Find("TargettingPlanes4").gameObject.SetActive(false);
        backTargets.Add(enemyCubeTankTargetting.Find("TargettingPlanes4").gameObject);

        enemyCubeTankTargetting.Find("TargettingPlanes6").gameObject.SetActive(false);
        backTargets.Add(enemyCubeTankTargetting.Find("TargettingPlanes6").gameObject);
    }

    public void ResetAttackBack(){
        ShowHidden();
        hideButton.SetActive(true);
        showButton.SetActive(false);
    }

    //   public void TurnOnTargetting(){
    // //print("Turning on targets");

    //   	for(int j = 0; j < gameManager.enemyCubePosition.transform.GetChild(1).childCount-1; j++){
    // 	gameManager.enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
    // 	// targetsystems.SetActive(false);
    // 	//print("turning enemy targets on");
    // }

    // for(int i = 0; i < gameManager.playerCubePosition.transform.GetChild(1).childCount-1; i++){
    // 	gameManager.playerCubePosition.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
    // 	// targetsystems.SetActive(false);
    // 	//print("turning player targets on");

    // }

    // //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

    //   }
    //   public void TurnOffTargetting(){
    // //print("Turning off targets");


    //   	for(int j = 0; j < gameManager.enemyCubePosition.transform.GetChild(1).childCount-1; j++){
    // 	gameManager.enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(false);
    // 	//print("turning enemy targets off");
    // 	// targetsystems.SetActive(false);
    // }

    // for(int i = 0; i < gameManager.playerCubePosition.transform.GetChild(1).childCount-1; i++){
    // 	gameManager.playerCubePosition.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
    // 	// targetsystems.SetActive(false);
    // 	//print("turning player targets off");

    // }

    // //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

    //   }
}

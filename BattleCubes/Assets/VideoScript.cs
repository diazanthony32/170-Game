using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoScript : MonoBehaviour
{
    public GameObject towerPrefab;

    public GameObject unit1;
    public GameObject unit2;
    public GameObject unit3;
    [Space(10)]
    public GameObject playerLocation;
    public GameManager gameManager;

    GameObject playerCubeTopFace;
    GameObject playerCubeLeftFace;
    GameObject playerCubeRightFace;
    GameObject playerCubeBackRightFace;
    GameObject playerCubeBottomFace;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Yeet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Yeet(){
        yield return new WaitForSeconds(1.0f);

        //----------------------------------------------------------------------------------------------------------------------------------------------------


        playerCubeTopFace = playerLocation.transform.GetChild(0).Find("Face_2").gameObject;
        playerCubeLeftFace = playerLocation.transform.GetChild(0).Find("Face_5").gameObject;
        playerCubeRightFace = playerLocation.transform.GetChild(0).Find("Face_1").gameObject;
        playerCubeBackRightFace = playerLocation.transform.GetChild(0).Find("Face_6").gameObject;
        playerCubeBottomFace = playerLocation.transform.GetChild(0).Find("Face_4").gameObject;

        GameObject enemyLeftFace = gameManager.enemyCubePosition.transform.GetChild(0).Find("Face_5").gameObject;
        GameObject enemyRightFace = gameManager.enemyCubePosition.transform.GetChild(0).Find("Face_1").gameObject;
        GameObject enemyTopFace = gameManager.enemyCubePosition.transform.GetChild(0).Find("Face_2").gameObject;


        ////SpawnEnemyUnit(unit1, enemyTopFace, "5", 180.0f);
        //SpawnEnemyUnit(towerPrefab, enemyTopFace, "3", 180.0f);



        ////SpawnEnemyUnit(unit2, enemyTopFace, "1", -90.0f);
        //GameObject e1 = Instantiate(unit2);
        //UnitInformation e1Info = e1.transform.GetComponent<UnitInformation>();

        //e1.transform.position = enemyTopFace.transform.Find("1").transform.position;
        //e1.transform.rotation = enemyTopFace.transform.Find("1").transform.rotation;
        //e1.transform.Rotate(0.0f, -90.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //e1.transform.SetParent(enemyTopFace.transform.Find("1").transform);

        //e1Info.ReColorUnit("Player", "Demon", "Blue");

        ////SpawnEnemyUnit(unit1, enemyTopFace, "5", 180.0f);
        //GameObject e2 = Instantiate(unit1);
        //UnitInformation e2Info = e2.transform.GetComponent<UnitInformation>();

        //e2.transform.position = enemyTopFace.transform.Find("5").transform.position;
        //e2.transform.rotation = enemyTopFace.transform.Find("5").transform.rotation;
        //e2.transform.Rotate(0.0f, 180.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //e2.transform.SetParent(enemyTopFace.transform.Find("5").transform);

        //e2Info.ReColorUnit("Player", "Demon", "Blue");




        //SpawnEnemyUnit(unit3, enemyLeftFace, "2", 90.0f);
        //SpawnEnemyUnit(towerPrefab, enemyLeftFace, "7", 90.0f);
        //SpawnEnemyUnit(unit2, enemyRightFace, "2", 0.0f);
        //SpawnEnemyUnit(unit1, enemyRightFace, "4", 0.0f);
        //SpawnEnemyUnit(unit1, enemyRightFace, "8", 0.0f);



        //SpawnUnit(towerPrefab, playerCubeTopFace, "4", 180.0f);
        ////SpawnUnit(unit3, playerCubeTopFace, "7", 180.0f);
        //SpawnUnit(unit2, playerCubeRightFace, "2", 180.0f);
        //SpawnUnit(unit1, playerCubeRightFace, "8", 180.0f);
        //SpawnUnit(unit1, playerCubeRightFace, "4", 180.0f);
        //SpawnUnit(towerPrefab, playerCubeRightFace, "5", 180.0f);
        //SpawnUnit(unit1, playerCubeLeftFace, "3", 180.0f);

        //GameObject tower = Instantiate(unit2);
        //UnitInformation towerInfo = tower.transform.GetComponent<UnitInformation>();

        //tower.transform.position = playerCubeTopFace.transform.Find("7").transform.position;
        //tower.transform.rotation = playerCubeTopFace.transform.Find("7").transform.rotation;
        //tower.transform.Rotate(0.0f, 180.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //tower.transform.SetParent(playerCubeTopFace.transform.Find("7").transform);

        //towerInfo.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //yield return new WaitForSeconds(8f);

        //towerInfo.DoAttackAnimation();
        //yield return new WaitForSeconds(1.25f);
        //AttackSpot(towerInfo.AttackParticle, enemyTopFace, "1");
        //AttackSpot(towerInfo.AttackParticle, enemyTopFace, "2");
        //AttackSpot(towerInfo.AttackParticle, enemyTopFace, "4");
        //AttackSpot(towerInfo.AttackParticle, enemyTopFace, "5");

        //yield return new WaitForSeconds(0.0f);
        //StartCoroutine(e1Info.Die());
        //StartCoroutine(e2Info.Die());




        //SpawnUnit(unit3, playerCubeBackRightFace, "2", 180.0f);
        //SpawnUnit(unit1, playerCubeBackRightFace, "7", 180.0f);
        //SpawnUnit(towerPrefab, playerCubeBackRightFace, "9", 180.0f);

        //SpawnUnit(towerPrefab, playerCubeRightFace, "6", 180.0f);
        //SpawnUnit(unit1, playerCubeRightFace, "1", 180.0f);

        //SpawnUnit(unit2, playerCubeBottomFace, "1", 180.0f);
        //SpawnUnit(unit1, playerCubeBottomFace, "5", 180.0f);



        //yield return new WaitForSeconds(2.5f);

        //towerInfo.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //GameObject tower = Instantiate(towerPrefab);
        //UnitInformation towerInfo = tower.transform.GetComponent<UnitInformation>();

        //tower.transform.position = playerCubeTopFace.transform.Find("1").transform.position;
        //tower.transform.rotation = playerCubeTopFace.transform.Find("1").transform.rotation;
        //tower.transform.Rotate(0.0f, 0.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //tower.transform.SetParent(playerCubeTopFace.transform.Find("1").transform);

        //towerInfo.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //yield return new WaitForSeconds(2f);

        //GameObject realUnit1 = Instantiate(unit2);
        //UnitInformation unitInfo1 = realUnit1.transform.GetComponent<UnitInformation>();

        //realUnit1.transform.position = playerCubeRightFace.transform.Find("1").transform.position;
        //realUnit1.transform.rotation = playerCubeRightFace.transform.Find("1").transform.rotation;
        //realUnit1.transform.Rotate(0.0f, 180.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //realUnit1.transform.SetParent(playerCubeRightFace.transform.Find("1").transform);

        //unitInfo1.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //yield return new WaitForSeconds(2f);

        //GameObject realUnit2 = Instantiate(unit1);
        //UnitInformation unitInfo2 = realUnit2.transform.GetComponent<UnitInformation>();

        //realUnit2.transform.position = playerCubeTopFace.transform.Find("9").transform.position;
        //realUnit2.transform.rotation = playerCubeTopFace.transform.Find("9").transform.rotation;
        //realUnit2.transform.Rotate(0.0f, 180.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //realUnit2.transform.SetParent(playerCubeTopFace.transform.Find("9").transform);

        //unitInfo2.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //yield return new WaitForSeconds(3.0f);

        //SpawnUnit( unit1, enemyTopFace, "5", 180.0f);
        //AttackSpot(unitInfo1.AttackParticle, playerCubeTopFace, "8");
        //unitInfo2.DoAttackAnimation();
        //unitInfo2.unitAnimator.SetTrigger("attack");
        //yield return new WaitForSeconds(0.1f);
        //unitInfo2.unitAnimator.SetTrigger("hit");
        //AttackSpot(unitInfo1.AttackParticle, playerCubeTopFace, "8");

        //AttackSpot(unitInfo1.AttackParticle, playerCubeTopFace, "8");



        //yield return new WaitForSeconds(3.0f);

        //gameManager.TranslateRotatePlayerCube("turn_L_down");

        //yield return new WaitForSeconds(2.5f);

        //gameManager.TranslateRotatePlayerCube("turn_left");

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        GameObject realUnit3 = Instantiate(unit3);
        UnitInformation unitInfo3 = realUnit3.transform.GetComponent<UnitInformation>();

        realUnit3.transform.position = playerCubeTopFace.transform.Find("5").transform.position;
        realUnit3.transform.rotation = playerCubeTopFace.transform.Find("5").transform.rotation;
        realUnit3.transform.Rotate(0.0f,  180.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        realUnit3.transform.SetParent(playerCubeTopFace.transform.Find("5").transform);

        unitInfo3.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        yield return new WaitForSeconds(1.5f);

        GameObject realUnit4 = Instantiate(towerPrefab);
        UnitInformation unitInfo4 = realUnit4.transform.GetComponent<UnitInformation>();

        //realUnit4.transform.position = playerCubeRightFace.transform.Find("3").transform.position;
        //realUnit4.transform.rotation = playerCubeRightFace.transform.Find("3").transform.rotation;
        //realUnit4.transform.Rotate(0.0f, 180.0f, 0.0f);
        //// Set unit as a child of the unitPlane
        //realUnit4.transform.SetParent(playerCubeRightFace.transform.Find("3").transform);

        //unitInfo4.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //unitInfo3.DoAttackAnimation();
        StartCoroutine(unitInfo3.Die());

        yield return new WaitForSeconds(1.0f);

        //gameManager.TranslateRotatePlayerCube("turn_L_down");

        AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "5");
        AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "6");
        AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "8");
        AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "9");

        //StartCoroutine(unitInfo3.Die());
        //unitInfo3.unitAnimator.SetTrigger("spawn");
        yield return new WaitForSeconds(1.0f);
        unitInfo3.DoAttackAnimation();


    }

    public void AttackSpot(GameObject particle, GameObject face, string place) {

        GameObject attackParticle = Instantiate(particle);
        attackParticle.transform.position = face.transform.Find(place).transform.position;
        attackParticle.transform.rotation = face.transform.Find(place).transform.rotation;
        attackParticle.transform.Rotate(0.0f, 180.0f, 0.0f);
        // Set unit as a child of the unitPlane
        attackParticle.transform.SetParent(face.transform.Find(place).transform);

    }

    public void SpawnUnit(GameObject particle, GameObject face, string place, float rotation) {
        GameObject realUnit69 = Instantiate(particle);
        UnitInformation unitInfo69 = realUnit69.transform.GetComponent<UnitInformation>();

        realUnit69.transform.position = face.transform.Find(place).transform.position;
        realUnit69.transform.rotation = face.transform.Find(place).transform.rotation;
        realUnit69.transform.Rotate(0.0f, rotation, 0.0f);
        // Set unit as a child of the unitPlane
        realUnit69.transform.SetParent(face.transform.Find(place).transform);

        unitInfo69.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));
    }
    public void SpawnEnemyUnit(GameObject particle, GameObject face, string place, float rotation)
    {
        GameObject realUnit69 = Instantiate(particle);
        UnitInformation unitInfo69 = realUnit69.transform.GetComponent<UnitInformation>();

        realUnit69.transform.position = face.transform.Find(place).transform.position;
        realUnit69.transform.rotation = face.transform.Find(place).transform.rotation;
        realUnit69.transform.Rotate(0.0f, rotation, 0.0f);
        // Set unit as a child of the unitPlane
        realUnit69.transform.SetParent(face.transform.Find(place).transform);

        unitInfo69.ReColorUnit("Player", "Demon", "Blue");
    }
}

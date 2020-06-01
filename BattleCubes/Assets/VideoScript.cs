using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoScript : MonoBehaviour
{
    public GameObject unit1;
    public GameObject unit2;
    public GameObject unit3;
    [Space(10)]
    public GameObject playerLocation;
    GameObject playerCubeTopFace;


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

        playerCubeTopFace = playerLocation.transform.GetChild(0).Find("Face_2").gameObject;

        GameObject realUnit1 = Instantiate(unit1);
        UnitInformation unitInfo1 = realUnit1.transform.GetComponent<UnitInformation>();

        realUnit1.transform.position = playerCubeTopFace.transform.Find("2").transform.position;
        realUnit1.transform.rotation = playerCubeTopFace.transform.Find("2").transform.rotation;
        realUnit1.transform.Rotate(0.0f, 180.0f, 0.0f);
        // Set unit as a child of the unitPlane
        realUnit1.transform.SetParent(playerCubeTopFace.transform.Find("2").transform);

        unitInfo1.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        yield return new WaitForSeconds(1.5f);

        GameObject realUnit2 = Instantiate(unit2);
        UnitInformation unitInfo2 = realUnit2.transform.GetComponent<UnitInformation>();

        realUnit2.transform.position = playerCubeTopFace.transform.Find("6").transform.position;
        realUnit2.transform.rotation = playerCubeTopFace.transform.Find("6").transform.rotation;
        realUnit2.transform.Rotate(0.0f, 180.0f, 0.0f);
        // Set unit as a child of the unitPlane
        realUnit2.transform.SetParent(playerCubeTopFace.transform.Find("6").transform);

        unitInfo2.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        yield return new WaitForSeconds(2.0f);

        GameObject realUnit3 = Instantiate(unit3);
        UnitInformation unitInfo3 = realUnit3.transform.GetComponent<UnitInformation>();

        realUnit3.transform.position = playerCubeTopFace.transform.Find("5").transform.position;
        realUnit3.transform.rotation = playerCubeTopFace.transform.Find("5").transform.rotation;
        realUnit3.transform.Rotate(0.0f,  180.0f, 0.0f);
        // Set unit as a child of the unitPlane
        realUnit3.transform.SetParent(playerCubeTopFace.transform.Find("5").transform);

        unitInfo3.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        yield return new WaitForSeconds(2.0f);

        unitInfo3.DoAttackAnimation();
        yield return new WaitForSeconds(1.0f);

        //AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "3");
        //AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "2");
        //AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "5");
        //AttackSpot(unitInfo3.AttackParticle, playerCubeTopFace, "6");


    }

    public void AttackSpot(GameObject particle, GameObject face, string place) {

        GameObject attackParticle = Instantiate(particle);
        attackParticle.transform.position = face.transform.Find(place).transform.position;
        attackParticle.transform.rotation = face.transform.Find(place).transform.rotation;
        attackParticle.transform.Rotate(0.0f, 180.0f, 0.0f);
        // Set unit as a child of the unitPlane
        attackParticle.transform.SetParent(face.transform.Find(place).transform);

    }
}

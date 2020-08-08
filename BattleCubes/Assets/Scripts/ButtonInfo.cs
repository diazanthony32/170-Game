using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour {
    [SerializeField] RotationByFinger swiperPannel;
    public string CubeTheme;
    public string CubeColor;
    [SerializeField] GameObject background;


    public void ChangeCube() {
        GameObject playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        //Destroy(playerCubePosition.transform.Find("Prefab(Clone)").gameObject);

        PlayerPrefs.SetString("CubeTheme", CubeTheme);
        PlayerPrefs.SetString("CubeColor", CubeColor);

        //GameObject cube = Instantiate(Resources.Load<GameObject>("Themes/" + CubeTheme + "/" + CubeColor + "/Cube"));
        //cube.transform.position = playerCubePosition.transform.position;
        //cube.transform.rotation = playerCubePosition.transform.rotation;
        //cube.transform.SetParent(playerCubePosition.transform);

        Recolor();

        if (Resources.Load<Sprite>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/main_background"))
        {
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/main_background");
        }

        //swiperPannel.ChangeCube(cube);
    }

    public void Recolor() {
        GameObject playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        Transform playerCubeTransform = playerCubePosition.transform.GetChild(0);

        CubeInformation cubeInformation = playerCubeTransform.GetComponent<CubeInformation>();

        for (int x = 0; x < playerCubeTransform.childCount; x++) {
            for (int i = 0; i < playerCubeTransform.GetChild(x).childCount; i++) {
                //int randomIndex = Random.Range(0, 8);

                if (playerCubeTransform.GetChild(x).GetChild(i).tag == "unitSquare" && playerCubeTransform.GetChild(x).GetChild(i).childCount > 0) {
                    for (int k = 0; k < playerCubeTransform.GetChild(x).GetChild(i).childCount; k++) {
                        UnitInformation unitInformation = playerCubeTransform.GetChild(x).GetChild(i).GetChild(k).GetComponent<UnitInformation>();
                        unitInformation.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

                        if (unitInformation.isTower) {
                            //print("Tower Found");
                            unitInformation.GetComponent<Shield>().ChangeMaterial();
                        }
                    }
                }
            }

        }

        cubeInformation.ReColorCube("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));
    }
}

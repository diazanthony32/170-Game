using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartUp : MonoBehaviour
{
    [SerializeField] GameObject newPlayerMenu;
    [SerializeField] GameObject mainMenu;
    [Space(10)]
    [SerializeField] GameObject playerCubePosition;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] GameObject background;

    CubeInformation cubeInformation;

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName")) {
            GameObject.FindGameObjectWithTag("playerName").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("PlayerName", "no name");
            newPlayerMenu.SetActive(false);
            mainMenu.SetActive(true);

        }
        else {
            newPlayerMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        if (PlayerPrefs.HasKey("CubeTheme")) {
            print(PlayerPrefs.GetString("CubeTheme"));
            if (PlayerPrefs.HasKey("CubeColor")) {
                print(PlayerPrefs.GetString("CubeColor"));
                print(("Themes/" + PlayerPrefs.GetString("CubeTheme") + "/"+ PlayerPrefs.GetString("CubeColor")));
                GameObject cube = Instantiate(Resources.Load<GameObject>("Themes/"+PlayerPrefs.GetString("CubeTheme")+"/Cube/Prefab"));
                cube.transform.position = playerCubePosition.transform.position;
                cube.transform.rotation = playerCubePosition.transform.rotation;
                cube.transform.SetParent(playerCubePosition.transform);

                cubeInformation = cube.GetComponent<CubeInformation>();

            }
            else {
                GameObject cube = Instantiate(Resources.Load<GameObject>("Themes/" + PlayerPrefs.GetString("CubeTheme") + "/Cube/Prefab"));
                cube.transform.position = playerCubePosition.transform.position;
                cube.transform.rotation = playerCubePosition.transform.rotation;
                cube.transform.SetParent(playerCubePosition.transform);

                PlayerPrefs.SetString("CubeTheme", "Demon");
                PlayerPrefs.SetString("CubeColor", "0");

                cubeInformation = cube.GetComponent<CubeInformation>();

            }
        }
        else {
            GameObject cube = Instantiate(Resources.Load<GameObject>("Themes/Demon/Cube/Prefab"));
            cube.transform.position = playerCubePosition.transform.position;
            cube.transform.rotation = playerCubePosition.transform.rotation;
            cube.transform.SetParent(playerCubePosition.transform);

            PlayerPrefs.SetString("CubeTheme", "Demon");
            PlayerPrefs.SetString("CubeColor", "0");

            cubeInformation = cube.GetComponent<CubeInformation>();
        }

        if(PlayerPrefs.HasKey("MusicVolume")){
            audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume"));
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }

        if (Resources.Load<Sprite>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/main_background"))
        {
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Themes/Demon/Colors/" + PlayerPrefs.GetString("CubeColor") + "/main_background");
        }

        //Only for testing purposes. dont need this, this spawns the targetting system on the main menu cube
        GameObject cubeTargeting = Instantiate(Resources.Load<GameObject>("MainCubePrefab/CubeTargeting"));
        cubeTargeting.transform.position = playerCubePosition.transform.position;
        cubeTargeting.transform.rotation = playerCubePosition.transform.rotation;
        cubeTargeting.transform.SetParent(playerCubePosition.transform);

        GameObject cubeAttackChecker = Instantiate(Resources.Load<GameObject>("MainCubePrefab/AttackChecker"));
        cubeAttackChecker.transform.position = playerCubePosition.transform.position;
        cubeAttackChecker.transform.rotation = playerCubePosition.transform.rotation;
        cubeAttackChecker.transform.SetParent(playerCubePosition.transform);

        GameObject cubeHideUnits = Instantiate(Resources.Load<GameObject>("MainCubePrefab/HideUnits"));
        cubeHideUnits.transform.position = playerCubePosition.transform.position;
        cubeHideUnits.transform.rotation = playerCubePosition.transform.rotation;
        cubeHideUnits.transform.SetParent(playerCubePosition.transform);

        RandomUnitPlacement();

        cubeInformation.ReColorCube("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

    }

    public void RandomUnitPlacement()
    {
        Transform playerCubeTransform = playerCubePosition.transform.GetChild(0);
        int unitPoints = 12;
        int towerCount = 3;
        while (unitPoints != 0 || towerCount != 0) {

            //print("unit points: " + unitPoints);
            //print("tower count: " + towerCount);

            for (int x = 0; x < playerCubeTransform.childCount; x++)
            {
                for (int i = 0; i < playerCubeTransform.GetChild(x).childCount; i++)
                {
                    int randomIndex = Random.Range(0, 8);

                    if (playerCubeTransform.GetChild(x).GetChild(i).tag == "unitSquare" && randomIndex == 1)
                    {
                        string[] cubeInfo = { PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor") };

                        int unitFolder = Random.Range(1, 5);
                        GameObject unitPrefab;

                        if (unitPoints <= 0) {
                            unitFolder = 4;
                        }

                        if (unitFolder == 4)
                        {
                            unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/" + cubeInfo[0] + "/Units/Tower/Prefab"));
                        }
                        else
                        {
                            unitPrefab = Instantiate(Resources.Load<GameObject>("Themes/" + cubeInfo[0] + "/Units/" + unitFolder + "/Prefab"));
                        }

                        //print("spawned a " + unitPrefab.GetComponent<UnitInformation>().unitName);

                        if (unitPoints >= unitPrefab.GetComponent<UnitInformation>().unitSpawnCost) {
                            //unitPrefab.transform.SetParent(playerCubeTransform.GetChild(x).GetChild(i));
                            //print("inside placement check");
                            if (unitPrefab.GetComponent<UnitInformation>().isTower && towerCount != 0) {

                                // the variable responible for allowing the Tower to be Placed on a face
                                bool safeToPlace = true;

                                // checks if there is another tower on the same face of where the tower wants to be placed
                                // if so, the tower placement is invalid
                                for (int y = 0; y < playerCubeTransform.GetChild(x).GetChild(i).transform.parent.childCount; y++)
                                {
                                    var plane = playerCubeTransform.GetChild(x).GetChild(i).transform.parent.GetChild(y);

                                    if (plane.gameObject.transform.childCount > 0 && plane.gameObject.CompareTag("unitSquare"))
                                    {
                                        for (int z = 0; z < plane.gameObject.transform.childCount; z++)
                                        {
                                            if (plane.gameObject.transform.GetChild(z).gameObject.GetComponent<UnitInformation>().isTower)
                                            {
                                                safeToPlace = false;
                                            }

                                        }

                                    }

                                }

                                if (safeToPlace && playerCubeTransform.GetChild(x).GetChild(i).childCount < 1) 
                                {
                                    unitPrefab.transform.position = playerCubeTransform.GetChild(x).GetChild(i).transform.position;
                                    unitPrefab.transform.rotation = playerCubeTransform.GetChild(x).GetChild(i).transform.rotation;

                                    var rand = Random.Range(0, 4);
                                    unitPrefab.transform.Rotate(0.0f, (rand * 90.0f), 0.0f);

                                    unitPrefab.transform.SetParent(playerCubeTransform.GetChild(x).GetChild(i).transform);
                                    towerCount--;
                                }
                                else
                                {
                                    //print("destroyed a " + unitPrefab.GetComponent<UnitInformation>().unitName);
                                    Destroy(unitPrefab);
                                }
                            }
                            else if (!unitPrefab.GetComponent<UnitInformation>().isTower && playerCubeTransform.GetChild(x).GetChild(i).childCount < 1)
                            {
                                unitPrefab.transform.position = playerCubeTransform.GetChild(x).GetChild(i).transform.position;
                                unitPrefab.transform.rotation = playerCubeTransform.GetChild(x).GetChild(i).transform.rotation;

                                var rand = Random.Range(0, 4);
                                unitPrefab.transform.Rotate(0.0f, (rand * 90.0f), 0.0f);

                                unitPrefab.transform.SetParent(playerCubeTransform.GetChild(x).GetChild(i).transform);
                                unitPoints -= unitPrefab.GetComponent<UnitInformation>().unitSpawnCost;
                            }
                            else
                            {
                                //print("destroyed a " + unitPrefab.GetComponent<UnitInformation>().unitName);
                                Destroy(unitPrefab);
                            }
                        }
                        else
                        {
                            //print("destroyed a " + unitPrefab.GetComponent<UnitInformation>().unitName);
                            Destroy(unitPrefab);
                        }
                    }
                }

                //print("unit points: " + unitPoints);
                //print("tower count: " + towerCount);

            }
        }
    }
}

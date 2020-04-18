using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartUp : MonoBehaviour
{
    [SerializeField] GameObject playerCubePosition;
    

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName")) {
            GameObject.FindGameObjectWithTag("playerName").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("PlayerName", "no name");
            GameObject.FindGameObjectWithTag("loginMenu").SetActive(false);
        }
        else {
            GameObject.FindGameObjectWithTag("loginMenu").SetActive(true);
            gameObject.SetActive(false);
        }

        if (PlayerPrefs.HasKey("CubeTheme")) {
            if (PlayerPrefs.HasKey("CubeColor")) {
                GameObject cube = Instantiate(Resources.Load<GameObject>(PlayerPrefs.GetString("CubeTheme") + "/" + PlayerPrefs.GetString("CubeColor") + "/Cube"));
                cube.transform.position = playerCubePosition.transform.position;
                cube.transform.rotation = playerCubePosition.transform.rotation;
                cube.transform.SetParent(playerCubePosition.transform);
            }
            else {
                GameObject cube = Instantiate(Resources.Load<GameObject>(PlayerPrefs.GetString("CubeTheme") + "/DefaultCube/Cube"));
                cube.transform.position = playerCubePosition.transform.position;
                cube.transform.rotation = playerCubePosition.transform.rotation;
                cube.transform.SetParent(playerCubePosition.transform);
            }
        }
        else {
            GameObject cube = Instantiate(Resources.Load<GameObject>("Demons/DefaultCube/Cube"));
            cube.transform.position = playerCubePosition.transform.position;
            cube.transform.rotation = playerCubePosition.transform.rotation;
            cube.transform.SetParent(playerCubePosition.transform);

            PlayerPrefs.SetString("CubeTheme", "Demons");
            PlayerPrefs.SetString("CubeColor", "DefaultCube");
        }

        //Only for testing purposes. dont need this, this spawns the targetting system on the main menu cube
        GameObject cubeTargeting = Instantiate(Resources.Load<GameObject>("MainCubePrefab/CubeTargeting"));
        cubeTargeting.transform.position = playerCubePosition.transform.position;
        cubeTargeting.transform.rotation = playerCubePosition.transform.rotation;
        cubeTargeting.transform.SetParent(playerCubePosition.transform);
    }
}

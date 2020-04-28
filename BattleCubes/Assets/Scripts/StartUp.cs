using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartUp : MonoBehaviour
{
    [SerializeField] GameObject playerCubePosition;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;


    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName")) {
            GameObject.FindGameObjectWithTag("playerName").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("PlayerName", "no name");
            GameObject.FindGameObjectWithTag("loginMenu").SetActive(false);
        }
        else {
            GameObject.FindGameObjectWithTag("loginMenu").SetActive(true);
            GameObject.FindGameObjectWithTag("mainMenu").SetActive(false);
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

        if(PlayerPrefs.HasKey("MusicVolume")){
            audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("MusicVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXVolume"));
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
    void Start() 
    {
        if (PlayerPrefs.HasKey("PlayerName")) 
        {
            GameObject.FindGameObjectWithTag("playerName").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("PlayerName", "no name");
            GameObject.FindGameObjectWithTag("loginMenu").SetActive(false);
        }
        else 
        {
            GameObject.FindGameObjectWithTag("loginMenu").SetActive(true);
            gameObject.SetActive(false);
        }
    }
    public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Debug.Log("Successfully Quit Game");
		Application.Quit();
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume(float volume){
    	audioMixer.SetFloat("Music", volume);
    	PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void QuitGame() {
        Application.Quit();
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/BattleCubesGame");

    }

    public void OpenWebsite()
    {
        Application.OpenURL("mentalblockgames.com");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour {
    public string CubeColor;
    public CubeCustomization cubeCustomization;

    public void ColorChange() 
    {
        PlayerPrefs.SetString("CubeColor", CubeColor);
        cubeCustomization.Recolor();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CubeCustomization : MonoBehaviour
{
    public List<string> cubeThemes;
    
    [Space(10)]
    
    public TextMeshProUGUI themeNameText;
    public Image background;
    
    [Space(10)]

    [SerializeField] GameObject colorButtons;

    int currentThemeIndex;

    string playerTheme;
    string themeColor;
    [Space(10)]

    [SerializeField] CanvasGroup previousThemeButton;
    bool ptbState = true;

    [SerializeField] CanvasGroup nextThemeButton;
    bool ntbState = true;

    [SerializeField] CanvasGroup confirmButton;

    // Start is called before the first frame update
    void Start()
    {
        playerTheme = PlayerPrefs.GetString("CubeTheme");
        themeColor = PlayerPrefs.GetString("CubeColor");

        for (int i = 0 ; i < cubeThemes.Count; i++) 
        {
            if (cubeThemes[i] == playerTheme) 
            {
                currentThemeIndex = i;
            }
        }
        //print(cubeThemes[currentThemeIndex] + ":" + currentThemeIndex);
        themeNameText.text = cubeThemes[currentThemeIndex];
        SetCubeColors();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentThemeIndex < cubeThemes.Count - 1)
        {
            previousThemeButton.alpha = 0.1f;
            previousThemeButton.interactable = false;
        }
        else //if(currentThemeIndex > cubeThemes.Count - 1 && !ptbState)
        {
            previousThemeButton.alpha = 1f;
            previousThemeButton.interactable = true;
        }


        if (currentThemeIndex > 0)
        {
            nextThemeButton.alpha = 0.1f;
            nextThemeButton.interactable = false;
        }
        else //if (currentThemeIndex < 0 && !ntbState) 
        {
            nextThemeButton.alpha = 1f;
            nextThemeButton.interactable = true;
        }

        if (playerTheme == "Coming Soon")
        {
            confirmButton.alpha = 0.1f;
            confirmButton.interactable = false;
        }
        else {
            confirmButton.alpha = 1f;
            confirmButton.interactable = true;
        }
    }

    void GetCurrentThemeAndColor() {
        playerTheme = PlayerPrefs.GetString("CubeTheme");
        themeColor = PlayerPrefs.GetString("CubeColor");
    }

    void SetCubeColors() {
        for (int i = 0; i < colorButtons.transform.childCount; i++)
        {
            Transform child = colorButtons.transform.GetChild(i);
            //if (child.CompareTag("colors"))
            //{
                if (playerTheme != "Coming Soon" && Resources.Load<Sprite>("Themes/" + playerTheme + "/Colors/" + i + "/c_palette"))
                {
                    print("Set Color for: " + child.name);
                    child.GetComponent<CanvasGroup>().alpha = 1f;
                    child.GetComponent<CanvasGroup>().interactable = true;

                    child.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Themes/" + playerTheme + "/Colors/" + i + "/c_palette");
                    child.GetComponent<ButtonInfo>().CubeColor = i.ToString();
                }
                else {
                    print("Missing color: " + child.name);
                    child.GetComponent<CanvasGroup>().alpha = 0.1f;
                    child.GetComponent<CanvasGroup>().interactable = false;

                    child.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("actionIcons/BattleCubesLogo");
                    child.GetComponent<ButtonInfo>().CubeColor = "";
                }

            //}
        }
    }

    public void NextTheme() { 
        if(currentThemeIndex < cubeThemes.Count-1) 
        {
            currentThemeIndex += 1;
            playerTheme = cubeThemes[currentThemeIndex];
            PlayerPrefs.SetString("CubeTheme", cubeThemes[currentThemeIndex]);
            themeNameText.text = cubeThemes[currentThemeIndex];

            ReplaceUnits();
            //SetCubeColors();
            print(cubeThemes[currentThemeIndex]);
        }
    }

    public void PrevTheme()
    {
        if (currentThemeIndex > 0)
        {
            currentThemeIndex -= 1;
            playerTheme = cubeThemes[currentThemeIndex];
            PlayerPrefs.SetString("CubeTheme", cubeThemes[currentThemeIndex]);
            themeNameText.text = cubeThemes[currentThemeIndex];

            ReplaceUnits();
            //SetCubeColors();
            print(cubeThemes[currentThemeIndex]);
        }
    }

    public void ReplaceUnits()
    {
        themeColor = "0";
        PlayerPrefs.SetString("CubeColor", "0");

        GameObject playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        Transform playerCubeTransform = playerCubePosition.transform.GetChild(0);

        CubeInformation cubeInformation = playerCubeTransform.GetComponent<CubeInformation>();

        for (int x = 0; x < playerCubeTransform.childCount; x++)
        {
            for (int i = 0; i < playerCubeTransform.GetChild(x).childCount; i++)
            {
                if (playerCubeTransform.GetChild(x).GetChild(i).tag == "unitSquare" && playerCubeTransform.GetChild(x).GetChild(i).childCount > 0)
                {
                    for (int k = 0; k < playerCubeTransform.GetChild(x).GetChild(i).childCount; k++)
                    {
                        Destroy(playerCubeTransform.GetChild(x).GetChild(i).GetChild(k).gameObject);
                    }
                }
            }
        }

        if (playerTheme != "Coming Soon") { 
            GameObject.Find("Start Up").GetComponent<StartUp>().RandomUnitPlacement();
        }
        //GameObject.Find("Start Up").GetComponent<StartUp>().RandomUnitPlacement();
        //cubeInformation.ReColorCube("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        Recolor();
        SetCubeColors();
    }

    public void Recolor()
    {
        themeColor = PlayerPrefs.GetString("CubeColor");

        GameObject playerCubePosition = GameObject.FindGameObjectWithTag("PlayerCubePosition");

        Transform playerCubeTransform = playerCubePosition.transform.GetChild(0);

        CubeInformation cubeInformation = playerCubeTransform.GetComponent<CubeInformation>();

        //if (PlayerPrefs.GetString("CubeTheme") != "Coming Soon")
        //{
            for (int x = 0; x < playerCubeTransform.childCount; x++)
            {
                for (int i = 0; i < playerCubeTransform.GetChild(x).childCount; i++)
                {
                    //int randomIndex = Random.Range(0, 8);

                    if (playerCubeTransform.GetChild(x).GetChild(i).tag == "unitSquare" && playerCubeTransform.GetChild(x).GetChild(i).childCount > 0)
                    {
                        for (int k = 0; k < playerCubeTransform.GetChild(x).GetChild(i).childCount; k++)
                        {
                            UnitInformation unitInformation = playerCubeTransform.GetChild(x).GetChild(i).GetChild(k).GetComponent<UnitInformation>();
                            unitInformation.ReColorUnit("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

                            if (unitInformation.isTower)
                            {
                                //print("Tower Found");
                                unitInformation.GetComponent<Shield>().ChangeMaterial();
                            }
                        }
                    }
                }

            }

            cubeInformation.ReColorCube("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        //}


        //cubeInformation.ReColorCube("Player", PlayerPrefs.GetString("CubeTheme"), PlayerPrefs.GetString("CubeColor"));

        if (Resources.Load<Sprite>("Themes/" + playerTheme + "/Colors/" + PlayerPrefs.GetString("CubeColor") + "/main_background"))
        {
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Themes/" + playerTheme + "/Colors/" + PlayerPrefs.GetString("CubeColor") + "/main_background");
        }
    }
}

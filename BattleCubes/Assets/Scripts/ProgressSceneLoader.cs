using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ProgressSceneLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ProgressText;
    [SerializeField] Slider ProgressBar;

    private static bool ProgressSceneLoaderExists;

    private AsyncOperation operation;
    private Canvas canvas;

    public void Awake() {
        if (!ProgressSceneLoaderExists) {
            ProgressSceneLoaderExists = true;
            canvas = GetComponentInChildren<Canvas>(true);
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Loadscene(string sceneName = "") {
        UpdateProgressUI(0);
        canvas.gameObject.SetActive(true);

        StartCoroutine(BeginLoad(sceneName));
    }

    public void Loadscene(int i) {
        UpdateProgressUI(0);
        canvas.gameObject.SetActive(true);

        StartCoroutine(BeginLoad(i));
    }

    private IEnumerator BeginLoad(string sceneName) {
        operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone) {
            UpdateProgressUI(operation.progress);
            yield return null;
        }

        UpdateProgressUI(operation.progress);
        operation = null;
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator BeginLoad(int i) {
        operation = SceneManager.LoadSceneAsync(i);

        while (operation != null && !operation.isDone) {
            UpdateProgressUI(operation.progress);
            yield return null;
        }

        if (operation != null) {
            UpdateProgressUI(operation.progress);
            operation = null;
            canvas.gameObject.SetActive(false);
        }
    }

    private void UpdateProgressUI(float progress) {
        ProgressBar.value = progress;
        ProgressText.text = (int)(progress * 100f) + "%";
    }
}

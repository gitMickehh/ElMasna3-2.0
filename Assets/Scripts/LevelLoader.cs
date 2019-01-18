using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public Image loadingFillImage;

    public void LoadLevel(int sceneNumber)
    {
        StartCoroutine(LoadAsynchorounsly(sceneNumber));
    }

    IEnumerator LoadAsynchorounsly(int sceneNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/ 0.9f);
            //Debug.Log(progress);
            loadingFillImage.fillAmount = progress;

            yield return null;
        }

    }

    public void LoadLevelAdditive(int sceneNumber)
    {
        StartCoroutine(LoadAsynchorounslyAdditive(sceneNumber));
    }

    IEnumerator LoadAsynchorounslyAdditive(int sceneNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber, LoadSceneMode.Additive);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(progress);
            loadingFillImage.fillAmount = progress;

            yield return null;
        }

        loadingScreen.SetActive(false);
    }

}

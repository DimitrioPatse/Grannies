using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{

    [SerializeField] Slider loadingBar;
    [SerializeField] GameObject loadingImage;

    int scene;
    AsyncOperation async;

    void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel(string name)
    {
        if (loadingImage && loadingBar)
        {
            loadingBar.gameObject.SetActive(true);
            if (loadingImage != null)
            {
                loadingImage.SetActive(true);
            }
            StartCoroutine(LoadLevelWithBar(name));
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }

    IEnumerator LoadLevelWithBar(string name)
    {
        async = SceneManager.LoadSceneAsync(name);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }

    }
    public void HomeMenu()
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();
        SceneManager.LoadSceneAsync(1);
    }
    public void Retry()
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.LoadLastLevel();
    }
    public void Quit()
    {
        Application.Quit();
    }
}

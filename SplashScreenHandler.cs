using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenHandler : MonoBehaviour
{
    AudioSource myaudio;

    void Start()
    {
        myaudio = GetComponent<AudioSource>();
        Invoke("loadHomeMenu", myaudio.clip.length);
    }

    void loadHomeMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }

}

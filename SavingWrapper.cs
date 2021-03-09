using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save";
    [SerializeField] float fadeInTime = 0.5f;
    private void Awake()
    {
        //LoadLastLevel();
    }
    IEnumerator LoadLastScene()
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        Fader fader = GameObject.FindObjectOfType<Fader>();
        fader.FadeOutImmidiately();
        yield return fader.FadeIn(fadeInTime);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Load(false);
            //LoadLastLevel();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Load(true);
            //LoadLastLevel();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Save();
            print("Save file created");

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DeleteAll();
            print("Save file deleted");
        }
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load(bool onlyPermanent)
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile, onlyPermanent); 
    }
    public void LoadLastLevel()
    {
        StartCoroutine(LoadLastScene());
    }

    public void DeleteAll()
    {
        GetComponent<SavingSystem>().DeleteAll(defaultSaveFile);
        print("Save file deleted");

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjectPrefab;

    static bool persistentHasSpawned = false;

    private void Awake()
    {
        if (persistentHasSpawned) return;

        SpawnPersistentObjects();

        persistentHasSpawned = true;
    }

    void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
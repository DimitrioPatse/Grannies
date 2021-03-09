using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game_Master : MonoBehaviour
{ 
    List<Transform> enemies = new List<Transform>();
    GameObject player;
    Portal exitPortal;

    public Action onLevelComplete;
    public Action onPlayerDeath;
    void Start()
    {
        FindObjectOfType<SavingWrapper>().Load(true);
        player = GameObject.FindGameObjectWithTag("Player");
        ScanForEnemies();

        if (!exitPortal)
        {
            exitPortal = FindObjectOfType<Portal>();
        }
    }

    void Update()
    {
        if (!enemies.Any() && exitPortal)
        {
            exitPortal.EnableExit();
        }
    }

    public void SetPortal(Portal portal)
    {
        exitPortal = portal;
    }

    void ScanForEnemies()
    {
        Collider scanArea = GameObject.FindGameObjectWithTag("ScanArea").GetComponent<Collider>();
        RaycastHit[] enemyHits = Physics.BoxCastAll(scanArea.bounds.center, scanArea.bounds.extents, Vector3.one, Quaternion.identity, 100, LayerMask.GetMask("Enemy"));
        enemies.Clear();
        foreach (RaycastHit enemy in enemyHits)
        {
            enemies.Add(enemy.transform);
        }
    }

    void SortEnemyList()
    {
        enemies = enemies.OrderBy(x => (player.transform.position - x.transform.position).sqrMagnitude).ToList();
    }

    public Transform GetRandomTarget()
    {
        if (enemies.Any())
        {
            return enemies[0];
        }
        return null;
    }
    public Transform GetClosestTarget()
    {
        if (enemies.Any())
        {
            SortEnemyList();
            return enemies[0];
        }
        return null;
    }

    public void RemoveFromEnemyList(Transform enemyTransform)
    {
        enemies.Remove(enemyTransform);
        if (enemies.Any()) {
            SortEnemyList();
        }
        else
        {
            exitPortal.EnableExit();
            onLevelComplete?.Invoke();
        }
    }

    public void PlayerDeath()
    {
        onPlayerDeath.Invoke();
    }
}
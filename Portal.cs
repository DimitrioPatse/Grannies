using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] bool isRestarter;
    [SerializeField] int loadScene = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeWaitTime = 0.5f;

    int nextSceneIndex;
    bool isEnabled = false;

    private void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }
    public void EnableExit()
    {
        ParticleSystem.MainModule psMain = GetComponent<ParticleSystem>().main;
        psMain.startColor = Color.green;
        isEnabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isEnabled)
        {
            StartCoroutine(Transition());
        }
    }
    IEnumerator Transition()
    {
        if (loadScene < 0)
        {
            Debug.LogError(gameObject.name + "  has not set LoadScene");
            yield break;
        }
        
        DontDestroyOnLoad(gameObject);

        Fader fader = GameObject.FindObjectOfType<Fader>();

        yield return fader.FadeOut(fadeOutTime);

        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.enabled = false;

        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();

        yield return SceneManager.LoadSceneAsync(isRestarter? loadScene : nextSceneIndex);

        PlayerController newPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // Ksanapsaxneis gia player gt allakse skhnh kai einai neos
        newPlayerController.enabled = false;

        wrapper.Load(false);


        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        wrapper.Save();

        yield return new WaitForSeconds(fadeWaitTime);

        Fader newFader = GameObject.FindObjectOfType<Fader>();
        newFader.FadeIn(fadeInTime);

        newPlayerController.enabled = true;

        Game_Master gm = FindObjectOfType<Game_Master>();
        gm.SetPortal(otherPortal);

        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = otherPortal.spawnPoint.position;
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            return portal;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
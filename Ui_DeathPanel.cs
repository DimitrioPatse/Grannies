using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_DeathPanel : MonoBehaviour
{
    Game_Master gm;
    Transform[] obj;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<Game_Master>();
        gm.onPlayerDeath += EnableDeathPanel;
        GetPanelObjects();
        DisableDeathPanel();
    }

    void DisableDeathPanel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            obj[i].gameObject.SetActive(false);
        }
    }

    void EnableDeathPanel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            obj[i].gameObject.SetActive(true);
        }
    }

    void GetPanelObjects()
    {
        obj = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            obj[i] = transform.GetChild(i);
        }
    }
}

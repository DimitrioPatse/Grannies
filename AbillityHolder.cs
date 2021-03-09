using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbillityHolder : MonoBehaviour
{

    public AbillityClass abillity { get; set; }

    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AddAbillity);
    }

    public void AddAbillity()
    {
        AbillitySystem abSys = FindObjectOfType<AbillitySystem>();
        AbillitySelector abSel = FindObjectOfType<AbillitySelector>();

        if (!abSys || !abSel)
        {
            Debug.Log("No (Abillity (System or Selector)) Found in abillity button " + gameObject.name);
        }

        abSys.AddAbillity(abillity);
        abSel.SetUseOfAbillity(abillity);
        MainGameUiManager uiMng = FindObjectOfType<MainGameUiManager>();
        uiMng.HideAbillitySelector();
    }
}

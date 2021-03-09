using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUiManager : MonoBehaviour
{
    [Header("Skill Info Panel Properties")]
    [SerializeField] GameObject uiSkillInfo;
    [SerializeField] float showHideTime = 0.2f;
    [SerializeField] LeanTweenType easeType;

    Skills skillScript;
    Button upgradeButton;
    SkillPanelButton[] skillbuttons;
    SkillPanelButton activeSkillButton;
    Wallet wallet;

    private void Start()
    {
        skillScript = FindObjectOfType<Skills>();
        skillbuttons = FindObjectsOfType<SkillPanelButton>();
        wallet = FindObjectOfType<Wallet>();
        upgradeButton = uiSkillInfo.GetComponentInChildren<Button>();

        LoadTextsToButtons();
        HideSkillInfo();
    }

    void LoadTextsToButtons()
    {
        Text txt;
        foreach (SkillPanelButton button in skillbuttons)
        {
            txt = button.GetComponentInChildren<Text>();
            txt.text = button.GetTitle() + ": " + skillScript.GetSkillLevel(button.skill).ToString();
        }
    }


    public void ShowSkillInfo(SkillPanelButton item)
    {
        activeSkillButton = item;
        if (!uiSkillInfo.activeInHierarchy)
        {
            LeanTween.scale(uiSkillInfo.GetComponent<RectTransform>(), Vector2.one, showHideTime).setEase(easeType);
            uiSkillInfo.SetActive(true);
        }

        UpdateSkillInfo();

    }
    public void UpdateSkillInfo()
    {
        int lvl = skillScript.GetSkillLevel(activeSkillButton.skill);
        CharacterSkillTable table = skillScript.GetSkillTable();
        int maxLvl = table.GetMaxLevel(activeSkillButton.skill);
        Text[] texts = uiSkillInfo.GetComponentsInChildren<Text>();
        int money = wallet.GetMoney();

        texts[0].text = activeSkillButton.GetTitle();
        texts[1].text = activeSkillButton.GetDescription();
        texts[2].text = "Now:  " + table.GetSkillStatValue(activeSkillButton.skill, lvl).ToString();
        int nextCost;
        if (lvl < maxLvl-1)
        {
            texts[3].text = "Next:  " + table.GetSkillStatValue(activeSkillButton.skill, lvl + 1).ToString();
            nextCost = table.GetSkillStatPrice(activeSkillButton.skill, lvl + 1);
            texts[4].text = "Upgrade Cost: " + nextCost.ToString();
        }
        else
        {
            texts[3].text = "!!Skill Maxed!!";
            nextCost = 1000000000;
            texts[4].text = "";
        }

        upgradeButton.interactable = (money >= nextCost && lvl < maxLvl-1) ? true : false;
        texts[4].color = (money >= nextCost) ? Color.yellow : Color.red;
    }

    public void HideSkillInfo()
    {
        LeanTween.scale(uiSkillInfo.GetComponent<RectTransform>(), Vector2.zero, showHideTime).setEase(easeType);
        uiSkillInfo.SetActive(false);
    }

    public void UpgradeSkillCaller()
    {
        int lvl = skillScript.GetSkillLevel(activeSkillButton.skill);
        CharacterSkillTable table = skillScript.GetSkillTable();
        int nextCost = table.GetSkillStatPrice(activeSkillButton.skill, lvl + 1);

        skillScript.UpgradeSkill(activeSkillButton.skill);
        wallet.SpendMoney(nextCost);
        
        UpdateSkillInfo();
        Invoke("LoadTextsToButtons",0.1f);
    }
}

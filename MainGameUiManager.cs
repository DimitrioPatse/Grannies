using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUiManager : MonoBehaviour
{
    [Header("Lean Tween Settings")]
    [SerializeField] float showHideTime = 1f;
    [SerializeField] LeanTweenType easeType;

    [Header("Expirience Display Settings")]
    [SerializeField] GameObject expiriencePanel;
    [SerializeField] RectTransform barForeground = null;
    Expirience exp;

    [Header("Abillity Selector Display Settings")]
    [SerializeField] GameObject abillitySelectorPanel;
    AbillitySelector abs;

    [Header("Death Display Settings")]
    [SerializeField] GameObject deathPanel;

    Game_Master gm;
    GameObject player;
    Joystick joystick;

    private void Awake()
    {
        gm = FindObjectOfType<Game_Master>();
        player = GameObject.FindGameObjectWithTag("Player");
        exp = player.GetComponent<Expirience>();
    }
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        UpdateExpirienceBar();
        abs = abillitySelectorPanel.GetComponent<AbillitySelector>();
    }



    #region Ui Expirience functions

    /// <summary>
    /// Updates Expirirence Ui Bar
    /// </summary>
    void UpdateExpirienceBar()
    {
        TextMeshProUGUI text = expiriencePanel.GetComponentInChildren<TextMeshProUGUI>();
        if (exp.GetLevel() < exp.GetMaxLevel())
        {
            barForeground.localScale = new Vector3(exp.GetPercentageForLevelup() / 100, barForeground.localScale.y, barForeground.localScale.z);
            text.text = exp.GetLevel().ToString();
        }
        else
        {
            barForeground.localScale = Vector3.one;
            text.text = exp.GetMaxLevel().ToString();
        }
    }
    #endregion

    #region Abillity Selector functions

    public void ShowAbillitySelector()
    {
        player.GetComponent<PlayerController>().DisableActions();
        abillitySelectorPanel.SetActive(true);
        LeanTween.moveY(abillitySelectorPanel.GetComponent<RectTransform>(), 0, showHideTime).setEase(easeType);
        abs.EnableAbillitySelector();
        joystick.enableMove = false;
    }
    /// <summary>
    /// Diadikasia apokrypsis toy Abillity Panel. Epitrepei tis kinhseis sto 
    /// Player
    /// </summary>
    public void HideAbillitySelector()
    {
        LeanTween.moveY(abillitySelectorPanel.GetComponent<RectTransform>(), 2000, showHideTime).setEase(easeType);
        abs.DisableAbillitySelector();
        joystick.enableMove = true;
        player.GetComponent<PlayerController>().EnableActions();
        Invoke("DisableAbillityPanel", showHideTime);
    }
    /// <summary>
    /// Kanei Disable to panel kai epitrepei sto Expirience 
    /// tou Player na synexisei to AddTempXpToExpirience()
    /// </summary>
    void DisableAbillityPanel()
    {
        abillitySelectorPanel.SetActive(false);
        exp.mustTakeAbillity = false;
    }

    #endregion

    #region Death Panel functions
    /// <summary>
    /// Enables Death Panel 
    /// </summary>
    void EnableDeathPanel()
    {
        if (!deathPanel.activeInHierarchy)
        {
            deathPanel.SetActive(true);
            LeanTween.moveY(deathPanel.GetComponent<RectTransform>(), 0, showHideTime).setEase(easeType);
        }
    }

    #endregion
    private void OnEnable()
    {
        exp.OnExperienceGained += UpdateExpirienceBar;
        gm.onPlayerDeath += EnableDeathPanel;
    }
    private void OnDisable()
    {
        exp.OnExperienceGained -= UpdateExpirienceBar;
        gm.onPlayerDeath -= EnableDeathPanel;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expirience : MonoBehaviour, ISaveable
{
    [SerializeField] float expiriencePoints = 0;
    [SerializeField] Progression levelXpProgression;
    [SerializeField] GameObject levelUpParticle = null;

    bool freshStart;
    List<float> tempXP = new List<float>();
    public bool mustTakeAbillity = false;
    int currentLevel;
    float xpMultiplier = 1;
    Game_Master gm;
    MainGameUiManager uiManager;


    // public delegate void ExpirienceGainedDelecate();
    // public event ExpirienceGainedDelecate onExpirienceGained;
    //
    // H C# to kanei kai etsi. To Action epistrefei void..
    public event Action OnExperienceGained;

    void Start()
    {
        uiManager = FindObjectOfType<MainGameUiManager>();
        UpdateLevel();
        OnExperienceGained(); // Gia na kanei update to UI
        //StartCoroutine(HeadStart());
        SetXpMultiplier();
    }


    //Den douleuei to head start.. Tsekare th coroutine apo katw

    /// <summary>
    /// Sets the xpMultiplier by adding the percentage from skills and from abilities.
    /// </summary>
    void SetXpMultiplier()
    {
        float fromSkills = GetComponentInChildren<Skills>().GetSkillValue(Skill.ExpiriencePlus) / 100;
        float fromAbilities = GetComponent<AbillitySystem>().expPlus / 100;
        xpMultiplier = 1 + fromAbilities + fromSkills;
    }

    /// <summary>
    /// Dinei ta abilities sthn arxh ths pistas
    /// </summary>
    IEnumerator HeadStart()
    {
        AbillitySelector abs = FindObjectOfType<AbillitySelector>(true);
        print("Headstarted = " + abs.headStarted);
        if (abs.headStarted) yield break;
        yield return new WaitForSeconds(2);

        int x = GetComponentInChildren<Skills>().GetSkillLevel(Skill.AbillityAtStart);
        print("Exeis " + x + "adilities na pareis sthn arxh");
        //Gia ka8e lvl tou skill
        for (int i = 0; i < x; i++)
        {
            uiManager.ShowAbillitySelector();
            yield return new WaitForSeconds(0.3f);
        }
        abs.headStarted = true;
    }

    void UpdateLevel()
    {
        int newLevel = CalculateLevel();

        if (newLevel > currentLevel)
        {
            currentLevel= newLevel;
            LevelUp();
            freshStart = true;  
        }
    }

    void LevelUp()
    {
        if (freshStart)
        {
            mustTakeAbillity = true;//Voh8aei otan einai na pareis 2 Abillities mazi
            uiManager.ShowAbillitySelector();
            Instantiate(levelUpParticle, transform);
        }
    }


    public void GainExperience(float experience)
    {
        tempXP.Add(experience * xpMultiplier); 
    }
    /// <summary>
    /// Caller ths AddTempXpToExpirience() gia na klh8ei apo ton 
    /// GM sto onLevelComplete
    /// </summary>
    void CoroutineCaller()
    {
        StartCoroutine(AddTempXpToExpirience());
    }

    /// <summary>
    /// Coroutine pou kanei load to neo xp ana kill sto telos 
    /// wste na ta fortwnei stadiaka kai na pairnei tyxwn 2 abillities
    /// an parei 2 level.
    /// </summary>
    public IEnumerator AddTempXpToExpirience()
    {
        freshStart = true;
        for (int i = tempXP.Count-1; i > 0; i--)
        {
            expiriencePoints += tempXP[i];
            UpdateLevel();
            OnExperienceGained();
            yield return new WaitUntil(() => !mustTakeAbillity);
        }
        tempXP.Clear();
    }
    
    /// <summary>
    /// Returns inGame currentLevel
    /// </summary>
    public int GetLevel(){ return currentLevel; }

    /// <summary>
    /// Returns current inGame xp 
    /// </summary>
    public float GetPoints(){ return expiriencePoints; }

    public int GetMaxLevel()
    {
        return levelXpProgression.GetLevels(Stat.INGAME_ExpirienceToLevelUp, CharacterClass.Player);
    }
    
    /// <summary>
    /// Returns the % needed for level up
    /// </summary>
    public float GetPercentageForLevelup()
    {
        if (currentLevel == 0)
        {
            return 100 * (expiriencePoints / levelXpProgression.GetStat(Stat.INGAME_ExpirienceToLevelUp, CharacterClass.Player, currentLevel));
        }
        float a = levelXpProgression.GetStat(Stat.INGAME_ExpirienceToLevelUp, CharacterClass.Player, currentLevel - 1);
        return 100 * ((expiriencePoints - a) / (levelXpProgression.GetStat(Stat.INGAME_ExpirienceToLevelUp, CharacterClass.Player, currentLevel)- a));
    }

    int CalculateLevel()
    {
        //To progression 
        float currentXp = GetPoints();
        int penultimateLevel = levelXpProgression.GetLevels(Stat.INGAME_ExpirienceToLevelUp, CharacterClass.Player);

        for (int level = 0; level < penultimateLevel; level++)
        {
            float XpToLevelUp = levelXpProgression.GetStat(Stat.INGAME_ExpirienceToLevelUp, CharacterClass.Player, level);
            if (XpToLevelUp > currentXp)
            {
                return level;
            }
        }
        return penultimateLevel + 1;
    }


    public object CaptureState()
    {
        return expiriencePoints;
    }

    public void RestoreState(object state)
    {
        expiriencePoints = (float)state;
    }
    void OnEnable()
    {
        gm = FindObjectOfType<Game_Master>();
        gm.onLevelComplete += CoroutineCaller;
    }
    void OnDisable()
    {
        gm.onLevelComplete -= CoroutineCaller;
    }
}

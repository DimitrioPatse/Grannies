using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour, ISaveable, IModifierProvider
{
    CharacterSkillTable skillTable;

    int strength_Lvl = 0;
    int health_Lvl = 0;
    int recoveryOnLvlUp_Lvl = 0;
    int recoveryOnKill_Lvl = 0;
    int equipStatPlus_Lvl = 0;
    int afkReward_Lvl = 0;
    int extraAbillity_Lvl = 0;
    int expiriencePlus_Lvl = 0;

    float strength_V;
    float health_V;
    float recoveryOnLvlUp_V;
    float recoveryOnKill_V;
    float equipStatPlus_V;
    float afkReward_V;
    float extraAbillity_V;
    float expiriencePlus_V;

    void Awake()
    {
        skillTable = Resources.Load<CharacterSkillTable>("PlayerSkillTable");
        if (skillTable == null) Debug.LogError("Skill table not found");
        LoadSkillValues();
    }


    void LoadSkillValues()
    {
        strength_V = skillTable.GetSkillStatValue(Skill.Strength, strength_Lvl);
        health_V = skillTable.GetSkillStatValue(Skill.Health, health_Lvl);
        recoveryOnLvlUp_V = skillTable.GetSkillStatValue(Skill.RecoveryOnLevel, recoveryOnLvlUp_Lvl);
        recoveryOnKill_V = skillTable.GetSkillStatValue(Skill.RecoveryOnKill, recoveryOnKill_Lvl);
        equipStatPlus_V = skillTable.GetSkillStatValue(Skill.EquipStatPlus, equipStatPlus_Lvl);
        afkReward_V = skillTable.GetSkillStatValue(Skill.AfkReward, afkReward_Lvl);
        extraAbillity_V = skillTable.GetSkillStatValue(Skill.AbillityAtStart, extraAbillity_Lvl);
        expiriencePlus_V = skillTable.GetSkillStatValue(Skill.ExpiriencePlus, expiriencePlus_Lvl);
    }

    public int GetSkillLevel(Skill skill)
    {
        switch (skill)
        {
            case Skill.Strength:
                return strength_Lvl;
            case Skill.Health:
                return health_Lvl;
            case Skill.RecoveryOnLevel:
                return recoveryOnLvlUp_Lvl;
            case Skill.RecoveryOnKill:
                return recoveryOnKill_Lvl;
            case Skill.EquipStatPlus:
                return equipStatPlus_Lvl;
            case Skill.AfkReward:
                return afkReward_Lvl;
            case Skill.ExpiriencePlus:
                return expiriencePlus_Lvl;
            case Skill.AbillityAtStart:
                return extraAbillity_Lvl;
            default:
                return 0;
        }
    }
    public float GetSkillValue(Skill skill)
    {
        switch (skill)
        {
            case Skill.Strength:
                return strength_V;
            case Skill.Health:
                return health_V;
            case Skill.RecoveryOnLevel:
                return recoveryOnLvlUp_V;
            case Skill.RecoveryOnKill:
                return recoveryOnKill_V;
            case Skill.EquipStatPlus:
                return equipStatPlus_V;
            case Skill.AfkReward:
                return afkReward_V;
            case Skill.ExpiriencePlus:
                return expiriencePlus_V;
            case Skill.AbillityAtStart:
                return extraAbillity_V;
            default:
                return 0;
        }
    }

    public CharacterSkillTable GetSkillTable() 
    {
        return skillTable;
    }

    public void UpgradeSkill(Skill skill)
    {
        switch (skill)
        {
            case Skill.Strength:
                strength_Lvl++;
                strength_V = skillTable.GetSkillStatValue(Skill.Strength, strength_Lvl);
                break;
            case Skill.Health:
                health_Lvl++;
                health_V = skillTable.GetSkillStatValue(Skill.Health, health_Lvl);
                break;
            case Skill.RecoveryOnLevel:
                recoveryOnLvlUp_Lvl++;
                recoveryOnLvlUp_V = skillTable.GetSkillStatValue(Skill.RecoveryOnLevel, recoveryOnLvlUp_Lvl);
                break;
            case Skill.RecoveryOnKill:
                recoveryOnKill_Lvl++;
                recoveryOnKill_V = skillTable.GetSkillStatValue(Skill.RecoveryOnKill, recoveryOnKill_Lvl);
                break;
            case Skill.EquipStatPlus:
                equipStatPlus_Lvl++;
                equipStatPlus_V = skillTable.GetSkillStatValue(Skill.EquipStatPlus, equipStatPlus_Lvl);
                break;
            case Skill.AfkReward:
                afkReward_Lvl++;
                afkReward_V = skillTable.GetSkillStatValue(Skill.AfkReward, afkReward_Lvl);
                break;
            case Skill.ExpiriencePlus:
                expiriencePlus_Lvl++;
                expiriencePlus_V = skillTable.GetSkillStatValue(Skill.ExpiriencePlus, expiriencePlus_Lvl);
                break;
            case Skill.AbillityAtStart:
                extraAbillity_Lvl++;
                extraAbillity_V = skillTable.GetSkillStatValue(Skill.AbillityAtStart, extraAbillity_Lvl);
                break;
            default:
                break;
        }

    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
        switch (stat)
        {
            case Stat.Health:
                yield return health_V;
                break;
            case Stat.Damage:
                yield return strength_V;
                break;
            default:
                break;
        }
    }

    public IEnumerable<float> GetPercentageModifiers(Stat stat)
    {
        switch (stat)
        {
            case Stat.ExpirienceReward:
                yield return expiriencePlus_V;
                break;
            default:
                break;
        }
    }

    public object CaptureState()
    {
        SkillSetup skilldata = new SkillSetup(strength_Lvl, health_Lvl, recoveryOnLvlUp_Lvl, recoveryOnKill_Lvl, equipStatPlus_Lvl, afkReward_Lvl, extraAbillity_Lvl, expiriencePlus_Lvl);
        return skilldata;
    }
    public void RestoreState(object state)
    {
        SkillSetup skilldata = (SkillSetup)state;
        strength_Lvl = skilldata.strength;
        health_Lvl = skilldata.health;
        recoveryOnLvlUp_Lvl = skilldata.recoveryOnLvlUp;
        recoveryOnKill_Lvl = skilldata.recoveryOnKill;
        equipStatPlus_Lvl = skilldata.equipStatPlus;
        afkReward_Lvl = skilldata.afkReward;
        extraAbillity_Lvl = skilldata.extraAbillity;
        expiriencePlus_Lvl = skilldata.expiriencePlus;
        LoadSkillValues();
    }
}



[Serializable]
public struct SkillSetup
{
    public int strength;
    public int health;
    public int recoveryOnLvlUp;
    public int recoveryOnKill;
    public int equipStatPlus;
    public int afkReward;
    public int extraAbillity;
    public int expiriencePlus;

    public SkillSetup(int new_strength, int new_health, int new_recoveryOnLvlUp,
        int new_recoveryOnKill, int new_equipStatPlus, int new_afkReward, int new_extraAbillity, int new_expiriencePlus)
    {
        strength = new_strength;
        health = new_health;
        recoveryOnLvlUp = new_recoveryOnLvlUp;
        recoveryOnKill = new_recoveryOnKill;
        equipStatPlus = new_equipStatPlus;
        afkReward = new_afkReward;
        extraAbillity = new_extraAbillity;
        expiriencePlus = new_expiriencePlus;
    }
}
public enum Skill
{
    Strength,
    Health,
    RecoveryOnLevel,
    RecoveryOnKill,
    EquipStatPlus,
    AfkReward,
    ExpiriencePlus,
    AbillityAtStart
}
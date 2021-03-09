using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbillitySystem : MonoBehaviour,ISaveable
{
    #region Setters-Getters 
    //Shooting
    public int extraFront { get; private set; }
    public int extraSides { get; private set; }
    public int extraAngled { get; private set; }
    public int extraRear { get; private set; }
    public int extraAngledRear { get; private set; }
    public int extraShot { get; private set; }
    public bool followTarget { get; private set; }
    public bool chainHit { get; private set; }
    public bool throughTarget { get; private set; }
    public bool wallBounce { get; private set; }

    //Shooting Stats
    public float attackSpeed { get; private set; }
    public float damagePlus { get; private set; }
    public float criticalRatePlus { get; private set; }
    public float criticalDamagePlus { get; private set; }
    public bool lowHpExtraDamage { get; private set; }
    public bool lowHpExtraAttackSpeed { get; private set; }
    public bool lowHpExtraCriticalRate { get; private set; }
    public bool lowHpCriticalDamage { get; private set; }
    public bool chargedShot { get; private set; }
    public bool poisonEffect { get; private set; }
    public bool flameEffect { get; private set; }
    public bool iceEffect { get; private set; }
    public bool electroEffect { get; private set; }

    //Health
    public float healPlus { get; private set; }
    public int hpPlus { get; private set; }
    public int extraLife { get; private set; }
    public float lifeSteal { get; private set; }
    public bool energyShield { get; private set; }

    //Fields
    public bool flameField { get; private set; }
    public bool iceField { get; private set; }
    public bool electroField { get; private set; }
    public bool sawField { get; private set; }

    //Attributes
    public float expPlus { get; private set; }
    public bool dwarf { get; private set; }
    public bool giant { get; private set; }
    public bool fly { get; private set; }
    public bool ghost { get; private set; }
    public float extraMoveSpeed { get; private set; }
    public bool slowEnemyProjectiles { get; private set; }
    public float evation { get; private set; }
    public bool fastPlayerProjectiles { get; private set; }

    #endregion

    List<AbillityClass> abillitiesOwned = new List<AbillityClass>();
    List<GameObject> gameObjectCompanions = new List<GameObject>();
    AbillityTable abillityTable;
    AbilityFieldSystem abFieldSys;
    public void Awake()
    {
        abillityTable = Resources.Load<AbillityTable>("AbillityTable");
        if(abillityTable == null) Debug.LogError("Abillity System couldn't find abillity table");

        abFieldSys = GetComponentInChildren<AbilityFieldSystem>();
        if (abFieldSys == null) Debug.LogWarning("No AbilityFieldSystem found");
    }


    public void AddAbillity(AbillityClass abillity)
    {
        abillitiesOwned.Add(abillity);

        switch (abillity)
        {
            case AbillityClass.ExtraFront:
                extraFront++;
                break;
            case AbillityClass.ExtraSides:
                extraSides++;
                break;
            case AbillityClass.ExtraAngled:
                extraAngled++;
                break;
            case AbillityClass.ExtraRear:
                extraRear++;
                break;
            case AbillityClass.ExtraRearAngled:
                extraAngledRear++;
                break;
            case AbillityClass.Extrashot:
                extraShot++;
                break;
            case AbillityClass.FollowTarget:
                followTarget = true;
                break;
            case AbillityClass.ChainHit:
                chainHit = true;
                break;
            case AbillityClass.ThroughTarget:
                throughTarget = true;
                break;
            case AbillityClass.WallBounce:
                wallBounce = true;
                break;
            case AbillityClass.AttackSpeed:
                attackSpeed += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.DamagePlus:
                damagePlus += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.CriticalRatePlus:
                criticalRatePlus += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.CriticalDamagePlus:
                criticalDamagePlus += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.LowHpExtraDamage:
                lowHpExtraDamage = true;
                break;
            case AbillityClass.LowHpExtraAttackSpeed:
                lowHpExtraAttackSpeed = true;
                break;
            case AbillityClass.LowHpExtraCriticalRate:
                lowHpExtraCriticalRate = true;
                break;
            case AbillityClass.LowHpCriticalDamage:
                lowHpCriticalDamage = true;
                break;
            case AbillityClass.ChargedShot:
                chargedShot = true;
                break;
            case AbillityClass.PoisonEffect:
                poisonEffect = true;
                break;
            case AbillityClass.FlameEffect:
                flameEffect = true;
                break;
            case AbillityClass.IceEffect:
                iceEffect = true;
                break;
            case AbillityClass.ElectroEffect:
                electroEffect = true;
                break;
            case AbillityClass.Heal:
                Health health = GetComponent<Health>();
                float hpPercent = abillityTable.GetAbillityValue(abillity) + healPlus;
                health.HealPercent(hpPercent);
                FindObjectOfType<HealthDisplay>().UpdateBarValues();
                break;
            case AbillityClass.HealPlus:
                healPlus += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.HpPlus:
                Health health2 = GetComponent<Health>();
                int hp= (int)GetComponentInChildren<BaseStats>().GetStat(Stat.Health);
                int hpToGet = (int)(hp * (1 + (abillityTable.GetAbillityValue(abillity) / 100)));
                health2.SetMaxHp(hpToGet);
                health2.Heal(hpToGet);
                FindObjectOfType<HealthDisplay>().UpdateBarValues();
                break;
            case AbillityClass.ExtraLife:
                Health health3 = GetComponent<Health>();
                health3.SetExtraLives(1);
                break;
            case AbillityClass.LifeSteal:
                lifeSteal += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.EnergyShield:
                energyShield = true;
                break;
            case AbillityClass.FireField:
                flameField = true;
                GetComponentInChildren<AbilityFieldSystem>().SetBuffsBools(flameField, iceField, electroField);
                break;
            case AbillityClass.IceField:
                iceField = true;
                GetComponentInChildren<AbilityFieldSystem>().SetBuffsBools(flameField, iceField, electroField);
                break;
            case AbillityClass.ElectroField:
                electroField = true;
                GetComponentInChildren<AbilityFieldSystem>().SetBuffsBools(flameField, iceField, electroField);
                break;
            case AbillityClass.SawField:
                sawField = true;
                break;
            case AbillityClass.ExpPlus:
                expPlus += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.Dwarf:
                dwarf = true;
                SetSizeOfPlayer();
                break;
            case AbillityClass.Giant:
                giant = true;
                SetSizeOfPlayer();
                break;
            case AbillityClass.Fly:
                fly = true;
                break;
            case AbillityClass.Ghost:
                ghost = true;
                break;
            case AbillityClass.ExtraMoveSpeed:
                extraMoveSpeed += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.SlowEnemyProjectiles:
                slowEnemyProjectiles = true;
                break;
            case AbillityClass.Evation:
                evation += abillityTable.GetAbillityValue(abillity);
                break;
            case AbillityClass.FastPlayerProjectiles:
                fastPlayerProjectiles = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Kanei spawn compnanions. Dn xreisimopoieitai akoma. Einai kai katw sto RestoreState()
    /// </summary>
    void SpawnAbillityCreeps()
    {
        foreach (GameObject obj in gameObjectCompanions)
        {
            Instantiate(obj, transform.position + Vector3.up, Quaternion.identity, transform.parent);
        }
    }

    void SetSizeOfPlayer()
    {
        Transform pl = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (dwarf && giant)
        {
            pl.localScale = Vector3.one;
        }
        else if (dwarf && !giant)
        {
            pl.localScale = Vector3.one/2;
        }
        else if(!dwarf && giant)
        {
            pl.localScale = Vector3.one * 2;
        }
        else
        {
            pl.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Returns an ability's value
    /// </summary>
    public float GetAbilityFloatValue(AbillityClass ability)
    {
        return (float)abillityTable.GetAbillityValue(ability);
    }
    public object CaptureState()
    {
        List<float> abillityData = new List<float> {extraFront, extraSides, extraAngled, extraRear, extraAngledRear, extraShot, followTarget?1:0, chainHit?1:0,
            throughTarget?1:0, wallBounce?1:0, attackSpeed, damagePlus, criticalRatePlus, criticalDamagePlus, lowHpExtraDamage?1:0, lowHpExtraAttackSpeed?1:0,
                lowHpExtraCriticalRate?1:0, lowHpCriticalDamage?1:0, chargedShot?1:0, poisonEffect?1:0, flameEffect?1:0, iceEffect?1:0,
                electroEffect?1:0, healPlus, hpPlus, extraLife, lifeSteal, energyShield?1:0, flameField?1:0, iceField?1:0, electroField?1:0,
                    sawField?1:0, expPlus, dwarf?1:0, giant?1:0, fly?1:0, ghost?1:0, extraMoveSpeed, slowEnemyProjectiles?1:0, evation, fastPlayerProjectiles?1:0 };
        return abillityData;
    }

    public void RestoreState(object state)
    {
        List<float> abilityData = (List<float>)state;

        extraFront = (int)abilityData[0];
        extraSides = (int)abilityData[1];
        extraAngled = (int)abilityData[2];
        extraRear = (int)abilityData[3];
        extraAngledRear = (int)abilityData[4];
        extraShot = (int)abilityData[5];
        followTarget = abilityData[6] > 0;
        chainHit = abilityData[7] > 0;
        throughTarget = abilityData[8] > 0;
        wallBounce = abilityData[9] > 0;
        attackSpeed = abilityData[10];
        damagePlus = abilityData[11];
        criticalRatePlus = abilityData[12];
        criticalDamagePlus = abilityData[13];
        lowHpExtraDamage = abilityData[14] > 0;
        lowHpExtraAttackSpeed = abilityData[15] > 0;
        lowHpExtraCriticalRate = abilityData[16] > 0;
        lowHpCriticalDamage = abilityData[17] > 0;
        chargedShot = abilityData[18] > 0;
        poisonEffect = abilityData[19] > 0;
        flameEffect = abilityData[20] > 0;
        iceEffect = abilityData[21] > 0;
        electroEffect = abilityData[22] > 0;
        healPlus = abilityData[23];
        hpPlus = (int)abilityData[24];
        extraLife = (int)abilityData[25];
        lifeSteal = abilityData[26];
        energyShield = abilityData[27] > 0;
        flameField = abilityData[28] > 0;
        iceField = abilityData[29] > 0;
        electroField = abilityData[30] > 0;
        sawField = abilityData[31] > 0;
        expPlus = abilityData[32];
        dwarf = abilityData[33] > 0;
        giant = abilityData[34] > 0;
        fly = abilityData[35] > 0;
        ghost = abilityData[36] > 0;
        extraMoveSpeed = abilityData[37];
        slowEnemyProjectiles = abilityData[38] > 0;
        evation = abilityData[39];
        fastPlayerProjectiles = abilityData[40] > 0;

        //Field Buffs
        GetComponentInChildren<AbilityFieldSystem>().SetBuffsBools(flameField, iceField, electroField);
        //Giant or Dwarf
        SetSizeOfPlayer();


       // SpawnAbillityCreeps(); //Ειναι για τυχόν companions
    }
}

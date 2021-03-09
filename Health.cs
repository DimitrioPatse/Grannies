using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, ISaveable
{
    [SerializeField] TakeDamageEvent takeDamage;
    [SerializeField] UnityEvent onDie;
    [SerializeField] ParticleSystem deathParticle;

    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float> { }

    LazyValue<float> healthPoints; //episodeio 153 
    float maxHp;
    float lives = 0;
    Expirience exp;
    BaseStats stats;
    Game_Master gm;
    BuffStats myBuffs = new BuffStats();
    HealthDisplay healthDisplay;
    bool isDead;
    float hpToRestorePerKill;
    public bool invincible;
    void Awake()
    {
        stats = GetComponentInChildren<BaseStats>();
        healthPoints = new LazyValue<float>(GetInitilHealth);
        gm = FindObjectOfType<Game_Master>();
        exp = gameObject.CompareTag("Player") ? GetComponent<Expirience>() : FindObjectOfType<Expirience>();//Mono oi Enemies to xreiazontai
        healthDisplay = gameObject.CompareTag("Player") ? GetComponentInChildren<HealthDisplay>() : null;
        hpToRestorePerKill = gameObject.CompareTag("Player") ? GetComponentInChildren<Skills>().GetSkillValue(Skill.RecoveryOnKill) : 0;
        maxHp = GetInitilHealth();
    }

    float GetInitilHealth()
    {
        return stats.GetStat(Stat.Health);
    }

    void Start()
    {
     //healthPoints.ForceInit();
    }

    public void TakeDamage(float damage)
    {
        if (invincible) return;
        healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); // Mathf gia na mhn phgainei katw apo to 0 h zvh
        takeDamage.Invoke(damage);
        if (healthPoints.value == 0)
        {
            if(lives > 0)
            {
                healthPoints.value = maxHp;
                lives--;
                healthDisplay.UpdateBarValues();
            }
            else
            {
                onDie.Invoke();
                Die();
            }
        }
    }
    public void BeInvincible()
    {
        invincible = true;
    }
    public float GetHP()
    {
        return healthPoints.value;
    }
    public void SetMaxHp(float value)
    {
        maxHp = value;
    }
    public void SetExtraLives(float value)
    {
        lives += value;
    }
    public float GetMaxHP()
    {
        return maxHp;
    }
    public float GetHealthPersentage()
    {
        float f = maxHp; 
        return 100 * (healthPoints.value / f);
    }

    public bool IsDead()
    {
        return isDead;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Animator anim = GetComponentInChildren<Animator>();

        anim.SetTrigger("deathTrigger");

        if (gameObject.CompareTag("Player"))
        {
            gm.PlayerDeath();
            return;
        }
        gm.RemoveFromEnemyList(this.transform);
        AwardExpirience();
        Instantiate(deathParticle,transform.position,Quaternion.identity, transform);
        Destroy(gameObject,deathParticle.main.duration);
    }

    void AwardExpirience()
    {
        if (exp == null || gameObject.CompareTag("Player")) return;
        exp.GainExperience(stats.GetStat(Stat.ExpirienceReward));
        exp.gameObject.GetComponent<Health>().KillHeal();
    }

    public void HealPercent(float regenerationPercentage)
    {
        if (isDead) return;

        float regenHP = healthPoints.value + (GetMaxHP() * (regenerationPercentage / 100));
        healthPoints.value = Mathf.Max(healthPoints.value, regenHP);
        healthDisplay.UpdateBarValues();

    }

    /// <summary>
    /// Restores hp 
    /// </summary>
    public void KillHeal()
    {
        Heal(hpToRestorePerKill);
    }

    /// <summary>
    /// Restores hp. If player then it first multiplies the hpToRestore with
    /// the healPlus ability and restores the new value
    /// </summary>
    public void Heal(float hpToRestore)
    {
        //Sets extra heal % from ability
        if (gameObject.CompareTag("Player"))
        {
            float extraHealFromAbility;
            AbillitySystem abs = GetComponent<AbillitySystem>();
            extraHealFromAbility = 1 + (abs.healPlus / 100);
            hpToRestore *= extraHealFromAbility;
        }
        //Heal
        healthPoints.value = Mathf.Min(healthPoints.value + hpToRestore, GetMaxHP());
        if (healthPoints.value > maxHp)
        {
            healthPoints.value = maxHp;
        }
        //Update UI
        healthDisplay.UpdateBarValues();
    }

    #region Buffs
    public void SetBuffs(Buffs buff)
    {
        switch (buff)
        {
            case Buffs.Burned:
                if(myBuffs.burned) StopCoroutine(myBuffs.Burn);
                myBuffs.Burn = StartCoroutine(DamageOverTime(stats.GetStat(Stat.Health) / 100 * 8, myBuffs.burnFrequency, myBuffs.burnDuration));
                myBuffs.burned = true; 
                break;

            case Buffs.Frozen:
                if(myBuffs.frozen) StopCoroutine(myBuffs.Freeze);
                myBuffs.Freeze = StartCoroutine(DamageOverTime(stats.GetStat(Stat.Health) / 100 * 5, myBuffs.frozenFrequency, myBuffs.frozenDuration));
                myBuffs.frozen = true;
                break;

            case Buffs.Poisoned:
                if(myBuffs.poisoned) StopCoroutine(myBuffs.Poison);
                myBuffs.Poison = StartCoroutine(DamageOverTime(stats.GetStat(Stat.Health) / 100 * 7, myBuffs.poisonFrequency, myBuffs.poisonDuration));
                myBuffs.poisoned = true;
                break;

            default:
                break;
        }
    }

    public IEnumerator DamageOverTime(float damage, float frequencyInSeconds, float duration)
    {
        while(duration > 0f)
        {
            TakeDamage(damage);
            duration -= frequencyInSeconds;
            yield return new WaitForSeconds(frequencyInSeconds);
        }
    }
    #endregion


    public object CaptureState()
    {
        float[] hpData = new float[3];
        hpData[0] = maxHp;
        hpData[1] = healthPoints.value;
        hpData[2] = lives;
        return hpData;
    }

    public void RestoreState(object state)
    {
        float[] hpData = (float[])state;
        maxHp = (hpData[0]>maxHp)? hpData[0] : stats.GetStat(Stat.Health);//Xwris ayto vgazei bug kai to maxHp einai mono to tou BaseHealth kai glitcharei to KillHill
        healthPoints.value = hpData[1];
        lives = hpData[2];

        //Einai gia na mporei na epistrefeiw se pistes kai oi nekroi exthroi na einai nekroi 
        if (healthPoints.value <= 0)
        {
            if (this.gameObject.CompareTag("Player")) return;
            gm.RemoveFromEnemyList(this.transform);
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        gm.onLevelComplete += BeInvincible;
    }
    void OnDisable()
    {
        gm.onLevelComplete -= BeInvincible;

    }
}
[System.Serializable]
class BuffStats
{
    public Coroutine Burn;
    public bool burned;
    public float burnDuration = 5f;
    public float burnFrequency = 1f;

    public Coroutine Freeze;
    public bool frozen;
    public float frozenDuration = 5f;
    public float frozenFrequency = 1f;

    public Coroutine Poison;
    public bool poisoned;
    public float poisonDuration = 5f;
    public float poisonFrequency = 1f;    
}


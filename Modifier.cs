using System;

public enum ModifierType
{
    Damage,
    Health,
    DamagePercentPlus,
    HealthPercentPlus,
    AttackSpeed,
    ProjectileSpeed,
    CriticalRate,
    CriticalDamage,
    Evasion,
    HealOnLvlUp,
    HealOnKill,
    HealEffectPercentPlus,
    GainXpPercentPlus,
    DmgFromBossPercentMinus,
    DmgToBossPercentPlus,
    FieldOfViewPercentPlus,
    GoldRewardPercentPlus,
}
[Serializable]
public class Modifier
{
    public ModifierType modifierType;
    public float Value;

    public Modifier(ModifierType type, float value)
    {
        modifierType = type;
        Value = value;
    }
}
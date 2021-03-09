using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour, ISaveable
{
    [Range(0, 99)]
    [SerializeField] int characterLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;
    [SerializeField] bool shouldUseModifiers = false;

    int totalExpirience;

    /// <summary>
    /// Returns the final calculated value of a stat
    /// </summary>
    public float GetStat(Stat stat)
    {
        return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat));
    }
    /// <summary>
    /// Returns the base value of the stat
    /// </summary>
    private float GetBaseStat(Stat stat)
    {
        return progression.GetStat(stat, characterClass, GetLevel());
    }

    /// <summary>
    /// Returns the character level
    /// </summary>
    public int GetLevel()
    {
        return characterLevel;
    }

    float GetAdditiveModifier(Stat stat)
    {
        if (!shouldUseModifiers) return 0;

        float total = 0;

        foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach (float modifier in provider.GetAdditiveModifiers(stat))
            {
                total += modifier;
            }
        }
        //print("Total additive IMdifier " + stat.ToString() + " = " + total);
        return total;
    }
    float GetPercentageModifier(Stat stat)
    {
        if (!shouldUseModifiers) return 0;

        float total = 0;

        foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach (float percentModifier in provider.GetPercentageModifiers(stat))
            {
                total += percentModifier;
            }
        }
        //print("Total percentage IMdifier " + stat.ToString() + " = " + total);
        return total / 100;
    }

    object ISaveable.CaptureState()
    {
        return characterLevel;
    }

    void ISaveable.RestoreState(object state)
    {
        characterLevel = (int)state;
    }
}

using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "CharacterSkillTable", menuName = "Stats/New Skill Table", order = 0)]

public class CharacterSkillTable : ScriptableObject
{

    [SerializeField] CharacterSkillClass[] characterSkills = null;

    Dictionary<Skill, float[]> lookupValueTable = null;
    Dictionary<Skill, int[]> lookupPriceTable = null;

    public int GetMaxLevel(Skill skill)
    {
        return lookupValueTable[skill].Length;
    }
    public float GetSkillStatValue(Skill skill,  int level)
    {
        BuildLookupValue();

        if (lookupValueTable[skill].Length < level)
        {
            return 0;
        }

        return lookupValueTable[skill][level]; 
    }

    public int GetSkillStatPrice(Skill skill, int level)
    {
        BuildLookupPrice();

        if (lookupPriceTable[skill].Length < level)
        {
            return 0;
        }

        return lookupPriceTable[skill][level];
    }

    void BuildLookupValue()
    {
        if (lookupValueTable != null) return;

        lookupValueTable = new Dictionary<Skill, float[]>();

        foreach (CharacterSkillClass charSkill in characterSkills)
        {
            lookupValueTable[charSkill.characterSkill] = charSkill.skillValue;
        }
    }


    void BuildLookupPrice()
    {
        if (lookupPriceTable != null) return;

        lookupPriceTable = new Dictionary<Skill, int[]>();

        foreach (CharacterSkillClass charSkill in characterSkills)
        {
            lookupPriceTable[charSkill.characterSkill] = charSkill.skillCost;
        }
    }
    [System.Serializable]
    class CharacterSkillClass
    {
        public Skill characterSkill;
        public float[] skillValue;
        public int[] skillCost;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AbillityTable", menuName = "Stats/New Abillity Table", order = 6)]
public class AbillityTable : ScriptableObject
{
    [SerializeField] Abillity[] abillities = null;

    Dictionary<AbillityClass, float> lookupValueTable = null;
    Dictionary<AbillityClass, int> lookupMaxCountTable = null;

    public Dictionary<AbillityClass, int> GetMaxCntDictionary()
    {
        BuildMaxCountLookup();
        return lookupMaxCountTable;
    }

    public float GetAbillityValue(AbillityClass abillity)
    {
        BuildValueLookup();
        return lookupValueTable[abillity];
    }

    public int GetAbillityMaxCount(AbillityClass abillity)
    {
        BuildMaxCountLookup();
        return lookupMaxCountTable[abillity];
    }
    void BuildValueLookup()
    {
        if (lookupValueTable != null) return;

        lookupValueTable = new Dictionary<AbillityClass, float>();

        foreach (Abillity abillity in abillities)
        {
            lookupValueTable[abillity.abillityClass] = abillity.value;
        }
    }
    void BuildMaxCountLookup()
    {
        if (lookupMaxCountTable != null) return;

        lookupMaxCountTable = new Dictionary<AbillityClass, int>();

        foreach (Abillity abillity in abillities)
        {
            lookupMaxCountTable[abillity.abillityClass] = abillity.maxCount;
        }
    }
}

[System.Serializable]
public struct Abillity
{
    public AbillityClass abillityClass;
    public float value;
    public int maxCount;
}

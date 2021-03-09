using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Stats/New Item Table", order = 2)]
public class ItemProgression : ScriptableObject
{

    [SerializeField] ItemClass[] itemClasses = null;

    Dictionary<ItemName, Dictionary<Rarity, Dictionary<ModifierType, float[]>>> lookupItemTable = null;
    
    Dictionary<ItemName, Dictionary<Rarity, int[]>> lookupPriceTable = null;
    Dictionary<ItemName, Dictionary<Rarity, int[]>> lookupMaterialTable = null;

    public int GetMaxLevel(ItemName item, Rarity rarity)
    {
        BuildLookupPriceCost();
        return lookupPriceTable[item][rarity].Length;
    }
    public float GetItemModValue(ItemName item, Rarity rarity, ModifierType modType,  int level)
    {
        BuildLookupValue();

        if (lookupItemTable[item][rarity][modType].Length < level)
        {
            return 0;
        }

        return lookupItemTable[item][rarity][modType][level];
    }

    /// <summary>
    /// Returns a Modifier[] with the item's mods
    /// </summary>
    public Modifier[] GetModifiers(ItemName itemName, Rarity rarity, int level)
    {
        BuildLookupValue();
        List<Modifier> mods = new List<Modifier>();
        foreach (ModifierType modType in lookupItemTable[itemName][rarity].Keys)
        {
            float value = lookupItemTable[itemName][rarity][modType][level];
            Modifier mod = new Modifier(modType, value);
            mods.Add(mod);
        }
        
        return mods.ToArray();
    }

    /// <summary>
    /// Returns coin cost for lvl upgrade
    /// </summary>
    public int GetItemPriceCost(ItemName itemName, Rarity rarity, int level)
    {
        BuildLookupPriceCost();

        if (lookupPriceTable[itemName][rarity].Length < level)
        {
            return 0;
        }

        return lookupPriceTable[itemName][rarity][level];
    }

    /// <summary>
    /// Returns material cost for lvl upgrade
    /// </summary>
    public int GetItemMaterialCost(ItemName itemName, Rarity rarity, int level)
    {
        BuildLookupMaterialCost();

        if (lookupMaterialTable[itemName][rarity].Length < level)
        {
            return 0;
        }

        return lookupMaterialTable[itemName][rarity][level];
    }


    void BuildLookupValue()
    {
        if (lookupItemTable != null) return;

        lookupItemTable = new Dictionary<ItemName, Dictionary<Rarity, Dictionary<ModifierType, float[]>>>();

        foreach (ItemClass itemClass in itemClasses)
        {
            var rarityLookupTable = new Dictionary<Rarity, Dictionary<ModifierType, float[]>>();

            foreach (ItemRarityClass itemRarity in itemClass.itemRarities)
            {
                var modLookupTable = new Dictionary<ModifierType, float[]>();

                foreach (ItemModifier mod in itemRarity.itemMods)
                {
                    modLookupTable[mod.itemModType] = mod.itemModValues;
                }

                rarityLookupTable[itemRarity.rarity] = modLookupTable;
            }

            lookupItemTable[itemClass.itemName] = rarityLookupTable;
        }
    }

    void BuildLookupPriceCost()
    {
        if (lookupPriceTable != null) return;

        lookupPriceTable = new Dictionary<ItemName, Dictionary<Rarity, int[]>>();

        foreach (ItemClass item in itemClasses)
        {
            var rarityLookupTable = new Dictionary<Rarity, int[]>();

            foreach (ItemRarityClass itemRarity in item.itemRarities)
            {
                rarityLookupTable[itemRarity.rarity] = itemRarity.itemNextLvlCost;
            }
                lookupPriceTable[item.itemName] = rarityLookupTable;
        }
    }
    void BuildLookupMaterialCost()
    {
        if (lookupMaterialTable != null) return;

        lookupMaterialTable = new Dictionary<ItemName, Dictionary<Rarity, int[]>>();

        foreach (ItemClass item in itemClasses)
        {
            var rarityLookupTable = new Dictionary<Rarity, int[]>();

            foreach (ItemRarityClass itemRarity in item.itemRarities)
            {
                rarityLookupTable[itemRarity.rarity] = itemRarity.itemNextLvlCost;
            }
            lookupMaterialTable[item.itemName] = rarityLookupTable;
        }
    }


    [System.Serializable]
    class ItemClass
    {
        public ItemName itemName;
        public ItemRarityClass[] itemRarities;
    }

    [System.Serializable]
    class ItemRarityClass
    {
        public Rarity rarity;
        public ItemModifier[] itemMods;
        public int[] itemNextLvlCost;
        public int[] itemNextLvlMaterialCost;
    }

    [System.Serializable]
    class ItemModifier
    {
        public ModifierType itemModType;
        public float[] itemModValues;
    }
}
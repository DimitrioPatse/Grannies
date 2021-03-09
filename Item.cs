using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/New Item", order = 0)]
public class Item : ScriptableObject,IComparable<Item>
{
    public int id;
    public ItemName itemName;
    public Rarity rarity;
    public ItemType itemType;
    public string description;
    public int level;
    public int CompareTo(Item other)
    {
        return other.id.CompareTo(id);
    }
}

[Serializable]
public class SaveableItem
{
    public ItemName itemName;
    public Rarity rarity;
    public int level;

    public SaveableItem(Item item)
    {
        itemName = item.itemName;
        rarity = item.rarity;
        level = item.level;
    }
    public SaveableItem(ItemMaterial material)
    {
        itemName = material.itemName;
        rarity = Rarity.Common;
        level = material.count;
    }

}
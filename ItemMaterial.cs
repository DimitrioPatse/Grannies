using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Material", menuName = "Items/New Material", order = 4)]
public class ItemMaterial : ScriptableObject, IComparable<ItemMaterial>
{
    public ItemName itemName;
    public ItemType itemType;
    public int count;

    public int CompareTo(ItemMaterial other)
    {
        return other.itemType.CompareTo(itemType);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour,ISaveable, IModifierProvider
{
    public List<Item> unequipedItems = new List<Item>();
    public List<Item> equipedItems = new List<Item>();
    
    ItemProgression itemProgession;

    void Awake()
    {
        itemProgession = Resources.Load<ItemProgression>("ItemTable");
        if (itemProgession == null) Debug.LogError("Item Table not found");
    }
    
    public void AddItemToStack(Item item)
    {
        unequipedItems.Add(item);
    }

    public void EquipItem(Item item)
    {
        equipedItems.Add(item);
        unequipedItems.Remove(item);
    }

    public void UnequipItem(Item item)
    {
        for (int i = 0; i < equipedItems.Count; i++)
        {
            if(equipedItems[i].itemType == item.itemType)
            {
                unequipedItems.Add(item);
                equipedItems.Remove(item);
                return;
            }
        }
    }
    public void DeleteItem(Item item)
    {
        unequipedItems.Remove(item);
    }

    public void SetupEquipedItemsInGame()
    {
        foreach (var item in equipedItems)
        {
            switch (item.itemType)
            {
                case ItemType.Googles:
                    break;
                case ItemType.Clothes:
                    break;
                case ItemType.Weapon:
                    WeaponConfig wep = Resources.Load<WeaponConfig>(item.itemName.ToString());
                    GetComponentInParent<PlayerController>().EquipWeapon(wep);
                    break;
                case ItemType.ChairShield:
                    break;
                case ItemType.Pet:
                    PetConfig pet = Resources.Load<PetConfig>(item.itemName.ToString());
                    pet.InstanciatePet();
                    break;
                case ItemType.Wheels:
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerable<float> IModifierProvider.GetAdditiveModifiers(Stat stat)
    {
        foreach (Item item in equipedItems)
        {
            Modifier[] mods = itemProgession.GetModifiers(item.itemName, item.rarity, item.level);

            foreach (Modifier mod in mods)
            {
                switch (mod.modifierType)
                {
                    case ModifierType.Damage:
                        if (stat == Stat.Damage){ yield return mod.Value; }
                        break;
                    case ModifierType.Health:
                        if (stat == Stat.Health) { yield return mod.Value; }
                        break;
                    case ModifierType.DamagePercentPlus:
                        break;
                    case ModifierType.HealthPercentPlus:
                        break;
                    case ModifierType.AttackSpeed:
                        break;
                    case ModifierType.ProjectileSpeed:
                        break;
                    case ModifierType.CriticalRate:
                        break;
                    case ModifierType.CriticalDamage:
                        break;
                    case ModifierType.Evasion:
                        break;
                    case ModifierType.HealOnLvlUp:
                        break;
                    case ModifierType.HealOnKill:
                        break;
                    case ModifierType.HealEffectPercentPlus:
                        break;
                    case ModifierType.GainXpPercentPlus:
                        break;
                    case ModifierType.DmgFromBossPercentMinus:
                        break;
                    case ModifierType.DmgToBossPercentPlus:
                        break;
                    case ModifierType.FieldOfViewPercentPlus:
                        break;
                    case ModifierType.GoldRewardPercentPlus:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat)
    {
        foreach (Item item in equipedItems)
        {
            Modifier[] mods = itemProgession.GetModifiers(item.itemName, item.rarity, item.level);

            foreach (Modifier mod in mods)
            {
                switch (mod.modifierType)
                {
                    case ModifierType.Damage:
                        break;
                    case ModifierType.Health:
                        break;
                    case ModifierType.DamagePercentPlus:
                        if (stat == Stat.Damage) { yield return mod.Value; }
                        break;
                    case ModifierType.HealthPercentPlus:
                        if (stat == Stat.Health) { yield return mod.Value; }
                        break;
                    case ModifierType.AttackSpeed:
                        break;
                    case ModifierType.ProjectileSpeed:
                        break;
                    case ModifierType.CriticalRate:
                        break;
                    case ModifierType.CriticalDamage:
                        break;
                    case ModifierType.Evasion:
                        break;
                    case ModifierType.HealOnLvlUp:
                        break;
                    case ModifierType.HealOnKill:
                        break;
                    case ModifierType.HealEffectPercentPlus:
                        break;
                    case ModifierType.GainXpPercentPlus:
                        break;
                    case ModifierType.DmgFromBossPercentMinus:
                        break;
                    case ModifierType.DmgToBossPercentPlus:
                        break;
                    case ModifierType.FieldOfViewPercentPlus:
                        break;
                    case ModifierType.GoldRewardPercentPlus:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public object CaptureState()
    {
        List<SaveableItem>[] inventoryData = new List<SaveableItem>[2];
        inventoryData[0] = new List<SaveableItem>();
        inventoryData[1] = new List<SaveableItem>();
        foreach (Item item in unequipedItems)
        {
            inventoryData[0].Add(new SaveableItem(item));
        }
        foreach (Item item in equipedItems)
        {
            inventoryData[1].Add(new SaveableItem(item) as SaveableItem);
        }
        return inventoryData;
    }

    public void RestoreState(object state)
    {
        unequipedItems.Clear();
        equipedItems.Clear();

        List<SaveableItem>[] inventoryData = (List<SaveableItem>[])state;
        foreach (SaveableItem savedItem in inventoryData[0])
        {
            Item itemTemplate = Resources.Load(savedItem.itemName.ToString(),typeof(Item)) as Item;
            if(itemTemplate == null)
            {
                Debug.LogError("No item found for item: " + savedItem.itemName.ToString());
            }
            Item item = Instantiate(itemTemplate);
            item.rarity = savedItem.rarity;
            item.level = savedItem.level;
            unequipedItems.Add(item);
        }
        foreach (SaveableItem savedItem in inventoryData[1])
        {
            Item itemTemplate = Resources.Load(savedItem.itemName.ToString(), typeof(Item)) as Item;
            Item item = Instantiate(itemTemplate);
            item.rarity = savedItem.rarity;
            item.level = savedItem.level;
            equipedItems.Add(item);
        }
    }
}

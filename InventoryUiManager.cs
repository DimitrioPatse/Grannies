using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] GameObject inventoryButtonPrefab;

    [Header ("Item Info Panel Properties")]
    [SerializeField] GameObject uiItemInfo;
    [SerializeField] Button equipButton;
    [SerializeField] Button unequipButton;
    [SerializeField] Button upgradeButton;
    [SerializeField] float showHideTime;
    [SerializeField] LeanTweenType easeType;

    [Header("Player Info Panel Properties")]
    [SerializeField] GameObject uiPlayerInfo;
    [SerializeField] Text damageModText;
    [SerializeField] Text hpModText;
    [SerializeField] Text speedModText;
    [SerializeField] Text armorModText;


    ItemProgression itemProg;
    Item currentItem;


    public GameObject currentButton { set; get; }
    InvetoryEquipedItemButton[] equipedItemButtons = new InvetoryEquipedItemButton[8];
    GameObject player;
    Inventory inventory;
    SavingWrapper saveWrapper;
    Wallet wallet;

    private void Start()
    {
        itemProg = Resources.Load<ItemProgression>("ItemTable");
        player = GameObject.FindGameObjectWithTag("Player");
        saveWrapper = FindObjectOfType<SavingWrapper>();
        inventory = player.GetComponentInChildren<Inventory>();
        equipedItemButtons = FindObjectsOfType<InvetoryEquipedItemButton>();
        wallet = FindObjectOfType<Wallet>();

        saveWrapper.Load(true); //Mallon prepei na fygei apo edw

        HideItemInfo();
        HidePlayerInfo();
        FillContainer();
        FillEquipedButtons();
    }

    Sprite LoadIcon(string name)
    {
        return Resources.Load<Sprite>(name);
    }
 
    /// <summary>
    /// Gemizei to unequiped container sto inventory me buttons me ta unequiped items
    /// </summary>
    void FillContainer()
    {
        inventory.unequipedItems.Sort();    //Taxinomish

        for (int i = 0; i < inventory.unequipedItems.Count; i++)
        {
            CreateButton(inventory.unequipedItems[i]);
        }
    }
    
    /// <summary>
    /// Dhmioyrgei button me unequiped item tou inventory 
    /// </summary>
    /// <param name="item"></param>
    void CreateButton(Item item)
    {
        GameObject button = Instantiate(inventoryButtonPrefab, inventoryContainer.transform) as GameObject;
        InvetoryItemButton buttonInfo = button.GetComponent<InvetoryItemButton>();
        buttonInfo.Item = item;
        string name = buttonInfo.Item.itemName.ToString() + " Lvl:" + buttonInfo.Item.level.ToString();
        button.GetComponent<Image>().sprite = LoadIcon(name);
        button.GetComponentInChildren<Text>().text = name;
    }

    

    /// <summary>
    /// Fortwnei ta slots twn Equiped Items
    /// </summary>
    void FillEquipedButtons()
    {
        foreach (var button in equipedItemButtons)
        {
            for (int i = 0; i < inventory.equipedItems.Count; i++)
            {
                if (inventory.equipedItems[i].itemType == button.itemType)
                {
                    button.Item = inventory.equipedItems[i];//test
                    button.GetComponent<Image>().sprite = LoadIcon(button.Item.itemName.ToString());
                    button.GetComponent<Image>().color = Color.red;
                    button.isFull = true;
                }
            }
        }
    }
   
    /// <summary>
    /// Emfanizei to ItemInfo panel me to info gia to antikeimeno poy epilexthike
    /// </summary>
    /// <param name="item"></param>
    public void ShowItemInfo(Item item, bool itemIsEquiped)
    {
        currentItem = item;

        if (!uiItemInfo.activeInHierarchy)
        {
            LeanTween.scale(uiItemInfo.GetComponent<RectTransform>(), Vector2.one, showHideTime).setEase(easeType);
            uiItemInfo.SetActive(true);
        }

        UpdateItemTexts();

        if (itemIsEquiped)
        {
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton.gameObject.SetActive(true);
            unequipButton.gameObject.SetActive(false);
        }
        CheckUpgradeButton();
    }

    /// <summary>
    /// Enables or Not the upgrade Button depending on item cost and available money in wallet
    /// </summary>
    void CheckUpgradeButton()
    {
        int maxLvl = itemProg.GetMaxLevel(currentItem.itemName,currentItem.rarity);
        int priceToUpgrade = itemProg.GetItemPriceCost(currentItem.itemName, currentItem.rarity, currentItem.level);
        bool active = priceToUpgrade <= wallet.GetMoney();
        //
        //TODO Implement material cost
        //
        if(currentItem.level < maxLvl - 1)
        {
            upgradeButton.GetComponentInChildren<Text>().text = "Upgrade Cost: " + priceToUpgrade.ToString();
            upgradeButton.interactable = active;
        }
        else
        {
            upgradeButton.GetComponentInChildren<Text>().text = "!!Maxed!!";
            upgradeButton.interactable = false;
        }

    }
    public void UpgradeItem()
    {
        currentItem.level++;
        UpdateItemTexts();
        CheckUpgradeButton();
    }

    /// <summary>
    /// Kanei update ta texts tou ItemInfo panel
    /// </summary>
    void UpdateItemTexts()
    {
        Text[] texts = uiItemInfo.GetComponentsInChildren<Text>();
        texts[0].text = currentItem.itemName.ToString() + " Lvl:" + currentItem.level.ToString();
        texts[1].text = currentItem.description;

        for (int i = 2; i < 6; i++)
        {
            texts[i].text = "";
        }
        Modifier[] mods = itemProg.GetModifiers(currentItem.itemName, currentItem.rarity, currentItem.level);
        for (int i = 2; i < mods.Length + 2; i++)
        {
            texts[i].text = mods[i - 2].modifierType.ToString() + ":  " + mods[i - 2].Value.ToString();
        }
    }
 
    /// <summary>
    /// Apokripsh tou ItemInfo panel
    /// </summary>
    public void HideItemInfo()
    {
        LeanTween.scale(uiItemInfo.GetComponent<RectTransform>(), Vector2.zero, showHideTime).setEase(easeType);
        uiItemInfo.SetActive(false);
    }

    /// <summary>
    /// Emfanizei to PlayerInfo panel me ta stat twn modifiers tou
    /// </summary>
    /// <param name="item"></param>
    public void ShowPlayerInfo()
    {
        if (!uiPlayerInfo.activeInHierarchy)
        {
            LeanTween.scale(uiPlayerInfo.GetComponent<RectTransform>(), Vector2.one, showHideTime).setEase(easeType);
            uiPlayerInfo.SetActive(true);
        }
        
        //Updates Texts
        BaseStats plStats = player.GetComponentInChildren<BaseStats>();
        damageModText.text = "Damage: " + plStats.GetStat(Stat.Damage).ToString();
        hpModText.text = "Health : " + plStats.GetStat(Stat.Health).ToString();
    }

    /// <summary>
    /// Apokripsh tou PlayerInfo panel
    /// </summary>
    public void HidePlayerInfo()
    {
        LeanTween.scale(uiPlayerInfo.GetComponent<RectTransform>(), Vector2.zero, showHideTime).setEase(easeType);
        uiPlayerInfo.SetActive(false);
    }

    /// <summary>
    /// Kanei equip to epilegmeno item
    /// </summary>
    public void EquipItem()
    {
        InvetoryEquipedItemButton eqPlaceholder = EquipItemPlaceholder(currentItem.itemType);
        if (eqPlaceholder.isFull)
        {
            Item equiped = eqPlaceholder.Item;
            inventory.UnequipItem(equiped);
            CreateButton(equiped);
            eqPlaceholder.ClearItem();
        }
        eqPlaceholder.Item = currentItem;
        eqPlaceholder.GetComponent<Image>().sprite = LoadIcon(currentItem.itemName.ToString());
        eqPlaceholder.GetComponent<Image>().color = Color.red;
        eqPlaceholder.isFull = true;

        inventory.EquipItem(currentItem);
        HideItemInfo();
        Destroy(currentButton);
    }

    /// <summary>
    /// Returns the proper equiped button for the given ItemType
    /// </summary>
    InvetoryEquipedItemButton EquipItemPlaceholder(ItemType type)
    {
        for (int i = 0; i < equipedItemButtons.Length; i++)
        {
            if (equipedItemButtons[i].itemType == type)
            {
                return equipedItemButtons[i];
            }
        }
        Debug.Log("No equiped button placeholder found for ItemType: " + type.ToString());
        return null;
    }

    /// <summary>
    /// Kanei unequip to item. Ui UnequipButton calls it
    /// </summary>
    public void UnequipItem()
    {
        currentButton.GetComponent<InvetoryEquipedItemButton>().ClearItem();
        inventory.UnequipItem(currentItem);
        CreateButton(currentItem);
        HideItemInfo();
    }
    

  
    /// <summary>
    /// Deletes an item in inventory... TODO(na metatrapei se anakyklvsh)
    /// </summary>
    public void DeleteItem()
    {
        inventory.DeleteItem(currentItem);
        Destroy(currentButton);
        HideItemInfo();
    }

}

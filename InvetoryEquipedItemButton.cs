using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvetoryEquipedItemButton : MonoBehaviour
{
    public ItemType itemType;
    InventoryUiManager mng;
    public Item Item { set; get; }

    public bool isFull { set; get; }
    private void Start()
    {
        mng = FindObjectOfType<InventoryUiManager>();
    }

    public void ShowInfo()
    {
        if (Item == null) return;

        if (Item.id <= 0 || mng.currentButton == this.gameObject)
        {
            mng.HideItemInfo();
            mng.currentButton = null;
            return;
        }

        mng.ShowItemInfo(Item,true);
        mng.currentButton = this.gameObject;
    }
    public void ClearItem()
    {
        Item = null;
        isFull = false;
    }
}

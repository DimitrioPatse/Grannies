using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvetoryItemButton : MonoBehaviour
{
    [HideInInspector]
   // public ItemsInStack Item;
    InventoryUiManager mng;
    public Item Item { set; get; }
    public void ShowInfo()
    {
        if (mng == null)
        {
            mng = FindObjectOfType<InventoryUiManager>();
        }
        if (Item.id <= 0 || mng.currentButton == this.gameObject)
        {
            mng.HideItemInfo();
            mng.currentButton = null;
            return;
        }
        
        mng.ShowItemInfo(Item,false);
        mng.currentButton = this.gameObject;
    }
}

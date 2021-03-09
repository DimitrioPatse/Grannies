using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] int amount;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            Inventory inv = col.gameObject.GetComponentInChildren<Inventory>();
            for (int i = 0; i < amount; i++)
            {
                Item itemToGive = Instantiate(item);
                inv.AddItemToStack(itemToGive);
            }
            Destroy(gameObject);
        }
    }
}

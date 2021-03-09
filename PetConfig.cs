using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet", menuName = "Items/New Pet", order = 5)]
public class PetConfig : Item
{
    [SerializeField] GameObject assignedPet;

    public void InstanciatePet()
    {
        Instantiate(assignedPet as GameObject);
    }
}

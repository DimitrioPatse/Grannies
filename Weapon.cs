using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    //Klash gia meelee nmz einai stimeno ayto gia ayto kai to event
    //Wstoso einai basiko kai gia to game 
    

    [SerializeField] UnityEvent onHit;

    public void OnHit()
    {
        onHit.Invoke();
    }
}
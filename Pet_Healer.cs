﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Healer : PetController
{
    [SerializeField] float actionsTakenEverySec = 1f;
    [SerializeField] float percentToHeal = 3f;
    [SerializeField] float hpToGoFix = 25f;
    [SerializeField] float attackDamage = 10f;    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Invoke("LateStart", actionsTakenEverySec);
    }
    void LateStart()
    {
        StartCoroutine(CalcNextAction());
    }
    IEnumerator CalcNextAction()
    {
        while (true)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            
            if (PlayerNeedsFix(hpToGoFix))
            {
                StartCoroutine(GoToTarget(player.transform));
                yield return new WaitWhile(()=> !atTarget);
                HealPlayer(percentToHeal);
            }
            else
            {
                target = GetTarget();
                if (target != null)
                {
                    StartCoroutine(GoToTarget(target));
                    yield return new WaitWhile(() => !atTarget);
                    MelleeAttackEnemy(attackDamage);
                }
                else
                {
                    StartCoroutine(GoToTarget(player.transform));
                    yield return new WaitWhile(() => !atTarget);
                }                
            }
            atTarget = false;
            yield return new WaitForSeconds(actionsTakenEverySec);
        }
    }
}

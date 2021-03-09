using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Ranged : EnemyController
{
    [Header("Attack Properties")]
    [SerializeField] float attackRange;
    [SerializeField] float timePerShoot = 1f;

    [Header("Patrol Properties")]
    [SerializeField] bool doesPtrol;
    [SerializeField] float maxPatrolDistance = 5f;
    [SerializeField] float timePerPatrol = 3f;
    [SerializeField] bool shootInPatrol = false;


    bool canMove;
    float timeSinceLastShot;
    float timeSinceLastMove;
    Vector3 nextDirection;
    float nextMoveDuration;
    float time;

    public override void Start()
    {
        base.Start();
        StartCoroutine(UpdateTimers());
    }

    void Update()
    {
        if(!canMove && timeSinceLastShot > timePerShoot)
        {
            canMove = true;
        }
        if (timeSinceLastShot >= timePerShoot && InRange())
        {
            canMove = shootInPatrol;
            AttackRanged(true);
            timeSinceLastShot = 0;
        }else if(timeSinceLastMove > timePerPatrol && doesPtrol)
        {
            RandomizeDirection();
            StartCoroutine(Patrol());
            timeSinceLastMove = 0;
        }
    }

    IEnumerator UpdateTimers()
    {
        while (true)
        {
            time = Time.deltaTime;
            timeSinceLastShot += time;
            timeSinceLastMove += time;
            nextMoveDuration -= time;
            yield return null;
        }
    }

    bool InRange()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return (dist < attackRange);
    }

    void RandomizeDirection()
    {
        nextDirection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        nextDirection.Normalize();
        nextMoveDuration = Random.Range(1, maxPatrolDistance / movementSpeed);
    }
    IEnumerator Patrol()
    {
        while (nextMoveDuration > 0 && canMove)
        {
            Move(nextDirection);
            yield return null;
        }
    }
    public override void OnCollisionEnter(Collision coll)
    {
        //9 is Wall Layer
        if (coll.gameObject.layer == 9) {
            nextDirection = -nextDirection;
        }
        base.OnCollisionEnter(coll);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Chaser : EnemyController
{
    [SerializeField] float attackRange = 10f;
    [SerializeField] float maxChaseDistance = 8f;
    [SerializeField] float timePerChase = 2f;
    [SerializeField] float maxPatrolDistance = 5f;
    [SerializeField] float timePerPatrol = 3f;
    [SerializeField] float patrolSpeedReduction = 0.5f;

    Vector3 nextDirection;
    float nextMoveDuration;
    float timeSinceLastChase = Mathf.Infinity;
    float timeSinceLastMove;
    float time;


    public override void Start()
    {
        base.Start();
        StartCoroutine(UpdateTimers());
    }

    void Update()
    {
        if (timeSinceLastChase > timePerChase && InRange())
        {
            nextMoveDuration = maxChaseDistance / movementSpeed;
            StartCoroutine(Chase());
            timeSinceLastChase = 0;
            timeSinceLastMove = 0;

        }
        else if (timeSinceLastMove > timePerPatrol)
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
            timeSinceLastChase += time;
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
        nextMoveDuration = Random.Range(1, maxPatrolDistance / movementSpeed * patrolSpeedReduction);
    }
    IEnumerator Patrol()
    {
        while (nextMoveDuration > 0)
        {
            Move(nextDirection, patrolSpeedReduction);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Chase()
    {
        transform.LookAt(player.transform);
        while (nextMoveDuration > 0)
        {
            Move(transform.forward);
            yield return null;
        }
    }

    public override void OnCollisionEnter(Collision coll)
    {
        //9 is Wall Layer
        if (coll.gameObject.layer == 9)
        {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Walker : EnemyController
{
    [SerializeField] float attackRange = 10f;
    [SerializeField] float maxChaseDistance = 10f;
    [SerializeField] float timePerChase = 1.5f;
    [SerializeField] float maxPatrolDistance = 5f;
    [SerializeField] float timePerPatrol = 3f;
    [SerializeField] float patrolSpeedReduction = 0.5f;

    Vector3 nextDirection;
    float nextMoveDuration;
    float timeSinceLastChase = Mathf.Infinity;
    float timeSinceLastMove;

    public override void Start()
    {
        base.Start();
        StartCoroutine(UpdateTimers());
    }

    void Update()
    {
        if (timeSinceLastChase >= timePerChase && InRange())
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
    IEnumerator UpdateTimers() //Htan sto Update
    {
        while (true)
        {
            timeSinceLastChase += Time.deltaTime;
            timeSinceLastMove += Time.deltaTime;
            nextMoveDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
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
        while (nextMoveDuration > 0)
        {
            transform.LookAt(player.transform);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Ranged_Clock : EnemyController
{
    [Header("Attack Properties")]
    [SerializeField] float attackRange;
    [SerializeField] float timePerShoot = 1f;
    [SerializeField] int shotsPerShot = 10;
    [SerializeField] float degreesPerShoot = 15f;
    [SerializeField] float timeBetweenEachTurnShot = 0.2f;

    float time;
    float timeSinceLastShot;
    bool shooting;

    public override void Start()
    {
        base.Start();
        StartCoroutine(UpdateTimers());
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (timeSinceLastShot > timePerShoot && !shooting)
            {
                shooting = true;
                for (int i = 0; i < shotsPerShot; i++)
                {
                    transform.Rotate(Vector3.up, degreesPerShoot);
                    AttackRanged(false);
                    yield return new WaitForSeconds(timeBetweenEachTurnShot);
                }
                timeSinceLastShot = 0;
            }
            shooting = false;
            yield return null;
        }
    }

    IEnumerator UpdateTimers()
    {
        while (true)
        {
            time = Time.deltaTime;
            timeSinceLastShot += time;
            yield return null;
        }
    }

    public override void OnCollisionEnter(Collision coll)
    {
        base.OnCollisionEnter(coll);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

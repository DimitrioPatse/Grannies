using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    [Header("General Pet Settings")]
    [SerializeField] protected float reachTargetAt = 1.5f;
    [SerializeField] protected float movementSpeed = 3f;
    [SerializeField] WeaponConfig defaultWeapon = null;
    [SerializeField] Transform[] handTransforms;

    Game_Master gm;

    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;
    protected Health player;
    protected Transform target;
    protected bool atTarget;


    void Awake()
    {
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
    }

    public virtual void Start()
    {
        gm = FindObjectOfType<Game_Master>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    /// <summary>
    /// Attaches a weapon to each hand
    /// </summary>
    protected Weapon AttachWeapon(WeaponConfig weapon)
    {
        Animator anim = GetComponentInChildren<Animator>();
        foreach (Transform hand in handTransforms)
        {
            return weapon.Spawn(hand, anim);
        }
        if (!handTransforms[0])
        {
            return weapon.Spawn(transform, anim);
        }
        return weapon.Spawn(handTransforms[0], anim);
    }
    protected void Cancel()
    {
        GetComponentInChildren<Animator>().ResetTrigger("attackTrigger");
        GetComponentInChildren<Animator>().SetTrigger("cancelAttack");
    }


    /// <summary>
    /// The aqual movement per frame
    /// </summary>
    /// <param name="direction"></param>
    protected void Move(Vector3 direction)
    {

        if (direction != Vector3.zero)
        {
            direction.y = 0;
            transform.LookAt(direction);
        }
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Asks GM for the next target and set him to "target"
    /// </summary>
    protected Transform GetTarget()
    {
        return gm.GetRandomTarget();
    }

    /// <summary>
    /// Looks at target and shoots a projectile from each hand
    /// </summary>
    /// <param name="damage"></param>
    protected void AttackRanged(float damage, Health target)
    {
        transform.LookAt(target.transform);

        if (!handTransforms[0])
        {
            Debug.Log("No hand Transforms to " + gameObject.name + "... Enemy can't shot ranged.");
            return;
        }
        foreach (Transform hand in handTransforms)
        {
            currentWeaponConfig.LaunchProjectile(hand, target, damage);
        }
    }
    /// <summary>
    /// Heals Player partially
    /// </summary>
    protected void HealPlayer(float percentToHeal)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        player.HealPercent(percentToHeal);
    }

    /// <summary>
    /// Mellee attack enemy target
    /// </summary>
    protected void MelleeAttackEnemy(float attackDamage)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        if (target == null) return;//In case enemy dies while going to him
        target.GetComponent<Health>().TakeDamage(attackDamage);
    }

    /// <summary>
    /// Coroutine that moves you to the given target up to "reachTargetAt" distance
    /// </summary>
    protected IEnumerator GoToTarget(Transform targetTransform)
    {
        Vector3 dir = (targetTransform.position - transform.position);

        while (dir.magnitude > reachTargetAt)
        {
            Move(dir.normalized);
            if (targetTransform == null) break;//In case enemy dies while going to him
            dir = targetTransform.position - transform.position;
            yield return null;
        }
        atTarget = true;
    }

    /// <summary>
    /// Checks Player for Hp% and returns true if he needs fixing
    /// </summary>
    protected bool PlayerNeedsFix(float minHpThreshold)
    {
        float hp = player.GetHealthPersentage();
        return (hp > minHpThreshold) ? false : true;
    }
}

public enum PetKind{
    Healer, Fighter, Missile, Bomber, Ninja 
}

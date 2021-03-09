using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header ("General Enemy Properties")]
    [SerializeField] protected float movementSpeed = 3f;
    [Range(0, 1)]
    [SerializeField] float rotationThreshold = 0.15f;
    [SerializeField] WeaponConfig defaultWeapon = null;
    [SerializeField] Transform[] handTransforms;

    float calculatedDamage;
    bool slowProjectiles;
    Health target;
    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;
    protected GameObject player;

    protected BaseStats stats;

    void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        stats = GetComponent<BaseStats>();
    }

    Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    public virtual void Start()
    {
        currentWeapon.ForceInit();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Health>();
        calculatedDamage = stats.GetStat(Stat.Damage);
        slowProjectiles = FindObjectOfType<AbillitySystem>().slowEnemyProjectiles;
    }

    public void Move(Vector3 direction)
    {
        Move(direction, 1);
    }
    public void Move(Vector3 direction, float speedMultiplier)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationThreshold);
        }
        transform.Translate(direction * movementSpeed * speedMultiplier * Time.deltaTime, Space.World);
    }

    public void AttackRanged(bool aimAtPlayer)
    {
        if (aimAtPlayer)//An true tote koitaei ton paixth prin riksei
        {
            transform.LookAt(player.transform);
        }

        if (!handTransforms[0])
        {
            Debug.Log("No hand Transforms to " + gameObject.name +"... Enemy can't shot ranged.");
            return;
        }
        foreach (Transform hand in handTransforms)
        {
            currentWeaponConfig.LaunchProjectile(hand, target, calculatedDamage, slowProjectiles);
        }
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon.value = AttachWeapon(weapon);
    }

    public Weapon AttachWeapon(WeaponConfig weapon)
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
    public void SlowProjectiles()
    {
        slowProjectiles = true;
    }
    public void Cancel()
    {
        GetComponentInChildren<Animator>().ResetTrigger("attackTrigger");
        GetComponentInChildren<Animator>().SetTrigger("cancelAttack");
    }

    public virtual void OnCollisionEnter(Collision coll)
    {
        GameObject otherObj = coll.gameObject;
        if (otherObj.CompareTag("Player"))
        {
            target.TakeDamage(stats.GetStat(Stat.Damage));
        }
    }
}


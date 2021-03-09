using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/New Weapon", order = 0)]
public class WeaponConfig :  Item
{
    [SerializeField] Weapon equippedWeapon = null;
    [SerializeField] Projectile projectile = null;
    [SerializeField] float timePerAttack;
    const string weaponName = "Weapon";

    public float GetTimePerAttack()
    {
        return timePerAttack;
    }

    /// <summary>
    /// Spawns a weapon in hand and destroys the previous one
    /// </summary>
    public Weapon Spawn(Transform hand, Animator animator)
    {
        DestroyOldWeapon(hand);
        Weapon weapon = null;

        if (equippedWeapon != null)
        {
            weapon = Instantiate(equippedWeapon, hand);
            weapon.gameObject.name = weaponName;
        }
        return weapon;
    }

    /// <summary>
    /// Destroies old weapon to equip a new one
    /// </summary>
    void DestroyOldWeapon(Transform hand)
    {
        Transform oldWeapon = hand.Find(weaponName);
        if (oldWeapon == null) return;

        oldWeapon.name = "DestroyingWeapon"; // to metonomazoyme prin th diagrafh giati vgazei bug mperdeyontas to palio me to neo e3aitias toy onomatos
        Destroy(oldWeapon.gameObject);
    }

    /// <summary>
    /// Instantiates a projectile and sets Target, Damage and Abillities
    /// </summary>
    public void LaunchProjectile(Transform hand, Health target, float calculatedDamage, bool canTargetThrough, bool canWallBounce, float angle,bool isBurning, bool isFreezing, bool isPoisonous, float critRate, float critMultiplier, bool fastProjectiles)
    {
        Projectile projectileInstance = Instantiate(projectile, hand.position, hand.rotation);
        projectileInstance.SetTarget(target, calculatedDamage, hand.rotation.eulerAngles.y + angle);
        projectileInstance.SetAbillities(isBurning, isFreezing, isPoisonous, canTargetThrough, canWallBounce, critRate, critMultiplier);
        if (fastProjectiles) projectileInstance.FastProjectiles();//Ability for Players Projectiles
    }

    /// <summary>
    /// Instantiates a projectile and sets Target, Damage with no Abillities
    /// </summary>
    public void LaunchProjectile(Transform hand, Health target, float calculatedDamage)
    {
        Projectile projectileInstance = Instantiate(projectile, hand.position, hand.rotation);
        projectileInstance.SetTarget(target, calculatedDamage, hand.rotation.eulerAngles.y);
    }

    /// <summary>
    /// Instantiates a projectile and sets Target, Damage with slowProjectiles ability
    /// </summary>
    public void LaunchProjectile(Transform hand, Health target, float calculatedDamage, bool slowProjectiles)
    {
        Projectile projectileInstance = Instantiate(projectile, hand.position, hand.rotation);
        projectileInstance.SetTarget(target, calculatedDamage, hand.rotation.eulerAngles.y);
        if (slowProjectiles) projectileInstance.SlowProjectiles(); // Ability that affects enemy projectiles
    }

    public void SpawnProjectiles(int numberOfProjectiles, Transform hand, Health target, float calculatedDamage)
    {
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {

            float projectileDirXposition = hand.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float projectileDirYposition = hand.position.z + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 projectileVector = new Vector3(projectileDirXposition, 0, projectileDirYposition);
            Vector3 projectileMoveDirection = (projectileVector - hand.position).normalized;

            var proj = Instantiate(projectile, hand.position, Quaternion.identity);


            proj.GetComponent<Rigidbody2D>().velocity =
                new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;
        }
    }
}

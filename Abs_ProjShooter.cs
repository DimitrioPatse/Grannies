using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abs_ProjShooter : MonoBehaviour
{
    [SerializeField] WeaponConfig weapon;
    [SerializeField] float timePerShoot = 3f;
    [SerializeField] float damagePercent = 15f;
    [SerializeField] ParticleSystem OnLvlCompParticle;

    float timeSinceLastShot;
    float damage;
    Game_Master gm;

    private void Awake()
    {
        gm = FindObjectOfType<Game_Master>();

    }
    private void Start()
    {
        // TODO Na ftiajw systhma gia to ti dmg kanei
        damage = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BaseStats>().GetStat(Stat.Damage) / 100 * damagePercent;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if(timeSinceLastShot > timePerShoot && gm.GetRandomTarget())
        {
            Health target = gm.GetRandomTarget().GetComponent<Health>();
            transform.LookAt(target.transform.position);
            weapon.LaunchProjectile(transform, target, damage);
            timeSinceLastShot = 0;
        }

    }
    private void OnEnable()
    {
        gm.onLevelComplete += DestroyMe;
    }
    private void OnDisable()
    {
        gm.onLevelComplete -= DestroyMe;
    }
    void DestroyMe() {
        if (OnLvlCompParticle)
        {
            Instantiate(OnLvlCompParticle, transform.position, Quaternion.identity, transform);
            Destroy(gameObject, OnLvlCompParticle.main.duration);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

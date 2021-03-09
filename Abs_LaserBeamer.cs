using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abs_LaserBeamer : MonoBehaviour
{
    [Range(1,100)]
    [SerializeField] float damagePercentage = 10f;
    [SerializeField] float timePerLaserbeam = 5f;
    [SerializeField] float timeLaserbeamOn = 0.5f;
    [SerializeField] ParticleSystem OnLvlCompParticle;

    int creepLevel = 1;

    GameObject player;
    LineRenderer lineRenderer;
    Game_Master gm;

    float damage;
    float timeSinceLastLaserBlast = 0;
    float timeSinceLaserActivated = 0;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gm = FindObjectOfType<Game_Master>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        CalculateDamage();
        lineRenderer.enabled = false;
        StartCoroutine("MoveToPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastLaserBlast += Time.deltaTime;

        if(timeSinceLastLaserBlast > timePerLaserbeam && gm.GetRandomTarget())
        {
            StartCoroutine("BlastLaser");
            timeSinceLastLaserBlast = 0;
        }
    }
    
    IEnumerator BlastLaser()
    {
        Transform target = gm.GetRandomTarget();
        lineRenderer.enabled = true;
        target.GetComponent<Health>().TakeDamage((int)damage);
        while (timeLaserbeamOn > timeSinceLaserActivated && target)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.position);
            timeSinceLaserActivated += Time.deltaTime;
            yield return null;
        }        
        lineRenderer.enabled = false;
        timeSinceLaserActivated = 0;
    }

    void CalculateDamage() 
    {
        float baseDamage = player.GetComponent<BaseStats>().GetStat(Stat.Damage);
        damage = baseDamage / 100 * damagePercentage;
    }

    IEnumerator MoveToPlayer()
    {
        transform.Translate(Vector3.Lerp(transform.position, player.transform.position, 1 * Time.deltaTime));
        yield return null;
    }
    private void OnEnable()
    {
        gm.onLevelComplete += DestroyMe;
    }
    private void OnDisable()
    {
        gm.onLevelComplete -= DestroyMe;
    }
    void DestroyMe()
    {
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

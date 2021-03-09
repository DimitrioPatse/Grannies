using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool followTarget = false;
    [SerializeField] UnityEvent onHit;
    float lifeAfterImpact = 0.1f;
    [SerializeField] int maxWallBounces;

    Health target;
    float damage = 0f;
    float criticalRate = -1f;
    float criticalDmgMultiplier = 1.5f;
    
    bool targetThrough;
    bool wallBounce;

    bool hitEnemies = false;

    bool isBurning;
    bool isFreezing;
    bool isPoisonous;
    
    Vector3 direction;

    private void Start()
    {
        direction = transform.forward;

        //This is for the abilitySystem.WallBounce or if it's an enemy's proj that does bounce walls
        //2 is the limit for the WallBounce. If to change do it here
        maxWallBounces = (maxWallBounces > 0) ? maxWallBounces : 2; 
    }

    void FixedUpdate()
    {
        if (followTarget && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        Move();
    }
    
    /// <summary>
    /// Translates the projectile
    /// </summary>
    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1);
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Sets enemy target and what damage to do
    /// </summary>
    public void SetTarget(Health target, float calculatedDamage, float angle)
    {
        damage = calculatedDamage;

        transform.rotation = Quaternion.Euler(0,angle,0);
        hitEnemies = target.gameObject.CompareTag("Enemy");       
    }


    /// <summary>
    /// Sets the (poison, flame, ice) effects on projectile, if it can bounce walls and if it can go trough enemies
    /// </summary>
    public void SetAbillities(bool isBurning, bool isFreezing, bool isPoisonous, bool canTargetThrough, bool canWallBounce, float criticalRate, float criticalDmgMultiplier)
    {
        targetThrough = canTargetThrough;
        wallBounce = canWallBounce;
        this.isBurning = isBurning;
        this.isFreezing = isFreezing;
        this.isPoisonous = isPoisonous;
        this.criticalRate = criticalRate;
        this.criticalDmgMultiplier += criticalDmgMultiplier;
    }

    /// <summary>
    /// Sets available bounces for the projectile. It's set to 2 already. Projectile must have the canBounce bool set to true 
    /// </summary>
    public void SetBounces(int bounces)
    {
        maxWallBounces = bounces;
    }

    /// <summary>
    /// Gets the aiming position of the enemy
    /// </summary>
    Vector3 GetAimLocation()
    {
        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider == null)
        {
            return target.transform.position + Vector3.up;
        }
        return targetCollider.bounds.center; 
    }

    /// <summary>
    /// Slows down projectile speed
    /// </summary>
    public void SlowProjectiles(){ speed /= 1.5f; }

    /// <summary>
    /// Increases projectile speed
    /// </summary>
    public void FastProjectiles() { speed *= 1.5f; }


    private void OnTriggerEnter(Collider collider)
    {
        // Collision with Projectile
        if (collider.GetComponent<Projectile>()) return;

        // Player's shot collides Player
        if (collider.gameObject.CompareTag("Player") && hitEnemies) return;

        // Enemy's shot collides Player
        if (collider.gameObject.CompareTag("Player") && !hitEnemies)
        {
            //Calc Evation chance and get damage if not lucky
            float rnd = Random.value * 100;
            float evation = collider.gameObject.GetComponent<PlayerController>().evation;
            print("Evation chance = " + rnd.ToString());
            if(rnd <= evation)//If evade succesfull
            {
                collider.GetComponentInChildren<DamageTextSpawner>().Spawn("Evaded");
            }
            else//If evade failed, so do attack
            {
                Dattack(collider.GetComponent<Health>(), damage);
            }

            speed = 0;
            Destroy(gameObject, lifeAfterImpact);
            return;
        }

        // Player's shot collides Enemy
        if (collider.gameObject.CompareTag("Enemy") && hitEnemies) 
        {
            //Calc crit chance and set dmg value
            float rnd = Random.value * 100;
            damage = (rnd <= criticalRate) ? damage * criticalDmgMultiplier : damage;
            //print("Calced crit chance = " + rnd.ToString());

            //Attack
            Dattack(collider.GetComponent<Health>(), damage);

            //Ability.ThroughTarget
            if (targetThrough) return;
            
            //Destroy projectile
            speed = 0;
            Destroy(gameObject, lifeAfterImpact);
            return;
        }

        // Wall collisions
        if (collider.gameObject.CompareTag("Wall")) 
        {
            //If it can wall bounce and have available bounces
            if (wallBounce && maxWallBounces > 0) 
            {
                RaycastHit hit;
                
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1000, LayerMask.GetMask("Wall")))
                {               
                    Vector3 incomingVec = hit.point - transform.position; // Find the line from the gun to the point that was clicked.    
                    direction = Vector3.Reflect(incomingVec, hit.normal); // Use the point's normal to calculate the reflection vector.
                }
                maxWallBounces--;
            }
            //If it cannot bounce walls or not available bounces left
            else
            {
                Destroy(gameObject, lifeAfterImpact);
            }
        }
    }

    /// <summary>
    /// The core attack method
    /// </summary>
    void Dattack(Health hitTarget, float damageToDo)
    {
        if (hitTarget.IsDead()) return;
        onHit.Invoke();

        hitTarget.TakeDamage(damageToDo);

        //Set Buffs
        if (isBurning) hitTarget.SetBuffs(Buffs.Burned);
        if (isFreezing) hitTarget.SetBuffs(Buffs.Frozen);
        if (isPoisonous) hitTarget.SetBuffs(Buffs.Poisoned);
    }
}

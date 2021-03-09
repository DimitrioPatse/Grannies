using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    [SerializeField] float movementSpeed = 3f;
    [Range(0, 1)]
    [SerializeField] float rotationThreshold = 0.15f;
    [SerializeField] WeaponConfig defaultWeapon = null;
    [SerializeField] Transform handTransform = null;
    [SerializeField] float timePerAttack = 1f;
    Vector3 inputDirection;
    Vector3 movementDirection;
    
    bool canMove = true;
    bool canAttack = true;
    bool isMoving;
    float timeSinceLastAttack;

    int extraShot;
    int extraFront;
    int extraRear;
    int extraSides;
    int extraAngled;
    int extraAngledRear;

    float damage;
    float critRate;
    float critDmgMultiplier;
    bool canThrough;
    bool canBounce;
    bool isBurning;
    bool isFreezing;
    bool isPoisonous;
    bool fastProjectiles;
    public float evation { get; private set; }

    Rigidbody myRigi;
    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;
    BaseStats stats;
    AbillitySystem abillities;
    Game_Master gameMaster;
    Joystick joystick;
    Transform target = null;

    void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        myRigi = GetComponent<Rigidbody>();
        gameMaster = FindObjectOfType<Game_Master>();
        abillities = GetComponent<AbillitySystem>();
        stats = GetComponentInChildren<BaseStats>();
        joystick = FindObjectOfType<Joystick>();
    }

    public Weapon SetupDefaultWeapon()
    {
        currentWeaponConfig = defaultWeapon;
        return AttachWeapon(defaultWeapon);
    }

    void Start()
    {
        currentWeapon.ForceInit();
        timePerAttack = currentWeaponConfig.GetTimePerAttack();
        GetComponentInChildren<Inventory>().SetupEquipedItemsInGame();//Loads weapon, pet etc.

        //Invoke("SetAbillityValues", 0.05f);
        SetAbillityValues();

        StartCoroutine(UpdateTimers());
    }
    void OnEnable()
    {
        if (gameMaster == null)
        {
            Debug.LogError("No GM found in player controller");
            return;
        }
        gameMaster.onPlayerDeath += DisableActions;
    }
    void OnDisable()
    {
        if (gameMaster == null)
        {
            Debug.LogError("No GM found in player controller");
            return;
        }
        gameMaster.onPlayerDeath -= DisableActions;
    }
    void SetAbillityValues()
    {
        movementSpeed *= 1 + abillities.extraMoveSpeed/100;

        damage = CalcDamage();
        timePerAttack = CalcAttackSpeed();
        critRate = abillities.criticalRatePlus;
        critDmgMultiplier = abillities.criticalDamagePlus / 100;

        canThrough = abillities.throughTarget;
        canBounce = abillities.wallBounce;
        isBurning = abillities.flameEffect;
        isFreezing = abillities.iceEffect;
        isPoisonous = abillities.poisonEffect;

        extraShot = abillities.extraShot;
        extraFront = abillities.extraFront;
        extraRear = abillities.extraRear;
        extraSides = abillities.extraSides;
        extraAngled = abillities.extraAngled;
        extraAngledRear = abillities.extraAngledRear;

        fastProjectiles = abillities.fastPlayerProjectiles;
        evation = abillities.evation;
    }

    /// <summary>
    /// Calculates exponentially the final Damage depending on count of Ability.DamagePlus 
    /// </summary>
    float CalcDamage()
    {
        float dmg = stats.GetStat(Stat.Damage);
        float dmgAbs = abillities.damagePlus;
        int dmgAbValueStep = (int)abillities.GetAbilityFloatValue(AbillityClass.DamagePlus);
        for (int i = 0; i <= dmgAbs; i+=dmgAbValueStep)
        {
            dmg *= 1 + (dmgAbValueStep/100);
        }
        return dmg;
    }
    /// <summary>
    /// Calculates exponentially the attack speed depending on count of Ability.AttackSpeed
    /// </summary>
    float CalcAttackSpeed()
    {
        float rate = abillities.attackSpeed;
        int atcSpValue = (int)abillities.GetAbilityFloatValue(AbillityClass.AttackSpeed);
        float atcTime = timePerAttack;
        for (int i = 0; i <= rate; i+= atcSpValue)
        {
            atcTime -= atcTime * (atcSpValue / 100);
        }
        return atcTime;
    }

    /// <summary>
    /// The final Calculated Damage
    /// </summary>
    public float GetDamage() { return damage; }

    void FixedUpdate()
    {
        if (gameMaster == null) return;

        GetAxis();
        if (canMove)
        {
            Move();
        }

        if (!isMoving && timeSinceLastAttack >= timePerAttack)
        {
            target = gameMaster.GetClosestTarget();
            StartCoroutine(Attack());
        }
    }
    public void EnableActions()
    {
        canMove = true;
        canAttack = true;
    }
    public void DisableActions()
    {
        canMove = false;
        canAttack = false;
    }
    void GetAxis()
    {
        inputDirection = joystick.Direction;

        if(inputDirection == Vector3.zero)//Gia na douleuei kai me keyboard 
        {
            inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
    }

    IEnumerator UpdateTimers()
    {
        while (true)
        {
            timeSinceLastAttack += Time.deltaTime;
            yield return null;
        }
    }

    void Move()
    {
        movementDirection = inputDirection;
        
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationThreshold);
            isMoving = true;
        }

        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
        if (movementDirection == Vector3.zero)
        {
            isMoving = false;
            myRigi.velocity = Vector3.zero;
        }
    }

    IEnumerator Attack()
    {
        if (target && canAttack)
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            timeSinceLastAttack = 0f;

            //For every Ability.ExtraShot launch a projectile
            for (int i = 0; i < extraShot + 1; i++)
            {
                if (isMoving || !target) continue;
                LaunchProjectile();
                yield return new WaitForSeconds(timePerAttack * 0.2f);
            }
        }
    }

    void LaunchProjectile()
    {
        Health targetHealth = target.GetComponent<Health>();
         

        for (int i = 0; i < extraFront + 1; i++)
        {
            currentWeaponConfig.LaunchProjectile(handTransform , targetHealth, damage, canThrough, canBounce, -(extraFront * 5 * i) + ( 10 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
        }
        for (int i = 0; i < extraAngled; i++)
        {
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, -45 - (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, 45 + (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
        }
        for (int i = 0; i < extraSides; i++)
        {
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, -90 - (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, 90 + (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
        }
        for (int i = 0; i < extraRear; i++)
        {
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, 180 + (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);

        }
        for (int i = 0; i < extraAngledRear; i++)
        {
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, 135 + (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
            currentWeaponConfig.LaunchProjectile(handTransform, targetHealth, damage, canThrough, canBounce, -135 - (5 * i), isBurning, isFreezing, isPoisonous, critRate, critDmgMultiplier, fastProjectiles);
        }
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon.value = AttachWeapon(weapon);
    }

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
        Animator anim = GetComponent<Animator>();
        return weapon.Spawn(handTransform, anim);
    }

    public void Cancel()                                            // TODO Use it
    {                                                              ///////////////
        GetComponent<Animator>().ResetTrigger("attackTrigger");
        GetComponent<Animator>().SetTrigger("cancelAttack");
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    public float damageDeviation = 1.15f;

    public string towerName;

    private Transform turretTransform, cannon1, cannon2;

    public GameObject bulletPrefab;
    public GameObject enemySpawners;

    public int cost = 5;
    public float damage = 1;
    public float radius = 0;
    public float range = 15f;
    public float acquisitionRange = 25f;
    public float fireCooldown = 1f;
    private float fireCooldownLeft = 0;
    public float bulletSpeed = 20f;
    public float reactionCooldown = 0.1f;
    private float reactionTime = 0f;
    public bool doubleCannons = false;
    public int upgradeLevel = 1;
    public float upgradeCost = 12;

    private float upgradeDamageFactor = 1.25f;
    private float upgradeCostFactor = 1.2f;

    private bool cannonSwitch = false;
    private AudioSource gunShot;
    
    private Enemy target = null;

    private System.Random random = new System.Random();

    private float baseDamage;
    private float baseCost;

    public int upgrade()
    {
        upgradeLevel++;
        damage += baseDamage * Mathf.Pow(upgradeDamageFactor, upgradeLevel);
        // fireCooldown *= Mathf.Pow(upgradeFactor * 0.01f + 1, (upgradeLevel - 1));
        upgradeCost = baseCost * Mathf.Pow(upgradeCostFactor, upgradeLevel);

        return upgradeLevel;
    }

    // Use this for initialization
    void Start()
    {
        turretTransform = transform.Find("Head");
        cannon1 = turretTransform.Find("Cannon_1");
        cannon2 = turretTransform.Find("Cannon_2");

        gunShot = GetComponent<AudioSource>();

        baseDamage = damage;
        baseCost = cost;
        upgradeCost = baseCost * Mathf.Pow(upgradeCostFactor, upgradeLevel);
    }

    private void findCloseEnemies(float distance)
    {
        reactionTime += Time.deltaTime;
        if (reactionTime >= reactionCooldown)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, acquisitionRange);

            foreach (Collider collider in hitColliders)
            {
                if (collider.name == "main")
                {
                    Enemy enemy = collider.GetComponentInParent<Enemy>();
                    if (enemy != null)
                    {
                        float distanceToTarget = Vector3.Distance(this.transform.position, enemy.transform.position);
                        if (distanceToTarget < distance)
                        {
                            target = enemy;
                            break;
                        }
                    }        
                }
            }
            reactionTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            findCloseEnemies(acquisitionRange);
        else
        {
            fireCooldownLeft -= Time.deltaTime;
            float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
            if (distanceToTarget > acquisitionRange)
                findCloseEnemies(acquisitionRange);  
            else{
                Vector3 dir = target.transform.position - this.transform.position;

                if (towerName != "Tesla Coil")
                {
                    Quaternion lookRot = Quaternion.LookRotation(dir);
                    turretTransform.rotation = Quaternion.Lerp(turretTransform.rotation, Quaternion.Euler(lookRot.eulerAngles.x, lookRot.eulerAngles.y, 0), Time.deltaTime * 5);
                }

                if (distanceToTarget <= range)
                {    
                    if (fireCooldownLeft <= 0 && dir.magnitude <= range)
                    {
                        fireCooldownLeft = fireCooldown;
                        ShootAt(target);
                    }
                }
                else        
                    findCloseEnemies(distanceToTarget);
            }
        }
    }

    public int getMinDamage(){
        return (int)(damage / damageDeviation);
    }

    public int getMaxDamage(){
        return (int)(damage * damageDeviation);
    }

    public int getRandomDamage(){
        return random.Next(getMinDamage(), getMaxDamage()+1);
    }

    void ShootAt(Enemy e)
    {
        gunShot.Play();
        Vector3 newPosition;

        if (doubleCannons)
            newPosition = cannonSwitch ? cannon1.position + cannon1.forward : cannon2.position + cannon2.forward;
        else
            newPosition = turretTransform.position + new Vector3(0, 2, 0) + turretTransform.forward;

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, newPosition, turretTransform.rotation);
        cannonSwitch = !cannonSwitch;

        if (towerName == "Tesla Coil")
        {
            Lightning b = bulletGO.GetComponent<Lightning>();
            b.target = e;
            b.lastHitPosition = e.transform.position;
            b.damage = getRandomDamage();
            b.radius = radius;
            b.lightningJumps = 2 + upgradeLevel * (int)0.25f;
            b.transform.position = transform.position + new Vector3(0, 4f, 0);
        }
        else if (towerName == "Rail Gun")
        {
            RailGunRay b = bulletGO.GetComponent<RailGunRay>();
            b.target = e.transform;
            b.damage = getRandomDamage();
            b.speed = bulletSpeed;
            b.lifespan = (acquisitionRange / bulletSpeed) * 3;
            b.start = true;
        }
        else
        {
            Bullet b = bulletGO.GetComponent<Bullet>();
            b.target = e.transform;
            b.damage = getRandomDamage();
            b.radius = radius;
            b.speed = bulletSpeed;
            b.lifespan = (acquisitionRange / bulletSpeed) * 2;
            b.start = true;
        }
    }
}

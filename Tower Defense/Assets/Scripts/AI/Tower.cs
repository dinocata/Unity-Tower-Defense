using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    Transform turretTransform;

    public float range = 15f;
    public GameObject bulletPrefab;
    public GameObject enemySpawners;

    public int cost = 5;

    public float fireCooldown = 1f;
    float fireCooldownLeft = 0;

    public float damage = 1;
    public float radius = 0;

    private List<Enemy> enemies = new List<Enemy>();

    // Use this for initialization
    void Start()
    {
        turretTransform = transform.Find("Head");
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpawner enemySpawner = enemySpawners.transform.GetChild(0).gameObject.GetComponent<EnemySpawner>();

        foreach(EnemySpawner.WaveComponent wc in enemySpawner.waveComps){
            foreach(Enemy e in wc.enemies){
                enemies.Add(e);
            }
        }

        Enemy nearestEnemy = null;
        float dist = Mathf.Infinity;

        foreach (Enemy e in enemies)
        {
            float d = Vector3.Distance(this.transform.position, e.transform.position);
            if (nearestEnemy == null || d < dist)
            {
                nearestEnemy = e;
                dist = d;
            }
        }

        if (nearestEnemy == null)
        {
            Debug.Log("No enemies?");
            return;
        }

        Vector3 dir = nearestEnemy.transform.position - this.transform.position;

        Quaternion lookRot = Quaternion.LookRotation(dir);
        turretTransform.rotation = Quaternion.Lerp(turretTransform.rotation, Quaternion.Euler(0, lookRot.eulerAngles.y, 0), Time.deltaTime * 5);

        fireCooldownLeft -= Time.deltaTime;
        if (fireCooldownLeft <= 0 && dir.magnitude <= range)
        {
            fireCooldownLeft = fireCooldown;
            ShootAt(nearestEnemy);
        }

    }

    void ShootAt(Enemy e)
    {
        // TODO: Fire out the tip!
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);

        Bullet b = bulletGO.GetComponent<Bullet>();
        b.target = e.transform;
        b.damage = damage;
        b.radius = radius;
        b.start = true;
    }
}

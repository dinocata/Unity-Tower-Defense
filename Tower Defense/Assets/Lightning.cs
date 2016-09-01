using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour {

    public GameObject explosionPrefab;

    private float recoverTimer = 0.025f;
    private float recoverRemaining = 0.025f;

    public float damageLoss = 0.25f;
    public Enemy target;
    public float damage;
    public float radius = 8;
    public int lightningJumps = 3;

    public List<Enemy> damagedUnits;

    private bool init = true;
    private bool hit = true;
    private bool dead = false;

    public Vector3 lastHitPosition;

    private void kill()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        Destroy(gameObject,1f);
        dead = true;
    }

    void Start()
    {
        damagedUnits = new List<Enemy>();
    }
	
	// Update is called once per frame
	void Update () {

        recoverRemaining -= Time.deltaTime;

        if (recoverRemaining <= 0 && !dead)
        {
            if (!hit || (init && target == null))
            {
                kill();
                return;
            }

            if (init)
            {
                transform.position = target.transform.position;
                GameObject explosionGO = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(explosionGO, 2f);
                init = false;
                target.TakeDamage(damage);
                damagedUnits.Add(target);
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(lastHitPosition, radius);
                hit = false;

                foreach (Collider collider in hitColliders)
                {
                    if (collider.name == "main")
                    {
                        Enemy enemy = collider.GetComponentInParent<Enemy>();
                        if (enemy != null && !damagedUnits.Contains(enemy))
                        {
                            damage *= (1 - damageLoss);
                            transform.position = enemy.transform.position;
                            enemy.TakeDamage(damage);
                            damagedUnits.Add(enemy);
                            lastHitPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
                            lightningJumps--;
                            GameObject explosionGO = (GameObject)Instantiate(explosionPrefab, lastHitPosition, transform.rotation);
                            Destroy(explosionGO, 2f);
                            hit = true;
                            
                            break;
                        }
                    }
                }

                recoverRemaining = recoverTimer;

                if (lightningJumps <= 0 || !hit)
                {
                    kill();
                }
            } 
        }  
	}
}

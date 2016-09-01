using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class RailGunRay : MonoBehaviour {

    public GameObject explosionPrefab;

    public float speed;
    public float damage;
    public float damageLoss = 0.25f;
    public float bulletSize = 0.5f;
    public bool start = false;
    public float lifespan = 0f;
    public Transform target;

    private List<Collider> damagedUnits = new List<Collider>();

    private void kill()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        start = false;
        Destroy(gameObject, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            lifespan -= Time.deltaTime;
            if (lifespan <= 0)
            {
                kill();
                return;
            }

            if (target != null)
            {
                float distThisFrame = speed * Time.deltaTime;
                Vector3 dir = target.position - this.transform.localPosition;
                transform.Translate(dir.normalized * distThisFrame, Space.World);
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime);
                if (dir.magnitude <= distThisFrame * 2)
                    target = null;
            }
            else
            {
                float distThisFrame = speed * Time.deltaTime;
                transform.position += transform.forward * Time.deltaTime * speed;
            }    

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, bulletSize);
            foreach (Collider collider in hitColliders)
            {
                if (collider.name == "main" && !damagedUnits.Contains(collider))
                {
                    Enemy e = collider.GetComponentInParent<Enemy>();
                    if (e != null)
                    {
                        GameObject explosionGO = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
                        Destroy(explosionGO, 2f);
                        e.GetComponent<Enemy>().TakeDamage(damage);
                        damagedUnits.Add(collider);

                        damage *= (1 - damageLoss);    
                    }
                }          
            }
        }
    }
}

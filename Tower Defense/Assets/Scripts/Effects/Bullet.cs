using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;

    public float speed;
    public Transform target;
    public float damage;
    public float radius = 0;
    public float bulletSize = 1f;
    public bool start = false;
    public float lifespan = 0f;

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
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);

                if (dir.magnitude <= distThisFrame * 2)
                {
                    DoBulletHit();
                }
            }
            else
            {
                transform.position += transform.forward * Time.deltaTime * speed;
            }    
        }
    }

    void DoBulletHit()
    {
        if (radius == 0)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
        else
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            GameObject explosionGO = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosionGO, 2f);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in hitColliders)
            {
                if (collider.name == "main")
                {
                    Enemy e = collider.GetComponentInParent<Enemy>();
                    if (e != null && e != target)
                    {
                        explosionGO = (GameObject)Instantiate(explosionPrefab, e.transform.position, transform.rotation);
                        Destroy(explosionGO, 2f);
                        e.GetComponent<Enemy>().TakeDamage(damage * 0.5f);    
                    }                               
                }
            }
        }

        // TODO: Maybe spawn a cool "explosion" object here?

        kill();
    }
}

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject explosionPrefab;

    private Transform targetPathNode;
    private int pathNodeIndex = 0;
    
    public float speed = 5f;
    public float health = 10f;
    public int moneyValue = 1;

    public MainPath mainPath;
    public ScoreManager scoreManager;

    void GetNextPathNode()
    {
        if (pathNodeIndex < mainPath.getNodeCount())
        {
            targetPathNode = mainPath.getPathNode(pathNodeIndex);
            pathNodeIndex++;
        }
        else
        {
            targetPathNode = null;
            ReachedGoal();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPathNode == null)
        {
            GetNextPathNode();
            if (targetPathNode == null)
            {
                // We've run out of path!
                return;
            }
        }

        Vector3 dir = targetPathNode.position - transform.localPosition;

        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame)
        {
            // We reached the node
            targetPathNode = null;
        }
        else
        {
            // Move towards node
            transform.Translate(dir.normalized * distThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    void ReachedGoal()
    {
        scoreManager.LoseLife();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {      
            Die();
        }
    }

    public void Die()
    {
        scoreManager.money += moneyValue;
        GameObject explosionGO = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosionGO, 2f);
        Destroy(gameObject);
    }
}

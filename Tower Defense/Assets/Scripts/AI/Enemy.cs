using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Transform targetPathNode;
    private int pathNodeIndex = 0;
    private bool dead = false;

    public float speed = 5f;
    public float health = 10f;
    public int moneyValue = 1;

    public MainPath mainPath;
    public ScoreManager scoreManager;

    public bool isDead()
    {
        return dead;
    }

    // Use this for initialization
    void Start()
    {

    }

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
        gameObject.SetActive(false);
        dead = true;
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
        gameObject.SetActive(false);
        dead = true;
    }
}

using UnityEngine;
using System.Collections;

public class TowerSpot : MonoBehaviour {

    void OnMouseUp()
    {
        Debug.Log("TowerSpot clicked");

        BuildingManager bm = GameObject.FindObjectOfType<BuildingManager>();
        if (bm.selectedTower != null)
        {
            ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
            Tower tower = bm.selectedTower.GetComponent<Tower>();
            if (sm.money < tower.cost)
            {
                Debug.Log("Not enough money!");
                return;
            }

            sm.money -= tower.cost;

            Instantiate(bm.selectedTower, transform.parent.position, transform.parent.rotation);
            Destroy(transform.parent.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

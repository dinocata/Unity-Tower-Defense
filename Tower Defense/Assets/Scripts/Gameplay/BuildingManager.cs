using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {

    public GameObject enemySpawners;
    public GameObject selectedTower;
    public Camera mainCamera;
    public Terrain terrain;

    private ScoreManager scoreManager;

    private float minHeight = 9.95f;
    private float maxHeight = 10.05f;

    private float minX = -48f;
    private float maxX = 53f;
    private float minZ = -48f;
    private float maxZ = 50f;

    public void Start()
    {
        scoreManager = GameObject.Find("_SCRIPTS_").GetComponent<ScoreManager>();
    }

    //replace Update method in your class with this one
    void FixedUpdate()
    {
        //if mouse button (left hand side) pressed instantiate a raycast
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (terrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                float x = hit.point.x;
                float z = hit.point.z;
                bool withinBounds = x > minX && x < maxX && z > minZ && z < maxZ;

                if (selectedTower != null && hit.point.y > minHeight && hit.point.y < maxHeight && withinBounds)
                {
                    float cost = selectedTower.GetComponent<Tower>().cost;

                    if (scoreManager.money < cost)
                    {
                        Debug.Log("Not enough money!");
                        return;
                    }
                    scoreManager.money -= cost;
                    GameObject towerGO = (GameObject)Instantiate(selectedTower, hit.point, Quaternion.Euler(0,0,0));
                    Tower tower = towerGO.GetComponent<Tower>();
                    tower.enemySpawners = enemySpawners;
                }
            }
        }
    }

    public void SelectTowerType(GameObject prefab)
    {
        selectedTower = prefab;
    }
}

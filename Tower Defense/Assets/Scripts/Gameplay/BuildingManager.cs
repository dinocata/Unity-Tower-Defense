using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public GameObject enemySpawners;
    public GameObject buildingTower;
    public GameObject selectedObject;
    public Camera mainCamera;
    public Terrain terrain;

    [System.Serializable]
    public class Selection
    {
        public GameObject selectionPanel;
        public Button upgradeBtn;
        public Text selectionName;
        public Text damageText;
        public Text rangeText;
        public Text fireRateText;
        public Text upgradeText;
    }
    public Selection selection;

    private ScoreManager scoreManager;

    private float minHeight = 9.95f;
    private float maxHeight = 10.05f;

    private float minX = -48f;
    private float maxX = 53f;
    private float minZ = -48f;
    private float maxZ = 50f;

    private float UIheight = 65f;

    private float iconTimer = 0.2f;
    private float iconTimerRemaining = 0;

    private List<Tower> towers = new List<Tower>();

    [System.Serializable]
    public class TowerBuilder
    {
        public Tower tower;
        public Text priceText;
    }

    public TowerBuilder[] towerBuilders;

    public void Start()
    {
        scoreManager = GameObject.Find("_SCRIPTS_").GetComponent<ScoreManager>();
        selection.selectionPanel.SetActive(false);
    }

    //replace Update method in your class with this one
    void FixedUpdate()
    {
        iconTimerRemaining -= Time.deltaTime;
        if (iconTimerRemaining <= 0)
        {
            iconTimerRemaining = iconTimer;
            foreach (TowerBuilder tb in towerBuilders)
            {
                if (tb.tower.cost > scoreManager.money)
                {
                    tb.priceText.color = new Color(1f, 0f, 0f);
                }else
                    tb.priceText.color = new Color(1f, 1f, 0);

                tb.priceText.text = "$" + tb.tower.cost;
            }

            if (selectedObject != null)
                updateSelectionValues(selectedObject.GetComponent<Tower>());
        }

        //if mouse button (left hand side) pressed instantiate a raycast
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > UIheight)
        {
            if (buildingTower != null)
            {
                //create a ray cast and set it to the mouses cursor position in game
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (terrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
                {
                    float x = hit.point.x;
                    float z = hit.point.z;
                    bool withinBounds = x > minX && x < maxX && z > minZ && z < maxZ;

                    if (hit.point.y > minHeight && hit.point.y < maxHeight && withinBounds)
                    {
                        Collider[] hitColliders = Physics.OverlapSphere(hit.point, 1f);

                        if (hitColliders.Length == 1 && hitColliders[0].name == "Terrain")
                        {
                            int cost = buildingTower.GetComponent<Tower>().cost;

                            scoreManager.money -= cost;
                            Tower tower = buildingTower.GetComponent<Tower>();
                            tower.enabled = true;
                            tower.enemySpawners = enemySpawners;
                            towers.Add(tower);
                            SetAllCollidersStatus(true);
                            buildingTower = null;
                            Cursor.visible = true;
                        }
                    }
                }
            }
            else
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.tag == "Tower")
                    {
                        selectedObject = hitInfo.transform.gameObject;
                        selection.selectionPanel.SetActive(true);
                        
                        Tower tower = selectedObject.GetComponent<Tower>(); 
                        updateSelectionValues(tower);
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (buildingTower != null)
                clearBuildingTower();
            else if (selectedObject != null)
                clearSelection();      
        }

        if (buildingTower != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (terrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                buildingTower.transform.position = hit.point;
            }
        }
    }

    private void clearSelection()
    {
        selectedObject = null;
        selection.selectionPanel.SetActive(false);
    }

    private void clearBuildingTower()
    {
        Destroy(buildingTower);
        buildingTower = null;
        Cursor.visible = true;
    }

    private void SetAllCollidersStatus (bool active) {
        foreach (Collider c in buildingTower.GetComponents<Collider>())
        {
             c.enabled = active;
         }
     }

    public void SelectTowerType(GameObject prefab)
    {
        if (buildingTower != null)
            clearBuildingTower();

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (terrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
        {
            buildingTower = (GameObject)Instantiate(prefab, hit.point, prefab.transform.rotation);
            Tower tower = buildingTower.GetComponent<Tower>();
            SetAllCollidersStatus(false);

            if (scoreManager.money >= tower.cost)
                tower.enabled = false;
            else
                clearBuildingTower();
        }
    }

    public void upgradeTower()
    {
        if (selectedObject != null)
        {
            Tower tower = selectedObject.GetComponent<Tower>();
            if (tower != null && scoreManager.money >= (int)tower.upgradeCost)
            {
                scoreManager.money -= (int)tower.upgradeCost;
                tower.upgrade();
                updateSelectionValues(tower);
            }
        }
    }

    private void updateSelectionValues(Tower tower)
    {
        int upgradeCost = (int)tower.upgradeCost;
        int damage = (int)tower.damage;

        selection.selectionName.text = tower.towerName + " (" + tower.upgradeLevel.ToString() + ")";
        selection.damageText.text = "Damage: " + tower.getMinDamage().ToString() + "-" + tower.getMaxDamage().ToString();
        selection.rangeText.text = "Range: " + tower.range.ToString();
        selection.fireRateText.text = "Fire Rate: " + tower.fireCooldown.ToString() + " sec";
        selection.upgradeText.text = "+Upgrade $" + upgradeCost.ToString();

        if (upgradeCost > scoreManager.money)
            selection.upgradeBtn.image.color = new Color(1f, 0f, 0f);
        else
            selection.upgradeBtn.image.color = new Color(161f, 255f, 0f);
    }
}

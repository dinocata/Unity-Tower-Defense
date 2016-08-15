using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    float spawnCD = 2f;
    float spawnCDremaining = 5;

    private int spawnerIndex = 0;
    private MainPath mainPath;
    private ScoreManager scoreManager;

    [System.Serializable]
    public class WaveComponent{
        public GameObject enemyPrefab;
        public int num;
        [System.NonSerialized]
        public int spawned = 0;
        [System.NonSerialized]
        public List<Enemy> enemies = new List<Enemy>();
    }

    public WaveComponent[] waveComps;

	// Use this for initialization
	void Start () {
        mainPath = GameObject.Find("Path").GetComponent<MainPath>();
        scoreManager = GameObject.Find("_SCRIPTS_").GetComponent<ScoreManager>();

        // Prespawn (to avoid fps hiccups on dynamic instantiates)
        foreach (WaveComponent wc in waveComps)
        {
            for (int i = 0; i < wc.num; i++)
            {
                // Spawn it
                GameObject enemyGO = (GameObject)Instantiate(wc.enemyPrefab, transform.position, transform.rotation);
                enemyGO.SetActive(false);
                Enemy enemy = enemyGO.GetComponent<Enemy>();
                enemy.mainPath = mainPath;
                enemy.scoreManager = scoreManager;
                wc.enemies.Add(enemy);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        bool didSpawn = false;
        spawnCDremaining -= Time.deltaTime;
        if (spawnCDremaining <= 0)
        {
            spawnCDremaining = spawnCD;

            // Go through the wave comps until we find something to spawn
            foreach (WaveComponent wc in waveComps)
            {
                if (wc.spawned < wc.num)
                {
                    // Spawn it
                    GameObject enemyGO = wc.enemies[wc.spawned].gameObject;
                    enemyGO.SetActive(true);
                    enemyGO.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
                    enemyGO.transform.GetChild(0).gameObject.SetActive(true);
                    wc.spawned++;
                    didSpawn = true;
                    break;
                }
            }
            if (!didSpawn)
            {
                spawnCDremaining = 5f;
                transform.parent.GetChild(spawnerIndex++).gameObject.SetActive(false);
                if (spawnerIndex >= transform.parent.childCount)
                {
                    spawnerIndex = 0;
                }
                transform.parent.GetChild(spawnerIndex).gameObject.SetActive(true);
            }
        }
	}
}

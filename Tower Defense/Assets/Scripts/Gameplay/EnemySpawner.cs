using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public float difficultyFactor = 1.25f;

    public float spawnCD = 5f;
    private float spawnCDremaining = 5f;
    private bool active = false;

    [System.NonSerialized]
    public MainPath mainPath;
    [System.NonSerialized]
    public ScoreManager scoreManager;

    [System.NonSerialized]
    public int wcIndex = 0;

    public class WaveComponent{
        public GameObject enemyPrefab;
        public int num;
        public float cd;
        [System.NonSerialized]
        public int spawned = 0;
    }

    public List<WaveComponent> waveComps = new List<WaveComponent>();

    public float getWaveCooldown()
    {
        float waveCooldown = 0f;
        foreach (WaveComponent wc in waveComps)
        {
            waveCooldown += (wc.cd * wc.num) + spawnCD;
        }
        return waveCooldown;
    }

	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            spawnCDremaining -= Time.deltaTime;
            if (spawnCDremaining <= 0)
            {
                WaveComponent wc = waveComps[wcIndex];
                GameObject enemyGO = (GameObject)Instantiate(wc.enemyPrefab, transform.position, transform.rotation);
                Enemy enemy = enemyGO.GetComponent<Enemy>();
                enemy.mainPath = mainPath;
                enemy.scoreManager = scoreManager;
                enemy.health *= Mathf.Pow(difficultyFactor, (scoreManager.level - 1));
                enemy.moneyValue *= scoreManager.level;
                wc.spawned++;

                spawnCDremaining = wc.cd;

                if (wc.spawned >= wc.num)
                {
                    wcIndex++;
                    spawnCDremaining += spawnCD;

                    if (wcIndex >= waveComps.Count)
                    {
                        foreach (WaveComponent waveComp in waveComps)
                            waveComp.spawned = 0;
                        wcIndex = 0;
                        active = false;    
                    }
                }
            }
        }
	}

    public void start(float delay) {
        active = true;
        spawnCDremaining = delay;
    }

    public bool getStatus() { return active; }
}

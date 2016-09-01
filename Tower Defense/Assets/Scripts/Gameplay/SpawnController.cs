using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {

    [System.NonSerialized]
    public const float basicTankOffset = 1f;
    [System.NonSerialized]
    public const float heavyTankOffset = 2.5f;

    private System.Random random = new System.Random();

    public float waveCD = 2.5f;
    public float waveCDremaining = 10;

    private float timer = 0;
    private int spawnerIndex = 0;

    private List<EnemySpawner> spawners = new List<EnemySpawner>();

    public MainPath mainPath;
    public ScoreManager scoreManager;

    public GameObject[] enemyTypePrefab;

    private bool init = true;

    private void generateWaveComponents()
    {
        int count = (int)(scoreManager.level * 0.5f) + 3;
        int spawnIndex = 3;

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.waveComps.Clear();

            for (int i = 0; i < count; i++)
            {
                EnemySpawner.WaveComponent wc = new EnemySpawner.WaveComponent();            

                int index = 0;

                if (scoreManager.level > 1 && random.Next(3) == 1)
                {
                    index = 1;
                    wc.num = (int)((scoreManager.level + spawnIndex) * 0.25f);
                    wc.cd = heavyTankOffset;
                }
                else
                {
                    wc.num = scoreManager.level + spawnIndex;
                    wc.cd = basicTankOffset;
                }

                wc.enemyPrefab = enemyTypePrefab[index];
   
                spawner.waveComps.Add(wc);
            }

            spawnIndex++;
        }
    }

	// Use this for initialization
	void Start () {
        mainPath = GameObject.Find("Path").GetComponent<MainPath>();
        scoreManager = GameObject.Find("_SCRIPTS_").GetComponent<ScoreManager>();

        foreach (Transform child in transform)
        {
            EnemySpawner spawner = child.GetComponent<EnemySpawner>();

            if (spawner.gameObject.activeSelf)
            {
                spawner.mainPath = mainPath;
                spawner.scoreManager = scoreManager;
                spawners.Add(spawner);
            }  
        }

        generateWaveComponents();

        int cdRemaining = (int)waveCDremaining - 1;
        TimeSpan time = TimeSpan.FromSeconds(cdRemaining);
        DateTime dateTime = DateTime.Today.Add(time);
        scoreManager.cooldownText.text = "Next wave: " + dateTime.ToString("mm:ss");   
	}

	// Update is called once per frame
	void Update () {
        waveCDremaining -= Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= 0.2f)
        {
            int cdRemaining = (int)waveCDremaining;
            TimeSpan time = TimeSpan.FromSeconds(cdRemaining);
            DateTime dateTime = DateTime.Today.Add(time);
            scoreManager.cooldownText.text = "Next wave: " + dateTime.ToString("mm:ss");
            timer = 0;
        }

        if (waveCDremaining <= 0)
        {
            if (init)
            {
                spawners[spawnerIndex].start(0);
                waveCDremaining = spawners[spawnerIndex].getWaveCooldown() + waveCD;
                init = false;

                if (spawners.Count == 1) waveCDremaining += scoreManager.levelCooldown;
            }
            else
            {
                spawnerIndex++;

                if (spawnerIndex < spawners.Count)
                {
                    waveCDremaining = spawners[spawnerIndex].getWaveCooldown() + waveCD;
                    spawners[spawnerIndex].start(0);

                    if (spawnerIndex == spawners.Count - 1) waveCDremaining += scoreManager.levelCooldown;
                }
                else
                {
                    spawnerIndex = 0;
                    scoreManager.level++;
                    generateWaveComponents();
                    scoreManager.levelText.text = "LEVEL " + scoreManager.level.ToString();
                    scoreManager.money += scoreManager.level * 10;
                    waveCDremaining = spawners[spawnerIndex].getWaveCooldown() + waveCD;
                    spawners[spawnerIndex].start(0);
                }
            }  
        }
	}
}

using System;
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour 
{
    public static WaveSpawner instance;

    public enum SpawnState { SPAWNING, WAITING, COUNTING, COMPLETE}

    public static SpawnState state = SpawnState.COUNTING;
    public static bool complete = false;

    public float timeBeforeFirstWave = 15f;

    public Transform[] spawnPoints;
    public Wave[] waves;

    public static int nextWave {get; private set;} = 0;
    public static float waveCountdown { get; private set; }

    private float searchCountdown = 3f; // How often the game checks if there are enemies left

    public static event Action WaveComplete = delegate {};
    public static event Action WaveStarting = delegate {};


    [System.Serializable]
    public class Wave 
    {
        public string name;
        // public Enemy enemy;
        // public int count;
        public Enemy[] enemies; // Different types of enemies spawning this wave
        public int[] counts; // Number of each type of enemy. Parallel to the enemies array
        public float rate;
        public float timeAfterWave = 15f;
    }


    void Awake() 
    {
        // Set up singleton behavior
        instance = this;

        if (spawnPoints.Length == 0)
        {
            Debug.Log("Error: No spawn points referenced.");
        }

        waveCountdown = timeBeforeFirstWave;
    }


    void Update() 
    {
        if (state == SpawnState.WAITING)
        {
            // Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                // Begin a new round
                WaveCompleted();
            }
            else 
            {
                return;
            }
        }

        if (waveCountdown <= 0) 
        {
            if (state != SpawnState.SPAWNING) 
            {
                // Start spawning wave.
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } 
        else if (state != SpawnState.COMPLETE)
        {
            waveCountdown -= Time.deltaTime;
        }
    }


    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        // waveCountdown = timeBetweenWaves;
        waveCountdown = waves[nextWave].timeAfterWave;

        // All Waves Completed
        if (nextWave + 1 > waves.Length - 1) 
        {
            // Debug.Log("All Waves Complete! Looping...");
            // nextWave = 0;
            // Go into complete state
            Debug.Log("All Waves Complete!");
            complete = true;
            state = SpawnState.COMPLETE;
            GameManager.isVictory = true;
            // GameManager.instance.EndGame();
        }
        else 
        {
            nextWave++;
        }

        WaveComplete();
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 3f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }


    IEnumerator SpawnWave(Wave _wave) 
    {
        Debug.Log("Starting " + _wave.name);
        state = SpawnState.SPAWNING;
        WaveStarting();

        // Spawn
        // for(int i = 0; i< _wave.count * GameStats.difficulty; i++) 
        for (int i = 0; i < _wave.enemies.Length; i++) // for each type of enemy
        // foreach ( Enemy enemy in _wave.enemies)
        {
            for (int j = 0; j < _wave.counts[i] * GameStats.difficulty; j++) // for each enemy of that type
            {
                // SpawnEnemy(_wave.enemy);
                SpawnEnemy(_wave.enemies[i]);
                yield return new WaitForSeconds(1f/_wave.rate);
            }
        }

        state = SpawnState.WAITING;

        yield break;
    }


    void SpawnEnemy(Enemy _enemy)
    {
        if (GameManager.playerRef.GetComponent<Creature>().health > 0)
        {
            // Choose a random spawn point to spawn the enemy at.
            Transform _sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            // Spawn enemy
            Enemy enemy = Instantiate<Enemy>(_enemy, _sp.position, _sp.rotation);
            enemy.startingPoint = _sp;
        }
    }
}
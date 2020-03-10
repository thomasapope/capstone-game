using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour 
{
    public Transform player;
    public enum SpawnState { SPAWNING, WAITING, COUNTING, COMPLETE}

    

    public Wave[] waves;
    // private int nextWave = 0;
    public static int nextWave {get; private set;} = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 15f;
    public static float waveCountdown { get; private set; }

    private float searchCountdown = 1f;

    public static SpawnState state = SpawnState.COUNTING;

    public static bool complete = false;


    [System.Serializable]
    public class Wave 
    {
        public string name;
        public Enemy enemy;
        public int count;
        public float rate;
        public float timeAfterWave = 15f;
    }


    void Start() 
    {
        // Check if there are spawn points
        if (spawnPoints.Length == 0)
        {
            Debug.Log("Error: No spawn points referenced.");
        }

        waveCountdown = timeBetweenWaves;
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
        Debug.Log("Wave Completed");

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
        }
        else 
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }


    IEnumerator SpawnWave(Wave _wave) 
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        // Spawn
        for(int i = 0; i< _wave.count; i++) 
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }


    void SpawnEnemy(Enemy _enemy)
    {
        // Choose a random spawn point to spawn the enemy at.
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn enemy
        // Debug.Log("Spawning Enemy: " + _enemy.name);

        Enemy enemy = Instantiate<Enemy>(_enemy, _sp.position, _sp.rotation);
        enemy.startingPoint = _sp;
        enemy.target = player;
        enemy.transform.rotation = Quaternion.LookRotation(new Vector3 (player.position.x, 0, player.position.z));
    }

}
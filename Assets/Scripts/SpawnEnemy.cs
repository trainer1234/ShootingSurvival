using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {
    GameObject[] spawnPoint;
    public GameObject zombie;

    public float minSpawnTime = 0.2f;
    public float maxSpawnTime = 1;
    public float lastSpawnTime = 0;
    public float spawnTime = 0;

	// Use this for initialization
	void Start () {
        spawnPoint = GameObject.FindGameObjectsWithTag("Respawn");
        UpdateSpawnTime();
	}

    void UpdateSpawnTime()
    {
        lastSpawnTime = Time.time;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Spawn()
    {
        int point = Random.Range(0, spawnPoint.Length);
        // no rotation
        Instantiate(zombie, spawnPoint[point].transform.position, Quaternion.identity);
        UpdateSpawnTime();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= (lastSpawnTime + spawnTime))
        {
            Spawn();
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject[] Vehicles;
    public GameObject ExplosionObject;

    [Header("SpawnPoints")]
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] int SpawnNumber;

    [Header("SpawnTime")]
    [SerializeField] float TimeUntilSpawn;
    [SerializeField] float MinimalTime;
    [SerializeField] float MaximalTime;

    // Start is called before the first frame update
    void Start()
    {
        TimeUntilSpawn = Random.Range(MinimalTime, MaximalTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SpawnNumber = Random.Range(0, 6);
        TimeUntilSpawn -= 1 * Time.deltaTime;
        if(TimeUntilSpawn <= 0)
        {
            SpawnVehicle();
        }
        
    }

    void SpawnVehicle()
    {

        Instantiate(Vehicles[Random.Range(0, 3)], SpawnPoints[SpawnNumber].position, SpawnPoints[SpawnNumber].rotation);
        TimeUntilSpawn = Random.Range(MinimalTime, MaximalTime);
    }
}

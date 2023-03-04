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
    //[SerializeField] int SpawnNumber2;

    [Header("SpawnTime")]
    [SerializeField] float TimeUntilSpawn;
    [SerializeField] float MinimalTime;
    [SerializeField] float MaximalTime;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnNumber2 = Random.Range(0, 6);
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
        //SpawnNumber2 = Random.Range(0, 6);
        //Instantiate(Vehicles[Random.Range(0, 3)], SpawnPoints[SpawnNumber2].position, SpawnPoints[SpawnNumber2].rotation);
        TimeUntilSpawn = Random.Range(MinimalTime, MaximalTime);
    }
}

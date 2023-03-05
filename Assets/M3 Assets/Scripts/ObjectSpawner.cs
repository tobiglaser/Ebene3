using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject[] Vehicles;
    public GameObject ExplosionObject;

    [Header("Lanes")]
    [SerializeField] Lane[] Lanes;

    [Header("SpawnTime")]
    [SerializeField] int numRetries;
    [SerializeField] float SafeZoneTowards;
    [SerializeField] float SafeZoneAway;
    [SerializeField] float MinimalTime;
    [SerializeField] float MaximalTime;
    private float TimeUntilSpawn;

    // Start is called before the first frame update
    void Start()
    {
        TimeUntilSpawn = Random.Range(MinimalTime, MaximalTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeUntilSpawn -= 1 * Time.deltaTime;
        if (TimeUntilSpawn <= 0)
        {
            SpawnVehicle();
        }

    }

    void SpawnVehicle()
    {
        Transform spawn = determineSafeSpawnPoint();
        if (spawn != null)
        {
            Instantiate(Vehicles[Random.Range(0, Vehicles.Length)], spawn.position, spawn.rotation);
        }
        TimeUntilSpawn = Random.Range(MinimalTime, MaximalTime);
    }

    Transform determineSafeSpawnPoint()
    {
        Transform SpawnPoint = null;
        for (int i = 0; i < numRetries; i++)
        {
            int laneNum = Random.Range(0, Lanes.Length);
            int side = Random.Range(0, 2);
            Vector3 dir;
            Vector3 pos;
            if (side == 0)
            {
                SpawnPoint = Lanes[laneNum].LeftSpawn;
                pos = SpawnPoint.position;
                dir = Vector3.Normalize(Lanes[laneNum].RightSpawn.position - pos);
            }
            else
            {
                SpawnPoint = Lanes[laneNum].RightSpawn;
                pos = SpawnPoint.position;
                dir = Vector3.Normalize(Lanes[laneNum].LeftSpawn.position - pos);
            }

            Ray ray = new Ray(pos, dir);
            RaycastHit hit;
            bool doesCollide = Physics.Raycast(ray, out hit, SafeZoneTowards);
            if (!doesCollide)
            {
                return SpawnPoint;
            }
            else
            {
                if (hit.transform.rotation.y * dir.x >= 0)
                {
                    //gleiche Rightung -> Away -> weniger Abstand -> nochmal Raycasten
                    if(!Physics.Raycast(ray, out hit, SafeZoneAway))
                    {
                        return SpawnPoint;
                    }
                }
                else
                {
                    // negativ -> Towards -> größerer Abstand -> kein SpawnPunkt -> nochmal
                }
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Vehicle : MonoBehaviour
{
    [SerializeField] protected string Name;
    [SerializeField] protected GameManager Manager;
    [SerializeField] protected ObjectSpawner Spawner;

    [SerializeField] protected enum States
    {
        Alive,
        Destroyed,
    }
    [Header("States")]
    [SerializeField] protected States CurrentState = States.Alive;

    [Header("Movement")]
    [SerializeField] protected Transform VehicleTransform;
    [SerializeField] protected Rigidbody VehicleBody;
    [SerializeField] protected int Speed;

    [Header("Destruction")]
    [SerializeField] protected float DespawnTime;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = Name;
        Manager = FindObjectOfType<GameManager>();
        Spawner = FindObjectOfType<ObjectSpawner>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SwitchStates();
    }

    protected void SwitchStates()
    {
        switch (CurrentState)
        {
            case States.Alive:
                IsAlive();
                break;
            case States.Destroyed:
                IsDestroyed();
                break;
            default:
                break;
        }
    }

    protected void IsAlive()
    {
        VehicleTransform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
    protected void IsDestroyed()
    {
        DespawnTime -= 1 * Time.deltaTime;
        if(DespawnTime <= 0)
        {
            Instantiate(Spawner.ExplosionObject, VehicleTransform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    protected void GetDestroyed()
    {
        if(CurrentState == States.Alive)
        {
            if(Manager.CurrentLives > 0)
            {
                Manager.CurrentLives -= 1;
            }
            CurrentState = States.Destroyed;
        }
        Instantiate(Spawner.ExplosionObject, VehicleTransform.position, Quaternion.identity);
        VehicleBody.AddExplosionForce(VehicleBody.mass * 5, VehicleTransform.position, VehicleBody.mass * 2.5f, VehicleBody.mass * 5.5f, ForceMode.Impulse);
    }
}

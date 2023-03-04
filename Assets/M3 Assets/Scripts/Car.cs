using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : AI_Vehicle
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Train")
        {
            IsDestroyed();
        }
        if(collision.gameObject.name == "Car")
        {
            IsDestroyed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            Destroy(this.gameObject);
        }
    }
}

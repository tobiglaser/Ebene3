using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canoe : AI_Vehicle
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Train")
        {
            GetDestroyed();
        }
        if(collision.gameObject.name == "Car")
        {
            GetDestroyed();
        }
        if (collision.gameObject.name == "Canoe")
        {
            GetDestroyed();
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

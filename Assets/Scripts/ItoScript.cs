using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItoScript : MonoBehaviour
{
    SpyderScript spyderScript;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        spyderScript = GetComponentInParent<SpyderScript>();

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spyderScript.KumoGravity();

        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            spyderScript.KumoGravity();

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spyderScript.KumoCrym();

        }

    }
}

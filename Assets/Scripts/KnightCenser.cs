using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCenser : MonoBehaviour
{
    KnightScript KS;

    Collider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();

        KS = GetComponentInParent<KnightScript>();
    }

    void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
        {
            KS.KnightPreparation();
        }
    }

    void OnCollision(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            KS.KnightPreparation();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            KS.KnightIdle();
        }
    }
}

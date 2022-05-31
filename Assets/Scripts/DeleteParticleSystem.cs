using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteParticleSystem : MonoBehaviour
{
    ParticleSystem partSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!partSystem.isPlaying)
        {
            GameObject objToDestroy;
            
            if (this.transform.parent != null)
            {
                objToDestroy = this.transform.parent.gameObject;
            }
            else
            {
                objToDestroy = this.gameObject;
            }
            
            Destroy(objToDestroy, 2.0f);
        }
    }
}
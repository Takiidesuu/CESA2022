using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPlatforms : MonoBehaviour
{
    private Collider col;
    Rigidbody playerRb;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        
        col.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y > 0.0f)
        {
            col.isTrigger = true;
        }
        else
        {
            col.isTrigger = false;
        }
    }
    
    /* IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.1f);
        
        col.isTrigger = false;
    } */
    
    /* private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Rigidbody>().velocity.y >= 0.0f)
            {
                StartCoroutine("CountDown");
            }
        }
    }
    
    private void OnCollisionExit(Collision other) 
    {
        if (other.gameObject.tag == "Player")
        {
            col.isTrigger = true;
        }
    } */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeBlockFall : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    
    public bool hold = true;
    
    public void SetNotHolding()
    {
        Debug.Log("BlockNot");
        hold = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        hold = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hold && this.rb.useGravity == false)
        {
            rb.constraints = RigidbodyConstraints.None;
            
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            
            Debug.Log("Release");

            rb.useGravity = true;
        }
        else
        {
            
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Tape")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            
            hold = true;
        }
    }
}

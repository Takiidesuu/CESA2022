using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider col;
    
    [Header("落ちる速度")]
    [SerializeField] float speed = 2.0f;
    
    private bool hold = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        
        col.isTrigger = true;
        col.size = new Vector3(col.size.x, col.size.y * 2.0f, col.size.z);
        col.center = new Vector3(col.center.x, col.center.y - (col.size.y / 2.0f), col.center.z);
        
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hold)
        {
            rb.AddForce(Physics.gravity * speed * rb.mass);
        }
        else
        {}
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            hold = false;
            col.isTrigger = false;
        }
    }
}

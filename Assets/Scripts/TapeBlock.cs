using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeBlock : MonoBehaviour
{
    [Header("出てくる速度")]
    [SerializeField] float speed = 5.0f;
    
    Rigidbody rb;
    Collider col;
    
    Vector3 direction;
    Vector3 targetPos;
    
    GameObject tape;
    
    bool hold = true;
    
    bool playMusic = true;
    
    public void SetNotHolding()
    {
        hold = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        tape = this.transform.parent.GetChild(0).gameObject;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        col.isTrigger = true;
        
        targetPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z - 1.8f);
        
        hold = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hold)
        {
            rb.constraints = RigidbodyConstraints.None;
            
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            
            col.isTrigger = false;
            
            if (this.transform.position != targetPos)
            {
                Vector3 vel = direction / 5.0f * speed;
                
                //rb.AddForce(vel, ForceMode.Impulse);
                
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, speed / 100.0f);
                
                if (playMusic)
                {
                    GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayBlockSE();
                    playMusic = false;
                }
            }
            else
            {
                Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
                rb.velocity = vel;
                
                this.transform.position = targetPos;
            }
        }
        else
        {
            
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Tape")
        {
            hold = true;
        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Tape")
        {
            hold = false;
        }
    }
}

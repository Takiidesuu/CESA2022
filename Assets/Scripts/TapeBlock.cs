using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeBlock : MonoBehaviour
{
    [Header("出てくる速度")]
    [SerializeField] float speed = 5.0f;
    
    Rigidbody rb;
    Collider col;
    
    Vector3 vel;
    
    float targetPos;
    
    GameObject tape;
    
    bool hold = true;
    bool moveObj = true;
    
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
        
        targetPos = this.transform.transform.position.z - 3.3f;
        
        vel = new Vector3(0.0f, 0.0f, -speed);
        
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
            
            if (this.transform.position.z > targetPos && moveObj)
            {
                moveObj = true;
            }
            else
            {
                moveObj = false;
            }
            
            if (moveObj)
            {
                rb.velocity = vel;
                
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
                
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, targetPos);
            }
        }
        else
        {
            
        }
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Player" && moveObj)
        {
            if (other.gameObject.transform.position.z < this.transform.position.z)
            {
                vel = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }
    
    private void OnCollisionExit(Collision other) 
    {
        if (other.gameObject.tag == "Player" && moveObj)
        {
            vel = new Vector3(0.0f, 0.0f, -speed);
        }
    }
}

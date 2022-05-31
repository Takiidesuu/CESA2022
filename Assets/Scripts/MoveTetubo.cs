using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTetubo : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float waitTime = 3.0f;
    
    [Header("??????")]
    [SerializeField] float force = 5.0f;
    float maxspeed ;
    float startspeed;

    GameObject platform, objA, objB;
    Vector3 posA, posB, velocity;
    Rigidbody rb;
    
    private bool playSE = true;
    
    private bool move = true;

    bool moveForward = true;

    private void Start()
    {
        platform = transform.GetChild(0).gameObject;
        rb = platform.GetComponent<Rigidbody>();
        objA = transform.GetChild(1).gameObject;
        objB = transform.GetChild(2).gameObject;

        velocity = new Vector3(0, speed, 0);
        maxspeed = speed * 10;
        startspeed = speed;
    }

    Vector3 dest;
    
    void FixedUpdate()
    {
        posA = objA.transform.position;
        posB = objB.transform.position;

        if (moveForward)
        {
            dest = posA;
            speed += force;

            if (speed>maxspeed)
            {
                speed = maxspeed;
            }
        }
        else
        {
            dest = posB;
            speed -= speed;
            if (speed < startspeed)
            {
                speed = startspeed;
            }
        }

        if (((platform.transform.position == posA && moveForward) || (platform.transform.position == posB && !moveForward)) && move)
        {
            StartCoroutine("HoldPosition");
            move = false;
        }

        if (move)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, dest, speed * Time.fixedDeltaTime);
            
            if (playSE)
            {
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayBarSE();
                playSE = false;
            }
        }
        else
        {
            playSE = true;
        }
    }

    IEnumerator HoldPosition()
    {
        yield return new WaitForSeconds(waitTime);

        moveForward = !moveForward;
        move = true;
    }
    
    public void StopMove()
    {
        StartCoroutine("HoldPosition");
        move = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "TapeBlock")
        {
            StartCoroutine("HoldPosition");
            move = false;
        }

        if (other.gameObject.tag == "Player")
        {
            if (this.transform.eulerAngles.z == 0.0f || this.transform.eulerAngles.z == 180.0f)
            {
                float dir;
                if (other.gameObject.transform.position.x < this.transform.position.x)
                {
                    dir = -1.0f;
                }
                else if (other.gameObject.transform.position.x > this.transform.position.x)
                {
                    dir = 1.0f;
                }
                else
                {
                    dir = Random.Range(-1.0f, 1.0f);
                }

                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + (Mathf.Sign(dir) * 1.0f), other.gameObject.transform.position.y, 0.0f);
            }
        }
    }
}

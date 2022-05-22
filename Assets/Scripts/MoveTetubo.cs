using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTetubo : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float waitTime = 3.0f;

    GameObject platform, objA, objB;
    Vector3 posA, posB, velocity;
    Rigidbody rb;

    private bool move = true;

    bool moveForward = true;

    private void Start()
    {
        platform = transform.GetChild(0).gameObject;
        rb = platform.GetComponent<Rigidbody>();
        objA = transform.GetChild(1).gameObject;
        objB = transform.GetChild(2).gameObject;

        velocity = new Vector3(0, speed, 0);
    }

    void FixedUpdate()
    {
        Vector3 dest;
        
        posA = objA.transform.position;
        posB = objB.transform.position;

        if (moveForward)
        {
            dest = posA;
            
        }
        else
        {
            dest = posB;
            
        }

        if (((platform.transform.position == posA && moveForward) || (platform.transform.position == posB && !moveForward)) && move)
        {
            StartCoroutine("HoldPosition");
            move = false;
        }

        if (move)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, dest, speed * Time.fixedDeltaTime);
        }
    }

    IEnumerator HoldPosition()
    {
        yield return new WaitForSeconds(waitTime);

        moveForward = !moveForward;
        move = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "TapeBlock")
        {
            if (this.transform.eulerAngles.z == 0.0f || this.transform.eulerAngles.z == 180.0f)
            {
                if (platform.transform.position.y >= other.transform.position.y)
                {                
                    objA.transform.position = new Vector3(objA.transform.position.x, other.transform.position.y + 1.0f + platform.GetComponent<Renderer>().bounds.size.y / 2.0f, objA.transform.position.z);
                }
                else
                {
                    objA.transform.position = new Vector3(objA.transform.position.x, other.transform.position.y - 1.0f - platform.GetComponent<Renderer>().bounds.size.y / 2.0f, objA.transform.position.z);
                }
            }
            else
            {
                if (platform.transform.position.x >= other.transform.position.x)
                {                
                    objA.transform.position = new Vector3(other.transform.position.x + 1.0f + platform.GetComponent<Renderer>().bounds.size.x / 2.0f, objA.transform.position.y, objA.transform.position.z);
                }
                else
                {
                    objA.transform.position = new Vector3(other.transform.position.x - 1.0f - platform.GetComponent<Renderer>().bounds.size.x / 2.0f , objA.transform.position.y, objA.transform.position.z);
                }
            }
        }
    }
}

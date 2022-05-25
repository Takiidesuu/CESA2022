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
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayBarSE();
        }
        else
        {
            
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
            StartCoroutine("HoldPosition");
            move = false;
        }
    }
}

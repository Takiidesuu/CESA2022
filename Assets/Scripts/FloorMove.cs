using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float waitTime = 3.0f;
    
    GameObject platform;
    Vector3 posA, posB, velocity;
    
    private bool move = true;
    
    bool moveForward = true;
    
    private void Start() 
    {
        platform = transform.GetChild(0).gameObject;
        
        posA = transform.GetChild(1).gameObject.transform.position;
        posB = transform.GetChild(2).gameObject.transform.position;
        
        velocity = new Vector3(0, speed, 0);
    }

    void FixedUpdate()
    {
        Vector3 dest;
        
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour
{

    private Transform target;

    private GameObject Knight;

    private Vector3 StartPos;



    private float LeftAngle = 90.0f;
    private float RightAngle = -90.0f;

    private float speed = 5.0f;


    //MovePlayer movePlayer;

    private float Distance = 3.0f;

    Rigidbody rb;

    private BoxCollider boxcol;

    Collider col;

    private Animator anim;

    private Vector3 DownPos;

    private Quaternion DownRotate;


    void Start()
    {
        target = GameObject.Find("NeelGicGrab").transform;

        Knight = GameObject.FindWithTag("soad");
 

        StartPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);


        col = GetComponent<Collider>();

        rb = GetComponent<Rigidbody>();

        anim = GetComponentInChildren<Animator>();

        anim.SetBool("Idle", true);


        //DownPos = new Vector3(this.transform.position.x - 2.5f, this.transform.position.y, this.transform.position.z * 3.0f);

        //DownRotate = transform.rotation;

        //DownRotate = Quaternion.Euler(52.511f, 6.84f, -78.288f);
    }

    void Update()
    {
        //if(movePlayer.GetStoneNum() > 0)
        //{

        //}


        //step = speed * Time.deltaTime;

      

        //float distance = Vector3.Distance(transform.position, target.position);

        //if (distance < 5.0f && distance > 1.5f)
        //{
        //this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0.0f, RightAngle, 0.0f), step);

        //}




        //float distance = (this.transform.position - target.transform.position).magnitude;

        //if (this.transform.position.x * -1.0f >= target.position.z)
        //{

        //}


    }

    public void KnightIdle()
    {
        anim.SetBool("Idle", true);
        anim.SetBool("AttackPreparation", false);

    }

    public void KnightPreparation()
    {
        anim.SetBool("AttackPreparation",true);
        anim.SetBool("Idle", false);
    }

    public void KnightAttack()
    {
        anim.SetBool("Attack", true);
        anim.SetBool("AttackPreparation", false);
    }

    public void KnightNotAttack()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", true);
    }

    public void KnightRotate()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("AttackPreparation", false);
        anim.SetBool("Idle", true);

        //float velocity02 = 2.2f;
        //this.transform.position = Vector3.MoveTowards(transform.position, DownPos, speed * Time.deltaTime / (3.6f - (1.0f * velocity02)));
        col.isTrigger = true;
        this.transform.rotation = Quaternion.Euler(0.0f, RightAngle, 0.0f);
    }


    //public void KnightDest()
    //{
    //    col.isTrigger = false;
    //}


}

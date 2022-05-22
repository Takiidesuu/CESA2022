using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpyderScript : MonoBehaviour
{

    [SerializeField]
    [Tooltip("クモの落下速度")]
    float fallspeed = 200.0f;

    [SerializeField]
    [Tooltip("クモがあがる速度")]
    float crymspeed = 200.0f;

    //リジッドボディー
    Rigidbody rb;

    //コライダー
    Collider col;

    //アニメーション
    private Animator anim;

    //クモの初期位置
    private Vector3 StartPos;

    //プレイヤーがクモのいとに引っ掛けたかどうかの判定
    //bool touch = false;




    void Start()
    {
        //オブジェクトのColliderコンポーネントを取得
        col = GetComponent<Collider>();

        //オブジェクトのColliderコンポーネントを取得
        rb = GetComponent<Rigidbody>();

        //オブジェクトのAnimatorコンポーネントを取得
        anim = GetComponentInChildren<Animator>();

        //初期位置の初期化
        StartPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z);

    }

    void FixedUpdate()
    {
        //if (touch == true)
        //{
        //Vector3 force = new Vector3(0.0f, -0.5f * fallspeed, 0.0f);

        //rb.AddForce(force, ForceMode.Force);


        //} 
        //if(touch == false)
        //{
        //    Vector3 force = new Vector3(0.0f, 0.5f * crymspeed, 0.0f);



        //    if (rb.velocity.y > 5.0f)
        //    {
        //        this.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z);

        //    }

        //}


    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "player")
    //    {
    //        //Vector3 force = new Vector3(0.0f, -0.5f * fallspeed, 0.0f);

    //        //rb.AddForce(force, ForceMode.Force);
    //        //touch = true;
    //    }
    //}

    

    public void KumoGravity()
    {
        Debug.Log("落ちてるよ！");
     
        Vector3 force = new Vector3(0.0f, -1.0f * fallspeed, 0.0f);

        rb.AddForce(force, ForceMode.Force);

        anim.SetBool("Falling", true);

        if (gameObject.tag == "Ground")
        {
            rb.velocity = Vector3.zero;
        }
        
        //rb.useGravity = true;
    }

    public void KumoCrym()
    {
        Debug.Log("上がって");

        Vector3 tikara = new Vector3(0.0f, 1.0f * crymspeed, 0.0f);

        rb.AddForce(tikara, ForceMode.Force);
        if (rb.velocity.y > 3.5f)
        {
            this.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z);
        }
        anim.SetBool("Shaking", false);
        anim.SetBool("Falling", true);



        //rb.useGravity = false;
    }
        

    //public void KumoShake()
    //{
    //    Debug.Log("揺れてる");
        
    //    anim.SetBool("Shaking", true);
    //    anim.SetBool("Falling", false);

    //    //クモを止まらせる
    //    rb.velocity = Vector3.zero;
    //}

    public void SpyderDest()
    {
        Destroy(this.gameObject);
    }
    //void OnTriggerEnter(Collider other)
    //{

    //    }
    //}



}


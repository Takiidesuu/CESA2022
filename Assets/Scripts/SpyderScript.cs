using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpyderScript : MonoBehaviour
{
    [Header("落ちる速度")]
    [SerializeField] float Fallspeed = 4.0f;
    [Header("あがる速度")]
    [SerializeField] float Climbspeed = 2.0f;
    //リジッドボディー
    Rigidbody rb;

    //コライダー
    Collider col;

    //アニメーション
    private Animator anim;

    private GameObject Spyder;

    //クモの初期位置
    private Vector3 StartPos;

    private Vector3 TargetPos;
    private Vector3 TargetPos2;

    private bool Falling = false;

    //private float Maxheight = 5.0f;
    //private float Minheight = -5.0f;

    //private float velocity = 0.0f;

    private bool Otiru = false;

    private float speed = 2.0f;

    private bool climb = false;


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

        Spyder = GameObject.FindWithTag("spyder");
        //初期位置の初期化
        StartPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y  , this.transform.transform.position.z);

        TargetPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 2.0f, this.gameObject.transform.position.z);
    }

    void FixedUpdate()
    {
        if (climb == true)
        {
            anim.SetBool("Falling", false);
            rb.velocity = Vector3.up * Climbspeed;
        }
        else
        {
            
        }

        if (this.transform.position.y >= StartPos.y)
        {
            this.transform.position = new Vector3(this.transform.position.x, StartPos.y, this.transform.position.z);
        }
    }



    

    public void KumoGravity()
    {
        climb = false;
        if (this.transform.position.y > TargetPos.y)
        {
            anim.SetBool("Falling", true);
            rb.velocity = Vector3.down * Fallspeed;
            //rb.velocity = new Vector3(0.0f, -1.0f * speed, 0.0f);
        }
        else
        {

            rb.velocity = Vector3.zero;
        }
        
        //else
        //{
        //    Debug.Log("落ち切った");
        //    rb.velocity = Vector3.zero;
        //}




    }

    public void KumoClimb()
    {
        climb = true;

            //rb.useGravity = false;
            //Vector3 tikara = new Vector3(0.0f, 1.0f * crymspeed, 0.0f);

            //rb.AddForce(tikara, ForceMode.Force);








    }

   
    public void SpyderDest()
    {
        Destroy(this.gameObject);
    }
 


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DouzouScript : MonoBehaviour
{
    //カウントダウンする方式
    [SerializeField] float WaitTime = 1.0f;

    //銅像の移動速度
    [SerializeField] float moveSpeed = 1.0f;

    //プレイヤーTransformコンポーネントを格納する変数
    private Transform target;

    private Quaternion defaultPos;

    MovePlayer moveplayer;

    Rigidbody rb;

    //銅像の移動速度
    //private float moveSpeed = 2.0f;

    //銅像が停止するプレイヤーとの距離を格納する変数
    private float StopDistance = 1.5f;

    //銅像がプレイヤーに向かって移動を開始する距離を格納する変数
    private float moveDistance = 5.0f;

    private bool Freeze;
    private bool DouzouMove;

    Animator anim;

    private void Start()
    {
        // targetにPlayerのタグを入れる
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        moveplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayer>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        DouzouMove = false;
        Freeze = false;

        defaultPos = this.transform.rotation;
        defaultPos = Quaternion.Euler(0.0f, -180.0f, 0.0f);


    }

    void FixedUpdate()
    {
        if (moveplayer.GetStoneNum() > 0)
        {
            anim.SetBool("Kidou", true);
            anim.SetBool("ShtoDown", false);

            WaitTime += 1.0f;
            if (WaitTime >= 120.0f)
            {
                anim.SetBool("Kidou", false);
                anim.SetBool("Stand", true);
                DouzouMove = true;

            }

            if (DouzouMove == true)
            {

                //変数targetPosを作成してプレイヤーの座標を格納
                Vector3 targetPos = target.position;
                Quaternion targetRotate = target.rotation;

                //銅像自身のY座標を変数targetのY座標に格納
                //(プレイヤーのX,Z座標のみ参照)
                targetPos.y = transform.position.y;
                //targetPos.x = transform.position.x;

                //transform.LookAt(targetPos);

                //変数distanceを作成して銅像の位置とプレイヤーの距離を格納
                float distance = Vector3.Distance(transform.position, target.position);

                // 銅像とプレイヤーの距離判定
                // 変数 distance（プレイヤーと銅像の距離）が変数 moveDistance の値より小さければ
                // さらに変数 distance が変数 stopDistance の値よりも大きい場合
                if (distance < moveDistance && distance > StopDistance)
                {
                    //銅像を変数targetPosの座標方向に向かせる
                    //transform.LookAt(targetPos);


                    Quaternion targetRotation = Quaternion.LookRotation(targetPos - this.transform.position);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation ,targetRotation, Time.deltaTime);

                    //変数moveSpeedを乗算した速度で銅像を前方向に移動する
                    //transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.position.x,this.transform.position.y, this.transform.position.z), Time.deltaTime * moveSpeed);
                    //anim.SetBool("Stand", false);
                    anim.SetBool("Move", true);
                                       
                }
                else if (distance >= 5.0f)
                {
                    //Debug.Log("5メートル離れたよ");
                    this.transform.rotation = Quaternion.Lerp(transform.rotation, defaultPos, Time.deltaTime);
                    //this.transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
                        anim.SetBool("Move", false);
                        anim.SetBool("Stand", true);                   
                }

                
            }
          
            DouzouMove = false;


            
        }
        else if(moveplayer.GetStoneNum() == 0)
        {
            this.transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0.0f);
            anim.SetBool("Stand", false);
            anim.SetBool("Move", false);
            anim.SetBool("ShtoDown", true);
            anim.SetBool("Kidou", false);
            WaitTime = 1.0f;
        }
    }

    public void DouzouDest()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        //if(col.gameObject.tag == "")
       

    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints.None;
            Freeze = true;
            if (Freeze == true)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezePositionZ
                | RigidbodyConstraints.FreezeRotation;
            }
            Freeze = false;


        }

    }

    void DouzouFreeze()
    {
        //if(Freeze == true)
        //{
        //    rb.constraints = RigidbodyConstraints.FreezePositionY;
        //    rb.constraints = RigidbodyConstraints.FreezePositionZ;
        //    rb.constraints = RigidbodyConstraints.FreezeRotation;
        //}
        
       
        
    }

}
//    [SerializeField]
//    [Tooltip("銅像の追跡速度")]
//    float targetspeed = 100.0f;

//    //リジッドボディー
//    Rigidbody rb;

//    //コライダー
//    Collider col;

//    //銅像の初期位置
//    private Vector3 StartPos;

//    //銅像の初期の向き
//    private Vector3 StartRotate;

//    //private Vector3 Ppos;




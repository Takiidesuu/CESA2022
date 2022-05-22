using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DouzouScript : MonoBehaviour
{   
    //プレイヤーTransformコンポーネントを格納する変数
    public Transform target;

    //銅像の移動速度
    private float moveSpeed = 2.0f;

    //銅像が停止するプレイヤーとの距離を格納する変数
    private float StopDistance = 1.5f;

    //銅像がプレイヤーに向かって移動を開始する距離を格納する変数
    private float moveDistance = 5.0f;

    void Update()
    {
        //変数targetPosを作成してプレイヤーにの座標を格納
        Vector3 targetPos = target.position;

        //銅像自身のY座標を変数targetのY座標に格納
        //(プレイヤーのX,Z座標のみ参照)
        targetPos.y = transform.position.y;
        //銅像を変数targetPosの座標方向に向かせる
        transform.LookAt(targetPos);

        //変数distanceを作成して銅像の位置とプレイヤーの距離を格納
        float distance = Vector3.Distance(transform.position, target.position);

        // 銅像とプレイヤーの距離判定
        // 変数 distance（プレイヤーと銅像の距離）が変数 moveDistance の値より小さければ
        // さらに変数 distance が変数 stopDistance の値よりも大きい場合
        if (distance < moveDistance && distance > StopDistance)
        {
            //変数moveSpeedを乗算した速度で銅像を前方向に移動する
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }
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



//    // Start is called before the first frame update
//    void Start()
//    {
//        //オブジェクトのColliderコンポーネントを取得
//        col = GetComponent<Collider>();

//        //オブジェクトのColliderコンポーネントを取得
//        rb = GetComponent<Rigidbody>();

//        //初期位置の初期化
//        StartPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z);

//        //初期の向きの初期化
//        StartRotate = new Vector3(0.0f, 0.0f, 0.0f);

//        //Ppos = GameObject.FindGameObjectWithTag("Player").transform.position;
//    }

//    //センサーに侵入した場合
//    public void StatuemoveLEFT()
//    {
//        //銅像を左に向かせる
//        //StartRotate = new Vector3(0.0f, 90.0f, 0.0f);
//        //銅像を左に動かす
//        Vector3 Idou = new Vector3(-1.0f * targetspeed, 0.0f, 0.0f);
//        rb.AddForce(Idou, ForceMode.Force);

//    }

//    //センサーに侵入した場合
//    public void StatuemoveRIGHT()
//    {
//        //銅像を左に向かせる
//       // StartRotate = new Vector3(0.0f, 90.0f, 0.0f);
//        //銅像を左に動かす
//        Vector3 move = new Vector3(1.0f * targetspeed, 0.0f, 0.0f);
//        rb.AddForce(move, ForceMode.Force);

//    }


//    public void Statuestop()
//    {
//        //銅像を正面に向かせる
//       // StartRotate = new Vector3(0.0f, 0.0f, 0.0f);
//        //銅像を止まらせる
//        rb.velocity = Vector3.zero;

//    }

//}
//        //if (Ppos.x < rb.velocity.x)
//        //{
//        //    //銅像を左に動かす
//        //    Vector3 Idou = new Vector3(-1.0f * targetspeed, 0.0f, 0.0f);
//        //    rb.AddForce(Idou, ForceMode.Force);
//        //}
//        //else
//        //{
//        //    rb.velocity = Vector3.zero;
//        //}
//        //if (Ppos.x > rb.velocity.x)
//        //{
//        //    //銅像を右に動かす
//        //    Vector3 move = new Vector3(1.0f * targetspeed, 0.0f, 0.0f);
//        //    rb.AddForce(move, ForceMode.Force);
//        //}
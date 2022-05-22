using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GansekiScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("プレイヤーのプレハブを設定")]
    private GameObject GansekiPrefab;

    //Xの値にプラスする加速度
    [SerializeField] float speed = 5.0f;
    //カウントダウンする方式
    [SerializeField] float WaitTime = 1.0f;

    ////ゲームオブジェクト
    GameObject Sphere;

    //岩石の初期位置
    Vector3 StartPos;

    //リジッドボディー
    Rigidbody rb;

    //コライダー
    Collider col;

    bool Restart = false;

    bool Conti = false;



    void Start()
    {
        //Vector3 forceDirection = new Vector3(0.0f, 0.0f, 0.0f);

        //オブジェクトのRigidbodyコンポーネントを取得
        rb = gameObject.GetComponent<Rigidbody>();

        //オブジェクトのColliderコンポーネントを取得
        col = gameObject.GetComponent<Collider>();

        //Rockとタグ付けされているゲームオブジェクトをさがして定義をボールにする
        //Sphere = GameObject.FindGameObjectWithTag("Player");

        StartPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z );

         Restart = false;

        //Conti = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //力を設定
        Vector3 force = new Vector3(0.5f * speed, 0.0f, 0.0f);

        //力を加える
        rb.AddForce(force , ForceMode.Force);

        if(Restart == true)
        {
            WaitTime += 1.0f;
            if (WaitTime >= 120.0f)
            {
                WaitTime = 1.0f;
                //Conti = true;
                Restart = false;
                 this.transform.position = new Vector3(StartPos.x, StartPos.y, StartPos.z);
            }
        }
        
       

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "TapeBlock" || other.gameObject.tag == "sikaku")
        {

            this.transform.position = new Vector3(-200.0f, -200.0f, -200.0f);
            Restart = true;
            //見えなくする
       
       
            //if (Conti == true)
            //{ 
                
            //    Conti = false;
                 
            //} 
            //this.transform.position = new Vector3(StartPos.x, StartPos.y, StartPos.z);
            //if(Restart == true)
            //{           
            //StartCoroutine("HoldPosition");

            // }
            
            //Restart = false;
            //Destroy(this.gameObject);

           
        }
       
           
        
    }

    //IEnumerator HoldPosition()
    //{
    //    yield return new WaitForSeconds(WaitTime);

    //    moveForward = !moveForward;
    //    move = true;
    //}


    //IEnumerator HoldPosition()
    //{
    //    yield return new WaitForSeconds(WaitTime);

    //    Restart = true;
    //}
    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground" || other.gameObject.tag == "TapeBlock")
    //    {
    //        Debug.Log("当っていない");
    //        GameObject newPlayerObj = Instantiate(playerPrefab);
    //    }
    //}

    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("当たっている");
    //    }
    //}

    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("当たっていない");
    //    }
    //}

}

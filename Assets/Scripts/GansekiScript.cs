using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GansekiScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�v���C���[�̃v���n�u��ݒ�")]
    private GameObject GansekiPrefab;

    //X�̒l�Ƀv���X��������x
    [SerializeField] float speed = 5.0f;
    //�J�E���g�_�E���������
    [SerializeField] float WaitTime = 1.0f;

    ////�Q�[���I�u�W�F�N�g
    GameObject Sphere;

    //��΂̏����ʒu
    Vector3 StartPos;

    //���W�b�h�{�f�B�[
    Rigidbody rb;

    //�R���C�_�[
    Collider col;

    bool Restart = false;

    bool Conti = false;



    void Start()
    {
        //Vector3 forceDirection = new Vector3(0.0f, 0.0f, 0.0f);

        //�I�u�W�F�N�g��Rigidbody�R���|�[�l���g���擾
        rb = gameObject.GetComponent<Rigidbody>();

        //�I�u�W�F�N�g��Collider�R���|�[�l���g���擾
        col = gameObject.GetComponent<Collider>();

        //Rock�ƃ^�O�t������Ă���Q�[���I�u�W�F�N�g���������Ē�`���{�[���ɂ���
        //Sphere = GameObject.FindGameObjectWithTag("Player");

        StartPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z );

         Restart = false;

        //Conti = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�͂�ݒ�
        Vector3 force = new Vector3(0.5f * speed, 0.0f, 0.0f);

        //�͂�������
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
            //�����Ȃ�����
       
       
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
    //        Debug.Log("�����Ă��Ȃ�");
    //        GameObject newPlayerObj = Instantiate(playerPrefab);
    //    }
    //}

    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("�������Ă���");
    //    }
    //}

    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("�������Ă��Ȃ�");
    //    }
    //}

}

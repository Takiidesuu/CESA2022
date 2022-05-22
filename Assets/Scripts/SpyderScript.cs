using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpyderScript : MonoBehaviour
{

    [SerializeField]
    [Tooltip("�N���̗������x")]
    float fallspeed = 200.0f;

    [SerializeField]
    [Tooltip("�N���������鑬�x")]
    float crymspeed = 200.0f;

    //���W�b�h�{�f�B�[
    Rigidbody rb;

    //�R���C�_�[
    Collider col;

    //�A�j���[�V����
    private Animator anim;

    //�N���̏����ʒu
    private Vector3 StartPos;

    //�v���C���[���N���̂��ƂɈ����|�������ǂ����̔���
    //bool touch = false;




    void Start()
    {
        //�I�u�W�F�N�g��Collider�R���|�[�l���g���擾
        col = GetComponent<Collider>();

        //�I�u�W�F�N�g��Collider�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();

        //�I�u�W�F�N�g��Animator�R���|�[�l���g���擾
        anim = GetComponentInChildren<Animator>();

        //�����ʒu�̏�����
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
        Debug.Log("�����Ă��I");
     
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
        Debug.Log("�オ����");

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
    //    Debug.Log("�h��Ă�");
        
    //    anim.SetBool("Shaking", true);
    //    anim.SetBool("Falling", false);

    //    //�N�����~�܂点��
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


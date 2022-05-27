using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpyderScript : MonoBehaviour
{
    [Header("�����鑬�x")]
    [SerializeField] float Fallspeed = 4.0f;
    [Header("�����鑬�x")]
    [SerializeField] float Climbspeed = 2.0f;
    //���W�b�h�{�f�B�[
    Rigidbody rb;

    //�R���C�_�[
    Collider col;

    //�A�j���[�V����
    private Animator anim;

    private GameObject Spyder;

    //�N���̏����ʒu
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

        Spyder = GameObject.FindWithTag("spyder");
        //�����ʒu�̏�����
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
        //    Debug.Log("�����؂���");
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


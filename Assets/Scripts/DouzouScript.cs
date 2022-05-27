using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DouzouScript : MonoBehaviour
{
    //�J�E���g�_�E���������
    [SerializeField] float WaitTime = 1.0f;

    //�����̈ړ����x
    [SerializeField] float moveSpeed = 1.0f;

    //�v���C���[Transform�R���|�[�l���g���i�[����ϐ�
    private Transform target;

    private Quaternion defaultPos;

    MovePlayer moveplayer;

    Rigidbody rb;

    //�����̈ړ����x
    //private float moveSpeed = 2.0f;

    //��������~����v���C���[�Ƃ̋������i�[����ϐ�
    private float StopDistance = 1.5f;

    //�������v���C���[�Ɍ������Ĉړ����J�n���鋗�����i�[����ϐ�
    private float moveDistance = 5.0f;

    private bool Freeze;
    private bool DouzouMove;

    Animator anim;

    private void Start()
    {
        // target��Player�̃^�O������
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

                //�ϐ�targetPos���쐬���ăv���C���[�̍��W���i�[
                Vector3 targetPos = target.position;
                Quaternion targetRotate = target.rotation;

                //�������g��Y���W��ϐ�target��Y���W�Ɋi�[
                //(�v���C���[��X,Z���W�̂ݎQ��)
                targetPos.y = transform.position.y;
                //targetPos.x = transform.position.x;

                //transform.LookAt(targetPos);

                //�ϐ�distance���쐬���ē����̈ʒu�ƃv���C���[�̋������i�[
                float distance = Vector3.Distance(transform.position, target.position);

                // �����ƃv���C���[�̋�������
                // �ϐ� distance�i�v���C���[�Ɠ����̋����j���ϐ� moveDistance �̒l��菬�������
                // ����ɕϐ� distance ���ϐ� stopDistance �̒l�����傫���ꍇ
                if (distance < moveDistance && distance > StopDistance)
                {
                    //������ϐ�targetPos�̍��W�����Ɍ�������
                    //transform.LookAt(targetPos);


                    Quaternion targetRotation = Quaternion.LookRotation(targetPos - this.transform.position);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation ,targetRotation, Time.deltaTime);

                    //�ϐ�moveSpeed����Z�������x�œ�����O�����Ɉړ�����
                    //transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.position.x,this.transform.position.y, this.transform.position.z), Time.deltaTime * moveSpeed);
                    //anim.SetBool("Stand", false);
                    anim.SetBool("Move", true);
                                       
                }
                else if (distance >= 5.0f)
                {
                    //Debug.Log("5���[�g�����ꂽ��");
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
//    [Tooltip("�����̒ǐՑ��x")]
//    float targetspeed = 100.0f;

//    //���W�b�h�{�f�B�[
//    Rigidbody rb;

//    //�R���C�_�[
//    Collider col;

//    //�����̏����ʒu
//    private Vector3 StartPos;

//    //�����̏����̌���
//    private Vector3 StartRotate;

//    //private Vector3 Ppos;




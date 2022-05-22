using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DouzouScript : MonoBehaviour
{   
    //�v���C���[Transform�R���|�[�l���g���i�[����ϐ�
    public Transform target;

    //�����̈ړ����x
    private float moveSpeed = 2.0f;

    //��������~����v���C���[�Ƃ̋������i�[����ϐ�
    private float StopDistance = 1.5f;

    //�������v���C���[�Ɍ������Ĉړ����J�n���鋗�����i�[����ϐ�
    private float moveDistance = 5.0f;

    void Update()
    {
        //�ϐ�targetPos���쐬���ăv���C���[�ɂ̍��W���i�[
        Vector3 targetPos = target.position;

        //�������g��Y���W��ϐ�target��Y���W�Ɋi�[
        //(�v���C���[��X,Z���W�̂ݎQ��)
        targetPos.y = transform.position.y;
        //������ϐ�targetPos�̍��W�����Ɍ�������
        transform.LookAt(targetPos);

        //�ϐ�distance���쐬���ē����̈ʒu�ƃv���C���[�̋������i�[
        float distance = Vector3.Distance(transform.position, target.position);

        // �����ƃv���C���[�̋�������
        // �ϐ� distance�i�v���C���[�Ɠ����̋����j���ϐ� moveDistance �̒l��菬�������
        // ����ɕϐ� distance ���ϐ� stopDistance �̒l�����傫���ꍇ
        if (distance < moveDistance && distance > StopDistance)
        {
            //�ϐ�moveSpeed����Z�������x�œ�����O�����Ɉړ�����
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }
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



//    // Start is called before the first frame update
//    void Start()
//    {
//        //�I�u�W�F�N�g��Collider�R���|�[�l���g���擾
//        col = GetComponent<Collider>();

//        //�I�u�W�F�N�g��Collider�R���|�[�l���g���擾
//        rb = GetComponent<Rigidbody>();

//        //�����ʒu�̏�����
//        StartPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.transform.position.z);

//        //�����̌����̏�����
//        StartRotate = new Vector3(0.0f, 0.0f, 0.0f);

//        //Ppos = GameObject.FindGameObjectWithTag("Player").transform.position;
//    }

//    //�Z���T�[�ɐN�������ꍇ
//    public void StatuemoveLEFT()
//    {
//        //���������Ɍ�������
//        //StartRotate = new Vector3(0.0f, 90.0f, 0.0f);
//        //���������ɓ�����
//        Vector3 Idou = new Vector3(-1.0f * targetspeed, 0.0f, 0.0f);
//        rb.AddForce(Idou, ForceMode.Force);

//    }

//    //�Z���T�[�ɐN�������ꍇ
//    public void StatuemoveRIGHT()
//    {
//        //���������Ɍ�������
//       // StartRotate = new Vector3(0.0f, 90.0f, 0.0f);
//        //���������ɓ�����
//        Vector3 move = new Vector3(1.0f * targetspeed, 0.0f, 0.0f);
//        rb.AddForce(move, ForceMode.Force);

//    }


//    public void Statuestop()
//    {
//        //�����𐳖ʂɌ�������
//       // StartRotate = new Vector3(0.0f, 0.0f, 0.0f);
//        //�������~�܂点��
//        rb.velocity = Vector3.zero;

//    }

//}
//        //if (Ppos.x < rb.velocity.x)
//        //{
//        //    //���������ɓ�����
//        //    Vector3 Idou = new Vector3(-1.0f * targetspeed, 0.0f, 0.0f);
//        //    rb.AddForce(Idou, ForceMode.Force);
//        //}
//        //else
//        //{
//        //    rb.velocity = Vector3.zero;
//        //}
//        //if (Ppos.x > rb.velocity.x)
//        //{
//        //    //�������E�ɓ�����
//        //    Vector3 move = new Vector3(1.0f * targetspeed, 0.0f, 0.0f);
//        //    rb.AddForce(move, ForceMode.Force);
//        //}
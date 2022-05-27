using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMove : MonoBehaviour
{
    private Transform target;

    ///ゴールのポジション
    private Vector3 GoalPos;

    [SerializeField] float speed = 0.5f;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponentInChildren<Animator>();

        GoalPos = GameObject.FindGameObjectWithTag("Entrance").transform.position;
    }

    public void PlayerWork()
    {
        anim.SetBool("isMove", true);
        this.transform.position = Vector3.MoveTowards(transform.position, GoalPos, Time.deltaTime * speed);
    }

}

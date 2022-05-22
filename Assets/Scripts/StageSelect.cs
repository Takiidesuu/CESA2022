using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    GameObject movedoor;
    bool coroutine_bool = false; // ��]�����ǂ���

    // Start is called before the first frame update
    void Start()
    {
        movedoor = GameObject.FindWithTag("movedoor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            //��]���ł͂Ȃ��ꍇ�͎��s 
            if (!coroutine_bool)
            {
                coroutine_bool = true;
                StartCoroutine("RightMove");
            }
        }

        if (Input.GetKeyDown("left"))
        {
            //��]���ł͂Ȃ��ꍇ�͎��s 
            if (!coroutine_bool)
            {
                coroutine_bool = true;
                StartCoroutine("LeftMove");
            }
        }
    }

    //�E�ɂ�������]����72���ŃX�g�b�v
    IEnumerator RightMove()
    {
        for (int turn = 0; turn < 72; turn++)
        {
            transform.Rotate(0, -1, 0);
            yield return new WaitForSeconds(0.01f);
        }
        coroutine_bool = false;
    }

    //���ɂ�������]����72���ŃX�g�b�v
    IEnumerator LeftMove()
    {
        for (int turn = 0; turn < 72; turn++)
        {
            transform.Rotate(0, 1, 0);
            yield return new WaitForSeconds(0.01f);
        }
        coroutine_bool = false;
    }


}

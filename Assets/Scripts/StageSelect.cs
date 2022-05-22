using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    GameObject movedoor;
    bool coroutine_bool = false; // 回転中かどうか

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
            //回転中ではない場合は実行 
            if (!coroutine_bool)
            {
                coroutine_bool = true;
                StartCoroutine("RightMove");
            }
        }

        if (Input.GetKeyDown("left"))
        {
            //回転中ではない場合は実行 
            if (!coroutine_bool)
            {
                coroutine_bool = true;
                StartCoroutine("LeftMove");
            }
        }
    }

    //右にゆっくり回転して72°でストップ
    IEnumerator RightMove()
    {
        for (int turn = 0; turn < 72; turn++)
        {
            transform.Rotate(0, -1, 0);
            yield return new WaitForSeconds(0.01f);
        }
        coroutine_bool = false;
    }

    //左にゆっくり回転して72°でストップ
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

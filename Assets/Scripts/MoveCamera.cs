using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("カメラの上下向き")]
    [SerializeField] float rotation = 15.0f;
    
    [Header("位置設定")]
    [SerializeField] float height = 8.0f;
    [SerializeField] float distance = 25.0f;
    
    private GameObject playerObj;   //プレイヤーオブジェクト変数
    
    private bool goal = false;
    private Vector3 goalPos;

    [HideInInspector] public  Camera m_Camera;
    
    public void SetGoal()
    {
        goal = true;
    }

    public void ZoomFov(Camera camera, float zoom, float duration)
    {
        float defaultFov = camera.fieldOfView;
        DOTween.To(() => camera.fieldOfView, fov => camera.fieldOfView = fov, zoom, duration);
    }

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーオブジェクトを変数に入れる
        playerObj = GameObject.FindGameObjectWithTag("Player");
        m_Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!goal)
        {
            //カメラの位置をプレイヤーの位置によって、変える
            this.transform.position = new Vector3(playerObj.transform.position.x + 2.0f, playerObj.transform.position.y + height, playerObj.transform.position.z - distance);
            
            //下に向かせる
            this.transform.localRotation = Quaternion.Euler(rotation, 0.0f, 0.0f);
        }
        else
        {
            goalPos = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y + 1.0f, playerObj.transform.position.z - 10.0f);
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, goalPos, 4.0f * Time.deltaTime);
            
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.identity, 18.0f * Time.deltaTime);
        }
    }
}
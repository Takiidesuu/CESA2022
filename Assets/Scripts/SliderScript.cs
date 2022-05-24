using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slide;
    bool beingPressed = false;
    bool stop = false;
    private float slideSpeed = 0.01f;

    [Header("�Y�[�����鋗���i����p���ǂ̂��炢�ς��邩�j")]
    [SerializeField] float zoomDistance = 0.0f;

    [Header("�Y�[�����鑬�x�i�Y�[��������܂łɂ����鎞�ԁj")]
    [SerializeField] float zoomSpeed = 0.0f;

    [Header("�Y�[����߂����x")]
    [SerializeField] float backSpeed = 0.0f;

    private float slideValue = 0.0f;
    private float holdTime = 2.0f;

    MoveCamera m_ZoomCamera;
    
    public void PressCondition(bool state, float hTime)
    {
        beingPressed = state;
        holdTime = hTime;
    }
    
    public void StopInc()
    {
        stop = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
       slide = GetComponent<Slider>();
       m_ZoomCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveCamera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        slideSpeed = 1.0f / (holdTime * 60.0f * 0.85f);
        
        if (beingPressed && !stop)
        {
            slideValue += slideSpeed;
           m_ZoomCamera.ZoomFov(m_ZoomCamera.m_Camera, zoomDistance, zoomSpeed);

        }
        else if (beingPressed && stop)
        {
            m_ZoomCamera.ZoomFov(m_ZoomCamera.m_Camera, m_ZoomCamera.m_Camera.fieldOfView, backSpeed);
            ;
        }
        else
        {
            slideValue = 0.0f;
            stop = false;
            m_ZoomCamera.ZoomFov(m_ZoomCamera.m_Camera, 33.0f, backSpeed);
        }

  
        slide.value = slideValue;
    }
}

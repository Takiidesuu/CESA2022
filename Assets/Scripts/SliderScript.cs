using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slide;
    bool beingPressed = false;
    bool stop = false;
    [SerializeField] float slideSpeed = 0.01f;

    [Header("ズームする距離（視野角をどのくらい変えるか）")]
    [SerializeField] float zoomDistance = 0.0f;

    [Header("ズームする速度（ズームしきるまでにかかる時間）")]
    [SerializeField] float zoomSpeed = 0.0f;

    [Header("ズームを戻す速度")]
    [SerializeField] float backSpeed = 0.0f;

    private float slideValue = 0.0f;
    private float holdTime = 2.5f;

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
        slideSpeed = 1 / 60.0f * holdTime * 0.85f;
        
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

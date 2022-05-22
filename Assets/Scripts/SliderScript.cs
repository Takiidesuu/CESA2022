using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slide;
    bool beingPressed = false;
    bool stop = false;
    
    private float slideValue = 0.0f;
    
    public void PressCondition(bool state)
    {
        beingPressed = state;
    }
    
    public void StopInc()
    {
        stop = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        slide = GetComponent<Slider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (beingPressed && !stop)
        {
            slideValue += 0.01f;
        }
        else if (beingPressed && stop)
        {
            ;
        }
        else
        {
            slideValue = 0.0f;
            stop = false;
        }
        
        slide.value = slideValue;
    }
}

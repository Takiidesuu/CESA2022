using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJoint : MonoBehaviour
{
    private float angle = 0.0f;
    private bool reverseFlg = false;
    private bool startRotate = false;
    
    private float rotateSpeed = 2.8125f;
    
    public void StartProcess(float speed)
    {
        startRotate = true;
        speed = rotateSpeed;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        angle = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startRotate)
        {
            angle += rotateSpeed;
            
            if (!reverseFlg)
            {
                this.transform.Rotate(0.0f, 0.0f, rotateSpeed, Space.Self);
                
                if (angle >= 60.0f)
                {
                    reverseFlg = true;
                    angle = 0.0f;
                }
            }
            else
            {
                this.transform.Rotate(0.0f, 0.0f, rotateSpeed, Space.Self);
                
                if (Mathf.Abs(angle) >= 60.0f)
                {
                    reverseFlg = false;
                    startRotate = false;
                    angle = 0.0f;
                }
            }
        }
    }
}

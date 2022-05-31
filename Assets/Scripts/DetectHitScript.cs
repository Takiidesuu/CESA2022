using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHitScript : MonoBehaviour
{
    MoveTetubo tetuboScript;
    
    private void Start() 
    {
        tetuboScript = this.transform.parent.parent.gameObject.GetComponent<MoveTetubo>();
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "TapeBlock")
        {
            tetuboScript.StopMove();
        }
    }
}

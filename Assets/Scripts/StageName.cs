using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageName : MonoBehaviour
{
    Text stageName;
    void Start()
    {
        stageName = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
    }

    public string SetStageName(int wNum , int sNum)
    {
        stageName.text = "Stage" + wNum.ToString() + "-" + sNum.ToString();
        return stageName.text;
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    private GameObject Player;
    private GameObject[] RadarObj;
    
    private void Start() 
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MoveCamera>().SetGoal();
            Player.GetComponent<MovePlayer>().ReachedGoal();
            
            RadarObj = GameObject.FindGameObjectsWithTag("RadarObj");
            
            foreach (GameObject radarObjs in RadarObj)
            {
                radarObjs.SetActive(false);
            }
        }
    }
}

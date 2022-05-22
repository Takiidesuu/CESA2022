using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadScript : MonoBehaviour
{
    private int worldNum = 0;
    private int stageNum = 0;
    
    public void SaveGame(int wNum, int sNum, int score)
    {
        var stageName = "Stage" + wNum + "-" + sNum;
        var pastScore = PlayerPrefs.GetInt(stageName, 0);
        
        if (pastScore < score)
        {
            PlayerPrefs.SetInt(stageName, score);
        }
        else
        {
            
        }
    }
    
    public int LoadGame(int wNum, int sNum)
    {
        var stageName = "Stage" + wNum + "-" + sNum;
        
        return PlayerPrefs.GetInt(stageName);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

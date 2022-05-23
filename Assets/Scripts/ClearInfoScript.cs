using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearInfoScript : MonoBehaviour
{
    List<ClearInfo> scores = new List<ClearInfo>();
    
    // Start is called before the first frame update
    void Start()
    {
        scores = XMLManager.instance.LoadScores();
    }
    
    public void SaveStageState(int wNum, int sNum, int stNum, bool isClear)
    {
        scores.Add(new ClearInfo {WorldNum = wNum, StageNum = sNum, StoneNum = stNum, IsCleared = isClear});
        
        XMLManager.instance.SaveScores(scores);
    }
}
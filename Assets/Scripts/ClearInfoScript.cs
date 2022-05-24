using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearInfoScript : MonoBehaviour
{
    List<ClearInfo> scores = new List<ClearInfo>();
    
    public static ClearInfoScript instance;
    
    private int worldNumber, stageNumber = 0;
    
    private void Awake() 
    {
        instance = this;
    }
    
    public void SetWorldStageNum(int wNum, int sNum)
    {
        worldNumber = wNum;
        stageNumber = sNum;
    }
    
    public void CreateSave()
    {
        for (int world = 1; world < 6; world++)
        {
            for (int stage = 1; stage < 6; stage++)
            {
                scores.Add(new ClearInfo{WorldNum = world, StageNum = stage, StoneNum = 0, IsCleared = false});
            }
        }
        
        XMLManager.instance.SaveScores(scores);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        scores = XMLManager.instance.LoadScores();
        
        for (int a = 0; a < scores.Count; a++)
        {
            Debug.Log(scores[a].WorldNum + "  " + scores[a].StageNum + "  " + scores[a].StoneNum + "  " + scores[a].IsCleared);
        }
        
        worldNumber = 1;
        stageNumber = 2;
    }
    
    public void SaveStageState(int stNum, bool isClear)
    {
        if (!scores.Contains(new ClearInfo{WorldNum = worldNumber, StageNum = stageNumber}))
        {
            scores.Add(new ClearInfo {WorldNum = worldNumber, StageNum = stageNumber, StoneNum = stNum, IsCleared = isClear});
        }
        else if (scores.Contains(new ClearInfo{WorldNum = worldNumber, StageNum = stageNumber}))
        {
            var index = scores.IndexOf(new ClearInfo{WorldNum = worldNumber, StageNum = stageNumber});
            
            if (!scores[index].IsCleared)
            {
                scores[index].StoneNum = stNum;
                scores[index].IsCleared = isClear;
            }
            else if (scores[index].IsCleared)
            {
                if (scores[index].StoneNum < stNum)
                {
                    scores[index].StoneNum = stNum;
                }
            }
        }
        
        XMLManager.instance.SaveScores(scores);
    }
}
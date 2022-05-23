using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class ClearInfo
{
    public int WorldNum;
    public int StageNum;
    public int StoneNum;
}

[System.Serializable]
public class Leaderboard
{
    public List<ClearInfo> list = new List<ClearInfo>();
}

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    
    void Awake()
    {
        instance = this;
    }
}

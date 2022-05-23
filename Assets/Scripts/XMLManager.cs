using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    
    public static XMLManager instance;
    
    void Awake()
    {
        instance = this;
        
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/");
        }
    }
    
    public void SaveScores(List<ClearInfo> scoresToSave)
    {
        leaderboard.list = scoresToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/SaveData/savedata.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }
    
    public List<ClearInfo> LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData/savedata.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.persistentDataPath + "/SaveData/savedata.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Leaderboard;
        }
        return leaderboard.list;
    }
}

[System.Serializable]
public class Leaderboard
{
    public List<ClearInfo> list = new List<ClearInfo>();
}
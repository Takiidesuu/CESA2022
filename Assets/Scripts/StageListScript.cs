using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class StageListScript : MonoBehaviour
{
    public StageListing stageListing;
    
    List<StageList> stageList = new List<StageList>();
    
    private void Awake() 
    {
        for (int a = 1; a < 6 ; a++)
        {
            for (int b = 1; b < 6; b++)
            {
                var numberStone = 0;
                switch (a)
                {
                    case 1:
                        switch (b)
                        {
                            case 1: numberStone = 3;
                                break;
                            case 2: numberStone = 3;
                                break;
                            case 3: numberStone = 3;
                                break;
                            case 4: numberStone = 4;
                                break;
                            case 5: numberStone = 4;
                                break;
                        }
                        break;
                    case 2:
                        switch (b)
                        {
                            case 1: numberStone = 4;
                                break;
                            case 2: numberStone = 3;
                                break;
                            case 3: numberStone = 3;
                                break;
                            case 4: numberStone = 4;
                                break;
                            case 5: numberStone = 4;
                                break;
                        }
                        break;
                    case 3:
                        switch (b)
                        {
                            case 1: numberStone = 4;
                                break;
                            case 2: numberStone = 4;
                                break;
                            case 3: numberStone = 3;
                                break;
                            case 4: numberStone = 3;
                                break;
                            case 5: numberStone = 5;
                                break;
                        }
                        break;
                    case 4:
                        switch (b)
                        {
                            case 1: numberStone = 3;
                                break;
                            case 2: numberStone = 3;
                                break;
                            case 3: numberStone = 3;
                                break;
                            case 4: numberStone = 3;
                                break;
                            case 5: numberStone = 3;
                                break;
                        }
                        break;
                    case 5:
                        switch (b)
                        {
                            case 1: numberStone = 3;
                                break;
                            case 2: numberStone = 3;
                                break;
                            case 3: numberStone = 3;
                                break;
                            case 4: numberStone = 3;
                                break;
                            case 5: numberStone = 3;
                                break;
                        }
                        break;
                }
                
                stageList.Add(new StageList{Name = "Stage" + a + "-" + "b", WorldNum = a, StageNum = b, StoneNum = numberStone});
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        /* stageListing.list = stageList;
        XmlSerializer serializer = new XmlSerializer(typeof(StageList));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/StageList.xml", FileMode.Create);
        serializer.Serialize(stream, stageListing);
        stream.Close(); */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [System.Serializable]
    public class StageListing
    {
        public List<StageList> list = new List<StageList>();
    }
}
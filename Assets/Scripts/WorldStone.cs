using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldStone : MonoBehaviour
{
    int stoneCnt;

    [SerializeField] Text stoneText;

    // Start is called before the first frame update
    void Start()
    {
        stoneCnt = GameObject.FindGameObjectsWithTag("Stone").Length;
        stoneText.text ="/ " + stoneCnt.ToString();
        
    }


}

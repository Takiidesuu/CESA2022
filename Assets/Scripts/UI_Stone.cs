using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stone : MonoBehaviour
{
    [SerializeField] Text StageName;
    [SerializeField] GameObject[] stoneUI;

    private void Start()
    {
        for (int i = 0; i < stoneUI.Length; i++)
        {
            stoneUI[i].SetActive(false);
        }
    }

    public void StoneSetFalse(int wNum, int sNum)
    {
        int stoneNum = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StageSelectScript>().GetStoneNum(wNum, sNum);
        stoneUI[stoneNum - 3].SetActive(false);
    }
    public void Init(int wNum , int sNum)
    {
        int stoneNum = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StageSelectScript>().GetStoneNum(wNum, sNum);
        // wNum��sNum����ΔՂ̌��Ђ̌����擾
        //
        //////////////////////

        // �X�e�[�W�̐ΔՂ̐��ɉ�����UI��\��
        stoneUI[stoneNum - 3].SetActive(true);
        
        var collectedStone = ClearInfoScript.instance.GetCollectedStoneNumber(wNum, sNum);
        
        for (int a = 0; a < stoneNum + 1; a++)
        {   
            if (a <= collectedStone)
            {
                stoneUI[stoneNum - 3].transform.GetChild(a).gameObject.SetActive(true);
            }
            else
            {
                stoneUI[stoneNum - 3].transform.GetChild(a).gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{   
    [Header("ブロック出てくる時のエフェクト")]
    [SerializeField] private GameObject rockPartObj;
    
    [Header("テープ剥がす時のエフェクト")]
    [SerializeField] private GameObject tapePartObj;
    
    [Header("石盤のエフェクト")]
    [SerializeField] private GameObject stonePartObj;
    
    [Header("石盤獲得時のエフェクト")]
    [SerializeField] private GameObject getStonePartObj;

    [Header("石盤ゴール時のエフェクト")]
    [SerializeField] private GameObject getGoalStoneObj;

    public GameObject GetRockEffect()
    {
        return rockPartObj;
    }
    
    public GameObject GetTapeEffect()
    {
        return tapePartObj;
    }
    
    public GameObject GetStoneEffect()
    {
        return stonePartObj;
    }
    
    public GameObject GetAchieveStoneEffect()
    {
        return getStonePartObj;
    }
    public GameObject GetGoalStoneEffect()
    {
        return getGoalStoneObj;
    }
}

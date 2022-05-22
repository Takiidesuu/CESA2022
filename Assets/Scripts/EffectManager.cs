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
    
    /* private ParticleSystem rockEffect;
    private ParticleSystem tapeEffect;
    private ParticleSystem stoneEffect;

    private void Start() 
    {
        rockEffect = rockPartObj.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        tapeEffect = tapePartObj.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        stoneEffect = stonePartObj.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    } */

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
}

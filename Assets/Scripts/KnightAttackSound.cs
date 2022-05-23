using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackSound : MonoBehaviour
{
    public void PlayAttackSound()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlaySwordSE();
    }
}
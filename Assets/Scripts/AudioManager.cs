using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource bgmAudioSource;
    AudioSource seAudioSource;
    
    private float bgmOptionVolume = 1.0f;
    private float seOptionVolume = 1.0f;
    
    [Header("メインBGM")]
    [SerializeField] AudioClip bgmClip = null;
    [SerializeField] [Range(0.0f, 1.0f)] float bgmVolume = 1.0f;
    
    
    
    
    
    [Header("カーソル移動")]
    [SerializeField] AudioClip cursorSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float cursorVolume = 1.0f;
    
    [Header("決定（タイトル")]
    [SerializeField] AudioClip titleDecideSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float titleDecideVolume = 1.0f;
    
    [Header("決定２")]
    [SerializeField] AudioClip decideSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float decideVolume = 1.0f;
    
    [Header("戻る")]
    [SerializeField] AudioClip cancelSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float cancelVolume = 1.0f;
    
    [Header("ワールド選択の回るところ")]
    [SerializeField] AudioClip rotateSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float rotateVolume = 1.0f;
    
    [Header("扉の空くSE")]
    [SerializeField] AudioClip doorSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float doorVolume = 1.0f;
    
    
    
    [Header("移動")]
    [SerializeField] AudioClip moveSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float moveVolume = 1.0f;
    
    [Header("ジャンプSE")]
    [SerializeField] AudioClip jumpSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float jumpVolume = 1.0f;
    
    [Header("剥がすSE")]
    [SerializeField] AudioClip tearSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float tearVolume = 1.0f;
    
    [Header("ブロック出てくるSE")]
    [SerializeField] AudioClip blockSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float blockVolume = 1.0f;
    
    [Header("石盤入手SE")]
    [SerializeField] AudioClip slateSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float slateVolume = 1.0f;
    
    [Header("ポーズSE")]
    [SerializeField] AudioClip pauseSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float pauseVolume = 1.0f;
    
    [Header("敵と衝突SE")]
    [SerializeField] AudioClip enemyHitSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float enemyHitVolume = 1.0f;
    
    [Header("剣と床衝突SE")]
    [SerializeField] AudioClip swordSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float swordVolume = 1.0f;
    
    [Header("岩石の転がるSE")]
    [SerializeField] AudioClip rockSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float rockVolume = 1.0f;
    
    [Header("砂埃が舞うSE")]
    [SerializeField] AudioClip dustSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float dustVolume = 1.0f;
    
    [Header("蜘蛛の動くSE")]
    [SerializeField] AudioClip spiderMoveSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float spiderMoveVolume = 1.0f;
    
    [Header("蜘蛛の落ちるSE")]
    [SerializeField] AudioClip spiderFallSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float spiderFallVolume = 1.0f;
    
    [Header("金棒SE")]
    [SerializeField] AudioClip barSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float barVolume = 1.0f;
    
    [Header("岩石に潰されたSE")]
    [SerializeField] AudioClip smashSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float smashVolume = 1.0f;
    
    [Header("岩石の衝突SE")]
    [SerializeField] AudioClip rockCrashSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float rockCrashVolume = 1.0f;
    
    [Header("風SE")]
    [SerializeField] AudioClip windSE = null;
    [SerializeField] [Range(0.0f, 1.0f)] float windVolume = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bgmAudioSource = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        bgmAudioSource.loop = true;
        
        seAudioSource = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        seAudioSource.loop = false;
        
        PlayBGM();
    }
    
    private void PlayBGM()
    {
        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.volume = bgmVolume;
        
        bgmAudioSource.Play();
    }
    
    public void PauseBGM(bool play)
    {
        if (play)
        {
            bgmAudioSource.Pause();
        }
        else
        {
            bgmAudioSource.UnPause();
        }
    }
    
    /////////////////////////////////////////////////////////////////////////
    
    public void PlayCursorSE()
    {
        PlaySE(cursorSE, cursorVolume);
    }
    
    public void PlayTitleDecideSE()
    {
        PlaySE(titleDecideSE, titleDecideVolume);
    }
    
    public void PlayDecideSE()
    {
        PlaySE(decideSE, decideVolume);
    }
    
    public void PlayCancelSE()
    {
        PlaySE(cancelSE, cancelVolume);
    }
    
    public void PlayRotateSE()
    {
        PlaySE(rotateSE, rotateVolume);
    }
    
    public void PlayDoorSE()
    {
        PlaySE(doorSE, doorVolume);
    }
    
    /////////////////////////////////////////////////////////////////////////
    
    public AudioClip GetMoveSE()
    {
        return moveSE;
    }
    
    public float GetMoveVolume()
    {
        return moveVolume;
    }
    
    public AudioClip GetRockSE()
    {
        return rockSE;
    }
    
    public float GetRockVolume()
    {
        return rockVolume;
    }
    
    public AudioClip GetWindSE()
    {
        return windSE;
    }
    
    public float GetWindVolume()
    {
        return windVolume;
    }
    
    public void PlayJumpSE()
    {
        PlaySE(jumpSE, jumpVolume);
    }
    
    public void PlayTearSE()
    {
        PlaySE(tearSE, tearVolume);
    }
    
    public void PlayBlockSE()
    {
        PlaySE(blockSE, blockVolume);
    }
    
    public void PlayGetStoneSE()
    {
        PlaySE(slateSE, slateVolume);
    }
    
    public void PlayPauseSE()
    {
        PlaySE(pauseSE, pauseVolume);
    }
    
    public void PlayEnemyHitSE()
    {
        PlaySE(enemyHitSE, enemyHitVolume);
    }
    
    public void PlaySwordSE()
    {
        PlaySE(swordSE, swordVolume);
    }
    
    public void PlayDustSE()
    {
        PlaySE(dustSE, dustVolume);
    }
    
    public void PlaySpiderMoveSE()
    {
        PlaySE(spiderMoveSE, spiderFallVolume);
    }
    
    public void PlaySpiderFallSE()
    {
        PlaySE(spiderFallSE, spiderFallVolume);
    }
    
    public void PlayBarSE()
    {
        PlaySE(barSE, barVolume);
    }
    
    public void PlaySmashSE()
    {
        PlaySE(smashSE, smashVolume);
    }
    
    public void PlayRockCrashSE()
    {
        PlaySE(rockCrashSE, rockCrashVolume);
    }
    
    private void PlaySE(AudioClip aClip, float aVolume)
    {   
        aVolume = aVolume * seOptionVolume / 5;
        seAudioSource.PlayOneShot(aClip, aVolume);
    }

    // Update is called once per frame
    void Update()
    {
        bgmOptionVolume = (float)TitleCameraScript.bgmvolume;
        seOptionVolume = (float)TitleCameraScript.sevolume;
        
        bgmAudioSource.volume = (bgmVolume * bgmOptionVolume / 5);
    }
}
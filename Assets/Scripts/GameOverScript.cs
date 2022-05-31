using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    InputManager inputScript;
    bool SECheck = false;

    // GameOver表示オブジェクト
    [SerializeField] Image fadePanel;

    float fadespeed = 0.5f;
    [SerializeField] GameObject gameOverText;

    // コンティニュー表示オブジェクト
    Image cursor;
    private RectTransform cursorPos;
    private int cursorPosition;

    // フェード管理変数
    bool isFade = false;

    // 現在の状況を管理する変数
    private bool nextState = false;
    private int nowState = 1;

    //  シーン遷移用
    CanvasGroup gameOverObject;
    string sceneName;

    // レーダー消す
    private GameObject[] RadarObj;

    AudioManager audioManager;
    bool KeydownFlag;

    private void Start()
    {
        
        fadePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();
        fadePanel.enabled = true;

        gameOverText = GameObject.FindGameObjectWithTag("GameOverText");
        gameOverText.SetActive(true);

        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Image>();
        cursorPos = cursor.GetComponent<RectTransform>();

        inputScript = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        cursorPosition = 1;
        gameOverObject = GameObject.FindGameObjectWithTag("GameOver").GetComponent<CanvasGroup>();
        gameOverObject.alpha = 0;

        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        KeydownFlag = false;
        SECheck = false;
    }
    void Update()
    {
        switch (nowState)
        {
            case 1:
                nextState =  Fade_In(fadePanel, fadespeed / 1.5f);
                if (true)
                {
                    if (nextState)
                    {
                        nextState = false;
                        isFade = false;

                        nowState = 2;
                    }
                }
                break;

            case 2:

                if (isFade)
                {
                    nextState = Fade_Out(fadePanel, fadespeed);

                }

                if (nextState)
                {
                    nextState = false;
                    isFade = false;

                    gameOverObject.alpha = 1;
                    nowState = 3;
                }
                break;
            case 3:
                SelectContinue();
                break;
            case 4:
                if (isFade)
                {
                    nextState = CanvasFade(gameOverObject , fadespeed , false);
                }
                if (nextState)
                {
                    isFade = false;
                    nowState = 5;
                }
                break;
            case 5:
                if (nextState)
                {
                    SceneManager.LoadScene(sceneName);
                    nextState = false;
                }
                
                break;

        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isFade = true;
            inputScript.SwitchToGameOver();

            RadarObj = GameObject.FindGameObjectsWithTag("RadarObj");

            foreach (GameObject radarObjs in RadarObj)
            {
                radarObjs.SetActive(false);
            }

            audioManager.SetGameOver();
            
            // ゲームオーバー音声流す
            if (!SECheck)
            {
                audioManager.PlayGameOverSE();
            }
            
            Time.timeScale = 0.0f;
        }
    }


    


    void SelectContinue()
    {
        bool ClickOK = inputScript.GetSelectState();
        Vector2 CursorVec = inputScript.GetMenuMoveFloat();

        if (CursorVec.x == -1.0f && !KeydownFlag)
        {
            cursorPosition = 1;
            cursorPos.localPosition = new Vector3(-215.0f, -200.0f,0.0f);
            audioManager.PlayCursorSE();
            KeydownFlag = true;
        }
        else if (CursorVec.x == 1.0f && !KeydownFlag)
        {
            cursorPosition = 2;
            cursorPos.localPosition = new Vector3(590.0f, -200.0f,0.0f);
            audioManager.PlayCursorSE();
            KeydownFlag = true;
        }
        else if(CursorVec.x == 0.0f)
        {
            KeydownFlag = false;
        }

        if(ClickOK && cursorPosition == 1)
        {
            sceneName = SceneManager.GetActiveScene().name;
            isFade = true;
            nowState = 4;
            audioManager.PlayDecideSE();
        }
        else if(ClickOK && cursorPosition == 2)
        {
            sceneName = "WorldSelect";
            isFade = true;
            nowState = 4;
            audioManager.PlayDecideSE();
        }
    }


    public bool Fade_In(Image fadepanel,float fadespeed)
    {
        float r = fadepanel.color.r;
        float g = fadepanel.color.g;
        float b = fadepanel.color.b;
        float alfa = fadepanel.color.a;

        alfa -= fadespeed * Time.deltaTime;
        fadepanel.color = new Color(r, g, b, alfa);
        if (fadepanel.color.a < 0)
        {
            return true;
        }
        return false;

    }
    public bool Fade_Out(Image fadepanel,float fadespeed)
    {
        float r = fadepanel.color.r;
        float g = fadepanel.color.g;
        float b = fadepanel.color.b;
        float alfa = fadepanel.color.a;

        
        alfa += fadespeed * Time.deltaTime;
        fadepanel.color = new Color(r, g, b, alfa);
        if (fadepanel.color.a >= 1)
        {
            return true;
        }
        return false;
    }

    bool CanvasFade(CanvasGroup canvasGroup , float fadespeed , bool fadeState)
    {
        if (fadeState)
        {
            // フェードイン
            float alfa = canvasGroup.alpha;

            alfa += fadespeed * Time.deltaTime;

            canvasGroup.alpha = alfa;
            if (canvasGroup.alpha >= 1)
            {
                return true;
            }
        }
        else
        {
            // フェードアウト
            float alfa = canvasGroup.alpha;

            alfa -= fadespeed * Time.deltaTime;

            canvasGroup.alpha = alfa;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                return true;
            }
        }

        return false;
    }


}


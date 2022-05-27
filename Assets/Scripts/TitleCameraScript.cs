using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCameraScript : MonoBehaviour
{
    TitleMove playerMove;
    //インプットのスクリプトを呼び出す
    private InputManager inputScript;
    
    private Transform target;

    //private Transform start;

    private AudioSource audioSource;
    private AudioManager audioManager;
    [SerializeField] private bool audioDebug;


    //サウンド処理
    //黒い玉
    //private GameObject BGMVolume;
    //private GameObject BGMVolume2;
    //private GameObject BGMVolume3;
    //private GameObject BGMVolume4;
    //private GameObject BGMVolume5;

    //今音量設定がどこにいるかをintで管理
    private int VolumePosistion;

    //キャンバスの中のカーソルを指定する
    private RectTransform cursor;
    private RectTransform startCursor;
    private RectTransform exitCursor;

    //カーソルの現在位置をintで管理
    private int CursorPosition;

    //ボタンが入力されているかどうか
    private bool KeyDownflag;
    private bool KeyDownflag2;

    //タイトルにいるかオプション設定画面にいるかどうかを判定
    private int nowPosition;

    ////プレイヤーの初期位置
    private Vector3 offset01;

    // エントランスのポジション
    private Vector3 entrancePos;

    //カメラのポジション
    private Vector3 CameraPos;
    ///ゴールのポジション
    private Vector3 GoalPos;
    ///スタートのポジション
    private Vector3 StartPos;

    private Quaternion StartRotate;

    //スピード
    private float speed;

    private bool cameramove = false;

    // タイトル
    public GameObject CanvasTitle;

    // Aボタン
    public GameObject CanvasAbutton;

    // はじめからetc...
    public GameObject CanvasStart;
    // はじめからで使うカーソル
    public Image CanvasStartCorsor;

    // ゲーム終了
    public GameObject CanvasExit;
    // ゲーム終了で使うカーソル
    public Image CanVasExitCorsor;


    // 音量レベル
    public static int bgmvolume = 5;
    public static int sevolume = 5;

    [SerializeField]
    [Tooltip("テキストのプレハブを設定")]
    private GameObject Canvas;

    //カウントダウンする方式
    [SerializeField] float WaitTime = 1.0f;

    bool ToGameStart = false;

    Slider BGMslider;
    Slider SEslider;

    [SerializeField] int part;
    private int maxvolume;

    void Start()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<TitleMove>();
        speed = 5.0f;
        //看板のゲームオブジェクトのタグを取得する
        target = GameObject.FindWithTag("SignBoard").transform;

        //スタートのゲームオブジェクトのタグを取得する
        //start = GameObject.FindWithTag("sikaku").transform;

        //BGMVolume = GameObject.FindWithTag("Level1");
        //BGMVolume2 = GameObject.FindWithTag("Level2");
        //BGMVolume3 = GameObject.FindWithTag("Level3");
        //BGMVolume4 = GameObject.FindWithTag("Level4");
        //BGMVolume5 = GameObject.FindWithTag("Level5");

        // タイトルに必要なCanvasだけ表示
        CanvasTitle.SetActive(true);
        CanvasAbutton.SetActive(true);
        CanvasStart.SetActive(false);
        CanvasExit.SetActive(false);


        //UIのレクトトランスフォーム
        cursor = GameObject.FindWithTag("Cursor").GetComponent<RectTransform>();
        startCursor = CanvasStartCorsor.GetComponent<RectTransform>();
        exitCursor = CanVasExitCorsor.GetComponent<RectTransform>();

        //inputのタグ
        inputScript = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();

        GoalPos = new Vector3(-2.33f, 4.1f, -25.36f);

        //カメラの位置
        StartPos = new Vector3(-5.38f, 2.5f, -22.96f);

        StartRotate = transform.rotation;

        StartRotate = Quaternion.Euler(-7.5f, 23.0f, 0.0f);

        //今現在どこのシーンにいるかどうかの初期化
        nowPosition = 1;

        //カーソルが今現在どこにるかどうか
        CursorPosition = 1;

        KeyDownflag = false;
        KeyDownflag2 = false;

        BGMslider = GameObject.FindWithTag("BGMSlider").GetComponent<Slider>();
        SEslider = GameObject.FindWithTag("SESlider").GetComponent<Slider>();

        BGMslider.value = 1.0f;
        SEslider.value = 1.0f;

        maxvolume = 1;

        //VolumePosistion = 1;
        //BGMVolume.SetActive(true);
        //BGMVolume2.SetActive(false);
        //BGMVolume3.SetActive(false);
        //BGMVolume4.SetActive(false);
        //BGMVolume5.SetActive(false);

        cameramove = false;

    }

    void Update()
    {

        //ここからオプション押したらの処理

        switch (nowPosition)
        {
            case 1:
                Title();
                break;
            case 2:
                SelectStart();
                break;
            case 3:
                Option();
                break;
            case 4:
                SelectExit();
                break;
        }
    }


    void Title()
    {
        bool ClickOK = inputScript.GetMenuOKState();
        bool ClickCancel = inputScript.GetMenuCancelState();
        Vector2 CursorVec = inputScript.GetMenuSelectFloat();

        if (ClickOK && !KeyDownflag)
        {
            KeyDownflag = true;
            GameObjectInvert(CanvasAbutton);
            GameObjectInvert(CanvasStart);
            nowPosition = 2;
            CursorPosition = 1;
            if (audioDebug)
            {
                audioManager.PlayTitleDecideSE();
            }
        }
        //else if (ClickCancel && !KeyDownflag)
        //{
        //    KeyDownflag = true;
        //    nowPosition = 4;
        //    GameObjectInvert(CanvasAbutton);
        //    GameObjectInvert(CanvasExit);
        //}

        if (!ClickOK && !ClickCancel)
        {
            KeyDownflag = false;
        }
        /*bool ClickOK = inputScript.GetMenuOKState();

        if (ClickOK == true)
        {
            nowPosition = 2;
            ClickOK = false;
            cameramove = true;

        }
        //ClickOK = true;
        bool ClickCancel = inputScript.GetMenuCancelState();
        //if (CursorVec.x == 1.0f)
        if (ClickCancel == true)
        {
            ClickCancel = false;
            //cameramove = true;*/
    }



    void SelectStart()
    {
        bool ClickOK = inputScript.GetMenuOKState();
        bool ClickCancel = inputScript.GetMenuCancelState();
        Vector2 CursorVec = inputScript.GetMenuSelectFloat();

        if (ClickOK && !KeyDownflag)
        {
            ToGameStart = true;
            if (audioDebug)
            {
                audioManager.PlayTitleDecideSE();
            }
        }

        if (CursorVec.y == 1.0f && !KeyDownflag)
        {
            if (CursorPosition > 1)
            {
                CursorPosition -= 1;
                KeyDownflag = true;
                if (audioDebug)
                {
                    audioManager.PlayCursorSE();
                }
            }
        }
        else if (CursorVec.y == -1.0f && !KeyDownflag)
        {
            if (CursorPosition < 4)
            {
                CursorPosition += 1;
                KeyDownflag = true;
                if (audioDebug)
                {
                    audioManager.PlayCursorSE();
                }
            }
        }

        if(CursorVec.y == 0.0f && !ClickOK && !ClickCancel)
        {
            KeyDownflag = false;
        }
        if (nowPosition == 2)
        {
            CursorPos(startCursor, CursorPosition, nowPosition);
        }

        if (ToGameStart)
        {
            StartGame(CursorPosition);
        }

        if (cameramove)
        {
            CameraMove(target, GoalPos);
        }
    }


    void CursorPos(RectTransform cursorpos, int posState , int sceneState)
    {
        if (sceneState == 2)
        {

            switch (posState)
            {
                case 1:
                    cursorpos.localPosition = new Vector3(506.0f, -146.0f, 0.0f);
                    break;
                case 2:
                    cursorpos.localPosition = new Vector3(506.0f, -302.0f, 0.0f);
                    break;
                case 3:
                    cursorpos.localPosition = new Vector3(506.0f, -436.0f, 0.0f);
                    break;
                case 4:
                    cursorpos.localPosition = new Vector3(506.0f, -577.0f, 0.0f);
                    break;
            }
        }
        else if (sceneState == 3)
        {
            switch (posState)
            {
                case 1:
                    cursorpos.localPosition = new Vector3(-133.86f, 86.0f, 0.0f);
                    break;
                case 2:
                    cursorpos.localPosition = new Vector3(-133.86f, 35.0f, 0.0f);
                    break;
                case 3:
                    cursorpos.localPosition = new Vector3(-50.6f, -20.0f, 0.0f);
                    break;

            }
        }
        else if (sceneState == 4)
        {
            switch (posState)
            {
                case 1:
                    cursorpos.localPosition = new Vector3(143.0f, -413.0f, 0.0f);
                    break;
                case 2:
                    cursorpos.localPosition = new Vector3(521.0f, -413.0f, 0.0f);
                    break;
            }
        }
    }

    void StartGame(int posState)
    {
        switch (posState)
        {
            case 1: // はじめから
                /*
                entranceに向かって歩いて行く
                */
                playerMove.PlayerWork();
                break;
            case 2: // つづきから
                    /*
                    セーブデータがあるかどうか確認
                    if()
                    {
                        playerMove.PlayerWork();
                    }
                    else
                    {
                        なければないよを表示
                    }
                     */
                break;

            case 3: // オプション
                cameramove = true;
                GameObjectInvert(CanvasTitle);
                CursorPosition = 1;
                GameObjectInvert(CanvasStart);
                ToGameStart = false;
                break;
            case 4: // 戻る
                nowPosition = 4;
                GameObjectInvert(CanvasExit);
                GameObjectInvert(CanvasStart);
                ToGameStart = false;
                break;
                
        }
        
    }


    // カメラ移動処理
    void CameraMove(Transform target, Vector3 goal)
    {
        Vector3 temp = this.transform.position;
        Vector3 targetPosition = target.position;


        if (this.transform.position.y != target.position.y)
        {
            targetPosition = new Vector3(target.position.x, this.transform.position.y, target.position.z);
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - this.transform.position);
        //4.0f,1.5f

        float velocity = 2.2f;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * (1.3f + (1.3f * velocity)));
        this.transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime / (3.6f - (1.0f * velocity)));

        if (temp == this.transform.position)
        {
            nowPosition = 3;
            cameramove = false;           
        }
    }


    // カメラスタートポジションに戻す処理
    void StartPosCamera()
    {
        Vector3 temp = this.transform.position;
            //target = start;

            float velocity01 = 2.2f;

            this.transform.rotation = Quaternion.Lerp(transform.rotation, StartRotate, Time.deltaTime * (1.3f + (1.3f * velocity01)));
            this.transform.position = Vector3.MoveTowards(transform.position, StartPos, speed * Time.deltaTime / (3.6f - (1.0f * velocity01)));

        if (temp == this.transform.position)
        {
            nowPosition = 2;
            cameramove = false;
            GameObjectInvert(CanvasStart);
            GameObjectInvert(CanvasTitle);
        }
    }


    void Option()
    {
        bool ClickCancel = inputScript.GetMenuCancelState();

        if (ClickCancel == true)
        {
            ClickCancel = false;
            cameramove = true;
            if (audioDebug)
            {
                audioManager.PlayCancelSE();
            }
        }

        if (cameramove)
        {
            StartPosCamera();
        }

        //カーソルの変更変数
        Vector2 CursorVec = inputScript.GetMenuSelectFloat();


        /////////////////////////
        //カーソルの移動入力受付
        ////////////////////////
        if (CursorVec.y == 1.0f && !KeyDownflag)
        {
            if (CursorPosition > 1)
            {
                CursorPosition -= 1;
            }

            if (audioDebug)
            {
                audioManager.PlayCursorSE();
            }
            Debug.Log("上に行く");
            KeyDownflag = true;
        }
        else if (CursorVec.y == -1.0f && !KeyDownflag)
        {
            if (CursorPosition < 3)
            {
                CursorPosition += 1;
                KeyDownflag = true;
            }

            if (audioDebug)
            {
                audioManager.PlayCursorSE();
            }
        }

        else if (CursorVec.y == 0.0f)
        {
            KeyDownflag = false;
        }




        /////////////////////
        ///音量調整の入力受付
        /////////////////////

        var temp = bgmvolume;
        var temp2 = sevolume;
        if (CursorVec.x == 1.0f && !KeyDownflag2)
        {
            if (CursorPosition == 1 )
            {
                if (bgmvolume < 6)
                {
                    bgmvolume = VolumeSlide(bgmvolume, 1);
                }
            }
            else if (CursorPosition == 2)
            {
                if (sevolume < 6)
                {
                    sevolume = VolumeSlide(sevolume, 1);
                }
            }

            KeyDownflag2 = true;
        }
        else if (CursorVec.x == -1.0f && !KeyDownflag2)
        {

            if (CursorPosition == 1)
            {
                if (bgmvolume > 0)
                {
                    bgmvolume = VolumeSlide(bgmvolume, -1);
                }
            }
            else if (CursorPosition == 2)
            {
                if (sevolume > 0)
                {
                    sevolume = VolumeSlide(sevolume, -1);

                }
            }


            KeyDownflag2 = true;

        }

        else if (CursorVec.x == 0.0f)
        {
            KeyDownflag2 = false;
        }

        if(temp != bgmvolume || temp2 != sevolume)
        {
            VolumeSetting(CursorPosition, bgmvolume , sevolume);
        }
        //////////////////////////////
        //カーソルの現在位置の管理処理
        //////////////////////////////
        switch (CursorPosition)
        {
            case 1:
                cursor.localPosition = new Vector3(-133.86f, 86.0f, 0.0f);
                break;
            case 2:
                cursor.localPosition = new Vector3(-133.86f, 35.0f, 0.0f);
                break;
            case 3:
                cursor.localPosition = new Vector3(-50.6f, -20.0f, 0.0f);

                break;

        }
        
        //////////////////
        ////音量調節の処理
        //////////////////
        //if (CursorVec.x == 1.0f)
        //{
        //    switch (VolumePosistion)
        //    {
        //        //ボリューム調整（レベル１）
        //        case 1:
                     
        //            break;
        //        //ボリューム調整（レベル２）
        //        case 2:


        //            break;
        //        //ボリューム調整（レベル３）
        //        case 3:


        //            break;
        //        //ボリューム調整（レベル４）
        //        case 4:

        //            break;
        //        //ボリューム調整（レベル５）
        //        case 5:

        //            break;


        //    }

           
        //}

        //bool ClickCancel = inputScript.GetMenuCancelState();
        ////if (CursorVec.x == 1.0f)
        //if (ClickCancel == true)
        //{
        //    ClickCancel = false;
        //    //cameramove = true;
        //    if (cameramove2 == true)
        //    {
                
        //        //target = start;

        //        float velocity01 = 2.2f;

        //        this.transform.rotation = Quaternion.Lerp(transform.rotation, StartRotate, Time.deltaTime * (1.3f + (1.3f * velocity01)));
        //        this.transform.position = Vector3.MoveTowards(transform.position, StartPos, speed * Time.deltaTime / (3.6f - (1.0f * velocity01)));

        //        if(this.transform.position == StartPos)
        //        {

        //            nowPosition = 1;

        //        }
        //    }
        //    else
        //    {
        //        cameramove = false;
        //    }

        //}
        

    }

    int VolumeSlide ( int vol, int cnt)
    {
        return vol = vol + cnt; 
    }


    void VolumeSetting(int position , int BGMvol , int SEvol)
    {
        switch (position)
        {
            case 1:
                BGMslider.value = (float)BGMvol * 0.2f;

                if (audioDebug)
                {
                    audioManager.PlayCursorSE();
                }
            break;
            case 2:
                SEslider.value = (float)SEvol * 0.2f;

                if (audioDebug)
                {
                    audioManager.PlayCursorSE();
                }
            break;
        }
    }

    void SelectExit()
    {
        bool ClickOK = inputScript.GetMenuOKState();
        bool ClickCancel = inputScript.GetMenuCancelState();
        Vector2 CursorVec = inputScript.GetMenuSelectFloat();

        if (ClickCancel && !KeyDownflag)
        {
            KeyDownflag = true;
            GameObjectInvert(CanvasExit);
            nowPosition = 2;
            CursorPosition = 4;
            GameObjectInvert(CanvasStart);
            if (audioDebug)
            {
                audioManager.PlayCancelSE();
            }

        }
        else if (ClickOK && !KeyDownflag)
        {
            ExitGame(CursorPosition);
            KeyDownflag = true;
            if (audioDebug)
            {
                audioManager.PlayTitleDecideSE();
            }
        }


        if (CursorVec.x == -1.0f && !KeyDownflag)
        {
            CursorPosition = 1;
            KeyDownflag = true;
            if (audioDebug)
            {
                audioManager.PlayCursorSE();
            }
        }
        else if (CursorVec.x == 1.0f && !KeyDownflag)
        {
            CursorPosition = 2;
            KeyDownflag = true;
            if (audioDebug)
            {
                audioManager.PlayCursorSE();
            }
        }

        if (CursorVec.x == 0.0f && !ClickOK && !ClickCancel)
        {
            KeyDownflag = false;
        }
        if (nowPosition == 4)
        {
            CursorPos(exitCursor, CursorPosition, nowPosition);
        } 
    }

    
    void ExitGame(int posState)
    {
        if (posState == 1)
        {
            Application.Quit();
        }
        else if(posState == 2)
        {
            CursorPos(exitCursor, 1, nowPosition);
            GameObjectInvert(CanvasExit);
            nowPosition = 2;
            CursorPosition = 4;
            GameObjectInvert(CanvasStart);
            
        }
    }

    // ゲームオブジェクトの状態(true or false)を反転
    void GameObjectInvert(GameObject obj)
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
}



 //Vector3 startPosition = target.position;
            ////nowPosition = 1;

            ////transform.position = StartPos;
            //if (this.transform.position.y != target.position.y)
            //{
            //    startPosition = new Vector3(target.position.x, this.transform.position.y, target.position.z);
            //}
            //Quaternion startRotate = Quaternion.LookRotation(startPosition - this.transform.position);

            //transform.rotation = Quaternion.Lerp(start.rotation, startRotate, Time.deltaTime * (1.3f + (1.3f * velocity)));
            //this.transform.position = Vector3.MoveTowards(transform.position, StartPos, speed * Time.deltaTime / (3.6f - (1.0f * velocity)));

        //カメラ元に戻る処理出来あがり
        //if (CursorVec.x == 1.0f)
        //{
        //    cameramove = false;
        //    target = start;
          

        //    float velocity01 = 2.2f;

        //    this.transform.rotation = Quaternion.Lerp(transform.rotation, StartRotate, Time.deltaTime * (1.3f + (1.3f * velocity01)));
        //    this.transform.position = Vector3.MoveTowards(transform.position, StartPos, speed * Time.deltaTime / (3.6f - (1.0f * velocity01)));
           // this.transform.rotation = StartRotate;

        //}
        //ここまで


        //cameramove = true;


        //if (CursorVec.x == -1.0f)
        //{

        //}

        //キャンセルを押したらカメラを戻す処理
        //bool ClickCancel = inputScript.GetMenuCancelState();


        //カメラを戻す処理ここまで
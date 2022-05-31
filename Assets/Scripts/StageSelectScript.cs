using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectScript : MonoBehaviour
{
    public bool dFlag;
    [SerializeField] Vector3[] w1sPosition;
    [SerializeField] Vector3[] w2sPosition;
    [SerializeField] Vector3[] w3sPosition;
    [SerializeField] Vector3[] w4sPosition;
    [SerializeField] Vector3[] w5sPosition;

    private enum ScaleState
    {
        In,Out
    }

    [SerializeField] float fadespeed;

    private InputManager inputScript;
    private bool KeyDownFlag;

    int stageSelectState;
    private int wNum;
    private int sNum;

    private bool isMove = false;
    private bool isZoom = false;
    private bool isStart = false;
    private bool isScale = false;

    GameObject stageSelectCursor;
    private RectTransform cursorPosition;
    Image selectCursorImage;

    CanvasGroup stageStartObject;
    private RectTransform cursorPosition2;
    Image startCursorImage;

    CanvasGroup exit;
    Image exitCursor;


    [SerializeField] float cursolSpeed = 0.75f;

    GameObject movedoor;
    [SerializeField] float turnRotation = 72.0f;
    int MovedoorState;
    [SerializeField]float turnspeed = 2.0f;

    ZoomCamera zoomCameraScript;

    [SerializeField] private bool audioDebug;
    AudioManager audioScript;


    private Tweener _shakeTweener;
    private Vector3 _initPosition;
    
    List<StageList> stageList = new List<StageList>();

    [Header("開けるドアをセット")]
    [SerializeField] public GameObject cube1;
    [SerializeField] public GameObject cube2;
    [SerializeField] public GameObject cube3;
    [SerializeField] public GameObject cube4;
    [SerializeField] public GameObject cube5;

    [Header("ドアをどこまで開けるか")]
    [SerializeField] float openHeight;

    [Header("ドアを開けるまでにかかる時間")]
    [SerializeField] float openSpeed;

    UI_Stone uistoneScript;
    StageName stageNameScript;

    public int GetStoneNum(int w, int s)
    {
        int index = ((w - 1) * 5) + (s - 1);
        return stageList[index].StoneNum;
    }

    void Start()
    {
        inputScript = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        zoomCameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ZoomCamera>();
        movedoor = GameObject.FindWithTag("movedoor");

        isMove = false;
        isZoom = false;
        isStart = false;

        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        KeyDownFlag = false;
        stageSelectState = 1;
        wNum = 1;
        sNum = 1;

        stageSelectCursor = GameObject.FindGameObjectWithTag("Cursor");
        selectCursorImage = stageSelectCursor.transform.GetChild(0).GetComponent<Image>();
        stageSelectCursor.SetActive(false);
        cursorPosition = selectCursorImage.GetComponent<RectTransform>();

        stageStartObject = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasGroup>();
        startCursorImage = stageStartObject.transform.GetChild(8).GetComponent<Image>();
        stageStartObject.alpha = 1;
        stageStartObject.gameObject.transform.localScale = new Vector3(0, 0, 0);
        cursorPosition2 = startCursorImage.GetComponent<RectTransform>();

        exit = GameObject.FindGameObjectWithTag("Entrance").GetComponent < CanvasGroup > ();
        exitCursor = exit.transform.GetChild(4).GetComponent<Image>();
        exit.transform.localScale = new Vector3(0, 0, 0);

        uistoneScript = stageStartObject.GetComponent<UI_Stone>();
        stageNameScript = GameObject.FindGameObjectWithTag("Text").GetComponent<StageName>();

        // �����ʒu��ێ�
        _initPosition = transform.position;
        
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
                            case 1: numberStone = 5;
                                break;
                            case 2: numberStone = 5;
                                break;
                            case 3: numberStone = 5;
                                break;
                            case 4: numberStone = 4;
                                break;
                            case 5: numberStone = 5;
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

    // Update is called once per frame
    void Update()
    {
        switch (stageSelectState)
        {
            case 1: // ���[���h�Z���N�g
                WorldSelect();
                break;

            case 2: // �X�e�[�W�Z���N�g
                if (wNum != 5)
                {
                    StageSelect();
                }
                else
                {
                    SelectExit();
                }
                break;

            case 3:

                    StageStart();
                break;

            case 4:
                if (isMove)
                {
                    UpDoor(wNum);
                }
                else if(isStart && !isMove)
                {
                    LoadStage(wNum, sNum);
                }
                break;
        }
    }

    void WorldSelect()
    {
        Vector2 DoorVec = inputScript.GetMenuSelectFloat();
        bool SelectOK = inputScript.GetMenuOKState();

        if (!isMove && !isZoom)
        {
            if (DoorVec.x == -1.0f && !KeyDownFlag)
            {
                MovedoorState = (MovedoorState - 1 + 5) % 5;
                
                KeyDownFlag = true;
                isMove = true;

                if (audioDebug)
                {
                    audioScript.PlayRotateSE();
                }
            }
            else if (DoorVec.x == 1.0f && !KeyDownFlag)
            {
                MovedoorState = (MovedoorState + 1) % 5;
                KeyDownFlag = true;
                isMove = true;

                if (audioDebug)
                {
                    audioScript.PlayRotateSE();
                }
            }
            else if (DoorVec.x == 0.0f)
            {
                KeyDownFlag = false;
            }


            if (SelectOK && !KeyDownFlag)
            {
                isZoom = true;
                if (audioDebug)
                {
                    audioScript.PlayDecideSE();
                }
            }
        }
        if (isMove)
        {
            Invoke("Movedoor", 0.5f);
        }

        if (isZoom)
        {
            ZoomCamera();
        }
    }


    void Movedoor()
    {
        var temp = movedoor.transform.eulerAngles.y;
        if (MovedoorState > 0)
        {
            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, Mathf.CeilToInt(movedoor.transform.localRotation.y - MovedoorState * turnRotation), 0.0f), turnspeed);
        }
        else if (MovedoorState == 0)
        {
            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, 0.0f, 0.0f), turnspeed);
        }

        if (movedoor.transform.eulerAngles.y != temp)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

    void ZoomCamera()
    {
        var temp = new Vector3(0f, 0.30f, 0f);
        
        zoomCameraScript.ZoomFov(zoomCameraScript.m_Camera, 12.0f, 2.0f);
        zoomCameraScript.m_Camera.transform.DOMove(new Vector3(0f, 0.30f, 0f), 1.0f);
        
        if (this.transform.position == temp)
        {
            stageSelectState = 2;
            wNum = System.Math.Abs(MovedoorState) + 1;
            sNum = 1;
            SetFirstStage(cursorPosition, wNum, sNum);
            if (wNum != 5)
            {
                stageSelectCursor.SetActive(true);
            }
            else
            {
                isScale = true;
            }
            isZoom = false;
            KeyDownFlag = false;
        }
    }

    void StageSelect()
    {
        Vector2 DoorVec = inputScript.GetMenuSelectFloat();
        bool SelectOK = inputScript.GetMenuOKState();
        bool SelectCancel = inputScript.GetMenuCancelState();
        var temp = cursorPosition.localPosition;

        if (!isScale)
        {
            if (SelectOK && !KeyDownFlag)
            {
                isScale = true;
                stageSelectCursor.SetActive(false);
                uistoneScript.Init(wNum , sNum);
                stageNameScript.SetStageName(wNum, sNum);
            }
            else if (SelectCancel && !KeyDownFlag)
            {
                stageSelectState = 1;
                stageSelectCursor.SetActive(false);
                zoomCameraScript.ZoomFov(zoomCameraScript.m_Camera, 20.0f, 2.0f);
                zoomCameraScript.m_Camera.transform.DOMove(new Vector3(0.0f, 0.005f, 1.626f), 1.0f);
                KeyDownFlag = true;                
            }

            if ((DoorVec.x == 1.0f) && !KeyDownFlag)
            {
                if (sNum < 5)
                {
                    sNum = sNum + 1;
                }
                KeyDownFlag = true;
            }
            else if ((DoorVec.x == -1.0f) && !KeyDownFlag)
            {
                if (sNum > 1)
                {
                    sNum = sNum - 1;
                }
                KeyDownFlag = true;
            }
        }
        CursorPos(cursorPosition, wNum, sNum);

        if (isScale)
        {
            ScaleUI(stageStartObject.gameObject, fadespeed, isScale, ScaleState.In);
        }
        
        if (stageStartObject.gameObject.transform.localScale.x >= 1)
        {
            stageSelectState = 3;
            isScale = false;
        }

        if (temp == cursorPosition.localPosition && DoorVec.x == 0.0f)
        {
            KeyDownFlag = false;
        }
    }

    void SetFirstStage(RectTransform cursorpos, int worldNo, int stageNo)
    {
        switch (worldNo)
        {
            case 1:
                cursorpos.localPosition = w1sPosition[stageNo - 1];
                break;
            case 2:
                cursorpos.localPosition = w2sPosition[stageNo - 1];
                break;
            case 3:
                cursorpos.localPosition = w3sPosition[stageNo - 1];
                break;
            case 4:
                cursorpos.localPosition = w4sPosition[stageNo - 1];
                break;
            case 5:
                cursorpos.localPosition = new Vector3(-423.0f, -183.0f);
                break;
        }
    }

    // �J�[�\���ړ�
    void CursorPos(RectTransform cursorpos,  int worldNo , int stageNo)
    {
        switch (worldNo )
        {
            case 1:
                cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, w1sPosition[stageNo - 1], cursolSpeed);
                break;
            //switch (stageNo)
            //{
            //    case 1:
            //        cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-241.0f, -160.0f, 0.0f), cursolSpeed);
            //        break;
            //    case 2:
            //        cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-141.0f, 240.0f, 0.0f), cursolSpeed);
            //        break;
            //    case 3:
            //        cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-37.0f, -165.0f, 0.0f), cursolSpeed);
            //        break;
            //    case 4:
            //        cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(91.0f, 201.0f, 0.0f), cursolSpeed);
            //        break;
            //    case 5:
            //        cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(179.0f, -202.0f, 0.0f), cursolSpeed);

            //        break;
            //}

            case 2:
                cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, w2sPosition[stageNo - 1], cursolSpeed);
                break;
            case 3:
                cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, w3sPosition[stageNo - 1], cursolSpeed);
                break;
            case 4:
                cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, w4sPosition[stageNo - 1], cursolSpeed);
                break;
            case 5:
                cursorpos.localPosition = new Vector3( w5sPosition[stageNo - 1].x , w5sPosition[stageNo - 1].y, w5sPosition[stageNo - 1].z); ;
                break;
        }
    }

    void StageStart()
    {
        Vector2 DoorVec = inputScript.GetMenuSelectFloat();
        bool SelectOK = inputScript.GetMenuOKState();
        bool SelectCancel = inputScript.GetMenuCancelState();
        if (!isScale)
        {
            if (DoorVec.y == 1.0f)
            {
                cursorPosition2.localPosition = new Vector3(403.0f, -183.0f);
            }
            else if (DoorVec.y == -1.0f)
            {
                cursorPosition2.localPosition = new Vector3(403.0f, -293.0f);
            }

            if (SelectCancel)
            {
                isScale = true;
                stageSelectCursor.SetActive(true);
            }

            if (SelectOK && cursorPosition2.localPosition.y == -183.0f)
            {
                stageSelectState = 4;
                stageStartObject.gameObject.SetActive(false);
                isMove = true;
            }
            else if (SelectOK && cursorPosition2.localPosition.y == -293.0f)
            {

                isScale = true;
                stageSelectCursor.SetActive(true);
            }
        }

        ScaleUI(stageStartObject.gameObject, fadespeed, isScale, ScaleState.Out);


        if (stageStartObject.gameObject.transform.localScale.x == 0)
        {
            stageSelectState = 2;
            uistoneScript.StoneSetFalse(wNum, sNum);
            isScale = false;
        }
    }

    void LoadStage(int WorldNo , int StageNo)
    {
        isStart = false;
        
        zoomCameraScript.ZoomFov(zoomCameraScript.m_Camera, 0.8f, 5.0f);
        
        // StageLoad�ύX�\��
        ClearInfoScript.instance.SetWorldStageNum(WorldNo, StageNo);
        
        StartCoroutine(LoadScene(WorldNo, StageNo));
        
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    
    IEnumerator LoadScene(int world, int stage)
    {
        yield return new WaitForSeconds(3.0f);
        
        string sceneName = "Stage" + world + "-" + stage;
        FadeManager.Instance.LoadScene(sceneName, 1.0f);
    }

    void ScaleUI(GameObject obj , float speed, bool isScale , ScaleState scaleState)
    {
        if (isScale)
        {
            var size = obj.transform.localScale;

            if (size.x < 1.0f && scaleState == ScaleState.In)
            {
                obj.transform.localScale =new Vector3(obj.transform.localScale.x +speed * Time.deltaTime, obj.transform.localScale.y + speed * Time.deltaTime,0);
            }
            else if(size.x >= 1.0f && scaleState == ScaleState.In)
            {
                obj.transform.localScale = new Vector3(1, 1, 0);
                isScale = false;
            }

            if (size.x >0.0f && scaleState == ScaleState.Out)
            {
                obj.transform.localScale = new Vector3(obj.transform.localScale.x - speed * Time.deltaTime, obj.transform.localScale.y - speed * Time.deltaTime,0);
            }
            else if (size.x <= 0.0f && scaleState == ScaleState.Out)
            {
                obj.transform.localScale = new Vector3(0, 0, 0);
                isScale = false;
            }
        }
    }

    void UpDoor(int WorldNo)
    {
        switch (WorldNo)
        {
            case 1:
                cube1.transform.position += Vector3.up * Time.deltaTime;
                if (cube1.transform.position.y >= 3.0f)
                {
                    isMove = false;
                }
                break;
            case 2:
                cube2.transform.position += Vector3.up * Time.deltaTime;
                if (cube2.transform.position.y >= 3.0f)
                {
                    isMove = false;
                }
                break;
            case 3:
                cube3.transform.position += Vector3.up * Time.deltaTime;
                if (cube3.transform.position.y >= 3.0f)
                {
                    isMove = false;
                }
                break;
            case 4:
                cube4.transform.position += Vector3.up * Time.deltaTime;
                if (cube4.transform.position.y >= 3.0f)
                {
                    isMove = false;
                }
                break;
        }
        if (!isMove)
        {
            isStart = true;
        }
    }


    void SelectExit()
    {
        if (!isScale)
        {
            Vector2 CursoeVec = inputScript.GetMenuSelectFloat();
            bool SelectOK = inputScript.GetMenuOKState();
            bool SelectCancel = inputScript.GetMenuCancelState();

            if (CursoeVec.x == -1.0f && !KeyDownFlag)
            {
                CursorPos(exitCursor.rectTransform, wNum, 1);
                KeyDownFlag = true;
                audioScript.PlayCursorSE();
            }
            else if(CursoeVec.x == 1.0f && !KeyDownFlag)
            {
                CursorPos(exitCursor.rectTransform, wNum, 2);
                KeyDownFlag = true;
                audioScript.PlayCursorSE();
            }
            else if (CursoeVec.x == 0.0f)
            {
                KeyDownFlag = false;
            }

            if (SelectCancel && !KeyDownFlag)
            {
                KeyDownFlag = true;
                zoomCameraScript.ZoomFov(zoomCameraScript.m_Camera, 20.0f, 2.0f);
                zoomCameraScript.m_Camera.transform.DOMove(new Vector3(0.0f, 0.005f, 1.626f), 1.0f);
                stageSelectState = 1;
                exit.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            }
            if (SelectOK && !KeyDownFlag)
            {
                if (exitCursor.transform.localPosition.x == -423.0f)
                {
                    FadeManager.Instance.LoadScene("TitleScene", 1.0f);
                }
                else if (exitCursor.transform.localPosition.x == 227.0f)
                {
                    KeyDownFlag = true;
                    zoomCameraScript.ZoomFov(zoomCameraScript.m_Camera, 20.0f, 2.0f);
                    zoomCameraScript.m_Camera.transform.DOMove(new Vector3(0.0f, 0.005f, 1.626f), 1.0f);
                    stageSelectState = 1;
                    exit.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
        }
        else if(isScale)
        {
            ScaleUI(exit.gameObject, fadespeed, isScale, ScaleState.In);
            Debug.Log(exit.transform.localScale);
            if (exit.transform.localScale.x >= 1.0f)
            {
                isScale = false;
            }
        }
    }
}



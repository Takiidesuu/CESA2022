using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
    private enum FadeState
    {
        In, Out
    }

    [SerializeField] float fadespeed;

    private InputManager inputScript;
    private bool KeyDownFlag;

    int stageSelectState;
    private int wNum;
    private int sNum;

    private bool isRotating = false;
    private bool isZoom = false;
    private bool isStart = false;
    private bool isFade = false;

    [SerializeField] GameObject stageSelectCursor;
    private RectTransform cursorPosition;
    [SerializeField] Image selectCursorImage;

    [SerializeField] CanvasGroup stageStartObject;
    private RectTransform cursorPosition2;
    [SerializeField] Image startCursorImage;


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

    void Start()
    {
        inputScript = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        zoomCameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ZoomCamera>();
        movedoor = GameObject.FindWithTag("movedoor");

        isRotating = false;
        isZoom = false;
        isStart = false;

        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        KeyDownFlag = false;
        stageSelectState = 1;
        wNum = 1;
        sNum = 1;

        stageSelectCursor.SetActive(false);
        cursorPosition = selectCursorImage.GetComponent<RectTransform>();

        if (dFlag)
        {
            stageStartObject.alpha = 1;
            stageStartObject.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            stageStartObject.alpha = 0;
        }
        
        cursorPosition2 = startCursorImage.GetComponent<RectTransform>();

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

    // Update is called once per frame
    void Update()
    {
        switch (stageSelectState)
        {
            case 1: // ���[���h�Z���N�g
                WorldSelect();
                break;

            case 2: // �X�e�[�W�Z���N�g

                StageSelect();
                break;

            case 3:
                StageStart();

                break;

            case 4:
                if (isStart)
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

        if (!isRotating && !isZoom)
        {
            if (DoorVec.x == -1.0f && !KeyDownFlag)
            {
                MovedoorState = (MovedoorState + 5 - 1) % 5;
                
                KeyDownFlag = true;
                isRotating = true;

                if (audioDebug)
                {
                    audioScript.PlayRotateSE();
                }
            }
            else if (DoorVec.x == 1.0f && !KeyDownFlag)
            {
                MovedoorState = (MovedoorState + 1) % 5;
                KeyDownFlag = true;
                isRotating = true;

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
        if (isRotating)
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
            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, Mathf.CeilToInt(movedoor.transform.localRotation.y + MovedoorState * turnRotation), 0.0f), turnspeed);
        }
        else if(MovedoorState == 0)
        {
            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, 0.0f, 0.0f), turnspeed );
        }


        if (movedoor.transform.eulerAngles.y != temp)
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
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
            stageSelectCursor.SetActive(true);
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

        if (!isFade)
        {
            if (SelectOK && !KeyDownFlag)
            {
                isFade = true;
                stageSelectCursor.SetActive(false);
                
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

        if (dFlag)
        {
            ScaleUI(stageStartObject.gameObject, fadespeed, isFade, ScaleState.In);
        }
        else
        {
            FadeUI(stageStartObject, fadespeed, isFade, FadeState.Out);
        }

        if (dFlag)
        {
            if (stageStartObject.gameObject.transform.localScale.x >= 1)
            {
                stageSelectState = 3;
                isFade = false;
            }
        }
        else
        {
            if (stageStartObject.alpha >= 1)
            {
                stageSelectState = 3;
                isFade = false;
            }
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
                cursorpos.localPosition = w5sPosition[stageNo - 1];
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
            //case 5:
            //    cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, w5sPosition[stageNo - 1], cursolSpeed);
            //    break;
        }
        //else if (worldNo == 2)
        //{

        //    switch (stageNo)
        //    {
        //        case 1:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-241.0f, 255.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 2:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-19.0f, -19.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 3:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(187.0f, 298.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 4:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(207.0f, -471.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 5:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-282.0f, -463.0f, 0.0f), cursolSpeed);
        //            break;
        //    }
        //}
        //else if (worldNo == 3)
        //{

        //    switch (stageNo)
        //    {
        //        case 1:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-141.0f, 207.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 2:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-282.0f, -264.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 3:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-39.0f, -287.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 4:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(243.0f, -291.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 5:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(215.0f, 254.0f, 0.0f), cursolSpeed);
        //            break;
        //    }
        //}
        //else if (worldNo == 4)
        //{

        //    switch (stageNo)
        //    {
        //        case 1:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(187.0f, 256.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 2:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(180.0f, -118.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 3:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-14.0f, -505.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 4:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-222.0f, -136.0f, 0.0f), cursolSpeed);
        //            break;
        //        case 5:
        //            cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-211.0f, 258.0f, 0.0f), cursolSpeed);
        //            break;
        //    }
        //}
    }

    void StageStart()
    {
        Vector2 DoorVec = inputScript.GetMenuSelectFloat();
        bool SelectOK = inputScript.GetMenuOKState();
        bool SelectCancel = inputScript.GetMenuCancelState();
        if (!isFade)
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
                isFade = true;
                stageSelectCursor.SetActive(true);
            }

            if (SelectOK && cursorPosition2.localPosition.y == -183.0f)
            {
                stageSelectState = 4;
                stageStartObject.gameObject.SetActive(false);
                isStart = true;
            }
            else if (SelectOK && cursorPosition2.localPosition.y == -293.0f)
            {

                isFade = true;
                stageSelectCursor.SetActive(true);
            }
        }

        if (dFlag)
        {
            ScaleUI(stageStartObject.gameObject, fadespeed, isFade, ScaleState.Out);
        }
        else
        {
            FadeUI(stageStartObject, fadespeed, isFade, FadeState.In);
        }

        if (dFlag)
        {
            if (stageStartObject.gameObject.transform.localScale.x == 0)
            {
                stageSelectState = 2;
                isFade = false;
            }
        }
        else
        {
            if (stageStartObject.alpha <= 0)
            {
                stageSelectState = 2;
                isFade = false;
            }
        }    
    }

    void LoadStage(int WorldNo , int StageNo)
    {
        FadeManager.Instance.LoadScene("Stage1-1", 1.0f);
        isStart = false;
        
        // StageLoad�ύX�\��
        ClearInfoScript.instance.SetWorldStageNum(WorldNo, StageNo);
    }

    void FadeUI(CanvasGroup fadeobject , float fadespeed , bool isfade , FadeState fadeState)
    {
        if (isFade)
        {
            if (fadeState == FadeState.Out && fadeobject.alpha >= 1)
            {
                isfade = false;
            }
            else if (fadeState == FadeState.Out && fadeobject.alpha < 1)
            {
                isFade = true;
                fadeobject.alpha += fadespeed * Time.deltaTime;
            }

            if (fadeState == FadeState.In && fadeobject.alpha <= 0)
            {
                isfade = false;
            }
            else if (fadeState == FadeState.In && fadeobject.alpha > 0)
            {
                fadeobject.alpha -= fadespeed * Time.deltaTime;
            }
        }
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
            else if(size.x > 1.0f && scaleState == ScaleState.In)
            {
                obj.transform.localScale = new Vector3(1, 1, 0);
                isScale = false;
            }

            if (size.x >0.0f && scaleState == ScaleState.Out)
            {
                obj.transform.localScale = new Vector3(obj.transform.localScale.x - speed * Time.deltaTime, obj.transform.localScale.y - speed * Time.deltaTime,0);
            }
            else if (size.x < 0.0f && scaleState == ScaleState.Out)
            {
                obj.transform.localScale = new Vector3(0, 0, 0);
                isScale = false;
            }
        }
    }


    void UpDoor(int WorldNo)
    {
        
    }
}



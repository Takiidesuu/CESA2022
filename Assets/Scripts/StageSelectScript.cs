using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StageSelectScript : MonoBehaviour
{
    private InputManager inputScript;
    private bool KeyDownFlag;

    private int stageSelectState;
    private int wNum;
    private int sNum;

    private bool isRotating = false;
    private bool isZoom = false;
    private bool isStart = false;

    [SerializeField] GameObject stageSelectCursor;
    private RectTransform cursorPosition;
    [SerializeField] Image selectCursorImage;

    [SerializeField] GameObject stageStartObject;
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


    //float minAngle = 0;
    //float maxAngle = 0;
    //float tick ;
    // Start is called before the first frame update
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
        cursorPosition = stageSelectCursor.GetComponent<RectTransform>();

        stageStartObject.SetActive(false);
        cursorPosition2 = startCursorImage.GetComponent<RectTransform>();


        // �����ʒu��ێ�
        _initPosition = transform.position;
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
                MovedoorState = (MovedoorState + 1) % 5;
                KeyDownFlag = true;
                isRotating = true;
                //minAngle = movedoor.transform.eulerAngles.y;
                //maxAngle = minAngle + 72;
                //tick = 0.0f;

                if (audioDebug)
                {
                    audioScript.PlayRotateSE();
                }
            }
            else if (DoorVec.x == 1.0f && !KeyDownFlag)
            {
                MovedoorState = (MovedoorState - 1) % 5;
                KeyDownFlag = true;
                isRotating = true;
                //minAngle = movedoor.transform.eulerAngles.y;
                //maxAngle = minAngle - 72;
                //tick = 0.0f;

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
        //tick += 0.01f;
        //float angle = Mathf.LerpAngle(minAngle, maxAngle, tick + Time.deltaTime);
        //movedoor.transform.eulerAngles = new Vector3(0, angle, 0);


        var temp = movedoor.transform.eulerAngles.y;
        if (MovedoorState > 0)
        {

            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, Mathf.FloorToInt(movedoor.transform.localRotation.y + MovedoorState * turnRotation), 0.0f), turnspeed );
        }
        else if(MovedoorState == 0)
        {
            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, 0.0f, 0.0f), turnspeed );
        }
        else if (MovedoorState < 0)
        {
            movedoor.transform.rotation = Quaternion.RotateTowards(movedoor.transform.rotation, Quaternion.Euler(0, Mathf.CeilToInt(movedoor.transform.localRotation.y + MovedoorState * turnRotation), 0.0f), turnspeed );
        }


        if (/*tick < 1.0f */ movedoor.transform.eulerAngles.y != temp)
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
            //// �O��̏������c���Ă���Β�~���ď����ʒu�ɖ߂�
            //if (_shakeTweener != null)
            //{
            //    _shakeTweener.Kill();
            //    gameObject.transform.position = _initPosition;
            //}
            //// �h��J�n
            //_shakeTweener = gameObject.transform.DOShakePosition(1.0f,1.0f,1,20.0f,false,true);
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

        if (SelectOK && !KeyDownFlag)
        {
            stageSelectState = 3;
            stageStartObject.SetActive(true);
            stageSelectCursor.SetActive(false);
;        }
        else if (SelectCancel && !KeyDownFlag)
        {
            stageSelectState = 1;
            stageSelectCursor.SetActive(false);
            zoomCameraScript.ZoomFov(zoomCameraScript.m_Camera, 20.0f, 2.0f);
            zoomCameraScript.m_Camera.transform.DOMove(new Vector3(0.0f, 0.005f, 1.626f), 1.0f);
            KeyDownFlag = true;
        }


        if ((DoorVec.x == 1.0f || DoorVec.y == -1.0f) && !KeyDownFlag)
        {
            if (sNum < 5)
            {
                sNum = sNum + 1;
            }
            KeyDownFlag = true;
        }
        else if ((DoorVec.x == -1.0f || DoorVec.y == 1.0f) && !KeyDownFlag)
        {
            if (sNum > 1)
            {
                sNum = sNum - 1;
            }
            KeyDownFlag = true;
        }

        CursorPos(cursorPosition, wNum, sNum);

        if (temp == cursorPosition.localPosition && DoorVec.x == 0.0f)
        {
            KeyDownFlag = false;
        }
    }


    // �J�[�\���ړ�
    void CursorPos(RectTransform cursorpos,  int worldNo , int stageNo)
    {
        if (worldNo == 1)
        {

            switch (stageNo)
            {
                case 1:
                    cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-241.0f, -160.0f, 0.0f), cursolSpeed);
                    break;
                case 2:
                    cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-138.0f, 243.0f, 0.0f), cursolSpeed);
                    break;
                case 3:
                    cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(-32.0f, -143.0f, 0.0f), cursolSpeed);
                    break;
                case 4:
                    cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(96.0f, 212.0f, 0.0f), cursolSpeed);
                    break;
                case 5:
                    cursorpos.localPosition = Vector3.MoveTowards(cursorpos.localPosition, new Vector3(187.0f, -178.0f, 0.0f), cursolSpeed);
                    break;
            }
        }
        
    }

    void StageStart()
    {
        Vector2 DoorVec = inputScript.GetMenuSelectFloat();
        bool SelectOK = inputScript.GetMenuOKState();

        if (DoorVec.y == 1.0f)
        {
           cursorPosition2.localPosition = new Vector3 (403.0f, -179.0f);
        }
        else if(DoorVec.y == -1.0f)
        {
            cursorPosition2.localPosition = new Vector3(403.0f, -293.0f);
        }

        if (SelectOK && cursorPosition2.localPosition.y ==  -179.0f)
        {
            isStart = true;
        }
        else if (SelectOK && cursorPosition2.localPosition.y == -293.0f)
        {
            stageStartObject.SetActive(false);
            stageSelectCursor.SetActive(true);
        }
    }


    void LoadStage(int WorldNo , int StageNo)
    {
        FadeManager.Instance.LoadScene("Stage1-1", 1.0f);
        isStart = false;

        // StageLoad�ύX�\��
    }


    void UpDoor(int WorldNo)
    {
        
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeScript : MonoBehaviour
{
    /* [Header("剥がししろ")]
    [SerializeField] bool upLeftPoint = false;
    [SerializeField] bool downLeftPoint = false;
    [SerializeField] bool upRightPoint = false;
    [SerializeField] bool downRightPoint = false; */

    [Header("戻る速度")]
    [SerializeField] float resetSpeed = 2.0f;
    
    [Header("めくるMAX角度 (90以上にしたら、自動で90になる)")]
    [SerializeField] float maxAngle = 60.0f;

    //選ばれたかどうかを示すマテリアル
    [Header("選ばれるかを示す色のマテリアル")]
    [SerializeField] Material selectMat;
    [SerializeField] Material unselectMat;

    GameObject[] pointObj;      //剥がししろのオブジェクト
    BoxCollider[] pointCol;     //剥がししろのコライダー

    //敵の変数を作って全てnullにする
    private GameObject Dguu;
    private GameObject Spyder;
    private GameObject KabeKnight;

    //敵のスクリプトを変数にする
    DouzouScript douzouScript;
    SpyderScript spyderScript;
    KnightScript knightScript;

    //敵のフラグ管理
    private bool DouzouGetflag;
    private bool SpyderGetflag;
    private bool KnightGetflag;

    MovePlayer playerScript;
    TapeBlock blockScript;

    private Vector3 tapeSize = Vector3.zero;    //テープのサイズ

    public class JointData
    {
        public GameObject obj;
        public Quaternion startingAngle;
    }
    
    private Vector3 startingScale;
    
    private JointData[] jointObj;      //関節のオブジェクト
    int jointNum = 5;           //関数の数

    private int direction = 0;          //剥がす方向

    private bool inRange = false;       //プレイヤーが範囲内にいるか
    private bool beingPulled = false;   //引っ張られてるか
    private bool startPulling = false;  //剥がす処理を始めるか
    private bool returnForm = false;    //形を戻すか

    private bool fastPull = false;      //一気に剥がすか
    private float pullSpeed = 0.0f;     //剥がす速度
    private float angleInfo = 0.0f;     //剥がす処理のための角度格納用変数
    
    private float targetAngle = 0.0f;
    private float nextPullSpeed = 0.0f;
    
    private List<GameObject> collidingTapes = new List<GameObject>();
    
    private bool canPull = true;

    //剥がす方向を指定する
    public void SetDirection(int dir)
    {
        direction = dir;
    }

    //引っ張られる情報を指定する
    public void SetPull(bool pulling)
    {
        beingPulled = pulling;
    }
    
    public bool CanPull()
    {
        return canPull;
    }

    //剥がす速度を指定する
    public void SetSpeed(bool mode, float speedS)
    {
        if (!startPulling)
        {
            fastPull = mode;

            startPulling = true;        //剥がす処理を開始
            
            jointNum = 5;
            angleInfo = jointObj[0].obj.transform.localRotation.eulerAngles.z;
            
            pullSpeed = speedS;
            
            if (direction == 2 || direction == 3)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z * -1.0f);
            }
            
            canPull = false;
        }
    }
    
    public void StopPulling()
    {
        targetAngle = jointObj[jointNum].startingAngle.z;
        angleInfo = jointObj[jointNum].obj.transform.localRotation.eulerAngles.z;
        
        returnForm = true;
    }

    private void Awake()
    {
        //テープのサイズを取得
        tapeSize = transform.GetChild(5).GetComponent<Renderer>().bounds.size;

        //テープコライダーのサイズをテープのサイズで決める
        Vector3 colSize = new Vector3(1.0f, 4.0f, 1.0f);
        
        jointNum = 5;
        
        if (maxAngle > 90)
        {
            maxAngle = 90.0f;
        }

        this.GetComponent<BoxCollider>().size = colSize;    //テープコライダーのサイズを指定
        this.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.0f, 0.0f);   //テープコライダーの位置を指定
    }

    // Start is called before the first frame update
    void Start()
    {
        var tapeLength = tapeSize.magnitude;
        float tapeHeight;
        
        startingScale = transform.localScale;
        
        if (this.transform.parent.localRotation.eulerAngles.z == 90 || this.transform.parent.localRotation.eulerAngles.z == -90)
        {
            tapeHeight = tapeSize.x;
        }
        else
        {
            tapeHeight = tapeSize.y;
        }
        
        //剥がししろ用の変数を初期化
        pointObj = new GameObject[4];
        pointCol = new BoxCollider[4];

        //敵の変数をタグ付け
        Dguu = GameObject.FindWithTag("douzou");
        Spyder = GameObject.FindWithTag("spyder");
        KabeKnight = GameObject.FindWithTag("soad");

        DouzouGetflag = false;
        SpyderGetflag = false;
        KnightGetflag = false;
        


        //関節用の変数を初期化
        jointObj = new JointData[7];
        
        var r = new JointData();
        r.obj = this.transform.GetChild(4).gameObject;
        r.startingAngle = r.obj.transform.localRotation;
        jointObj[0] = r;
        
        angleInfo = jointObj[0].obj.transform.localRotation.eulerAngles.z;

        for (int a = 1; a < 7; a++)
        {
            var p = new JointData();
            
            //その次の関節の情報を次々と入れる
            Transform nextJointChild = jointObj[a - 1].obj.transform.GetChild(0);
            p.obj = nextJointChild.gameObject;
            p.startingAngle = p.obj.transform.localRotation;
            
            jointObj[a] = p;
        }

        for (int a = 0; a < 4; a++)
        {
            Transform childObj = transform.GetChild(a);

            pointObj[a] = childObj.gameObject;
            pointObj[a].transform.GetChild(0).gameObject.transform.localScale = new Vector3(tapeLength / (tapeLength * 40.0f), tapeHeight / (tapeHeight * 10.0f), tapeSize.z);

            pointCol[a] = pointObj[a].GetComponent<BoxCollider>();
            pointCol[a].center = new Vector3(0.0f, 0.0f, -4.75f);
        }

        //左上
        pointObj[0].transform.localPosition = new Vector3(0.569f, 0.0f, 0.344f * -1.0f);
        pointCol[0].size = new Vector3(0.1f, 1.0f, 9.0f);
        //pointObj[0].SetActive(upLeftPoint);

        //左下
        pointObj[1].transform.localPosition = new Vector3((0.569f - 0.25f) * -1.0f, 0.0f, 0.344f * -1.0f);
        pointCol[1].size = new Vector3(0.08f, 0.5f, 9.0f);
        //pointObj[1].SetActive(downLeftPoint);

        //右上
        pointObj[2].transform.localPosition = new Vector3(0.569f, 0.0f, 0.344f);
        pointCol[2].size = new Vector3(0.1f, 1.0f, 9.0f);
        //pointObj[2].SetActive(upRightPoint);

        //右下
        pointObj[3].transform.localPosition = new Vector3((0.569f - 0.25f) * -1.0f, 0.0f, 0.344f);
        pointCol[3].size = new Vector3(0.08f, 0.5f, 9.0f);
        //pointObj[3].SetActive(downRightPoint);
        
        if (this.transform.parent.localRotation.eulerAngles.z == 90 || this.transform.parent.localRotation.eulerAngles.z == -90)
        {
            pointCol[0].size = new Vector3(0.06f, 1.0f, 9.0f);
            pointCol[1].size = new Vector3(0.06f, 1.0f, 9.0f);
            pointCol[2].size = new Vector3(0.06f, 1.0f, 9.0f);
            pointCol[3].size = new Vector3(0.06f, 1.0f, 9.0f);
            
            pointCol[0].center = new Vector3(0.0f, 0.15f, -4.75f);
            pointCol[1].center = new Vector3(0.0f, -0.15f, -4.75f);
            pointCol[2].center = new Vector3(0.0f, 0.15f, -4.75f);
            pointCol[3].center = new Vector3(0.0f, -0.15f, -4.75f);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        for (int a = 0; a < 4; a++)
        {
            //選ばれたしろを光らせる
            if (a == direction && inRange)
            {
                pointObj[a].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = selectMat;
            }
            else
            {
                pointObj[a].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = unselectMat;
            }
        }
        
        
        if (beingPulled)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayer>();

            playerScript.GiveScript(this, transform.parent.eulerAngles.z);

            for (int a = 0; a < 4; a++)
            {
                //選ばれたしろを光らせる
                if (a == direction)
                {
                    pointObj[a].SetActive(true);
                }
                else
                {
                    pointObj[a].SetActive(false);
                }
                
            }

            if (startPulling)
            {
                for (int a = 0; a < 4; a++)
                {
                    pointObj[a].SetActive(false);
                }
            
                if (fastPull)
                {
                    if (angleInfo <= maxAngle)
                    {
                        jointObj[jointNum].obj.transform.Rotate(0.0f, 0.0f, pullSpeed * 2.0f, Space.Self);
                        
                        if (jointNum < 4)
                        {
                            jointObj[jointNum + 2].obj.transform.Rotate(0.0f, 0.0f, -pullSpeed, Space.Self);
                        }
                        
                        if (jointNum < 3)
                        {
                            jointObj[jointNum + 3].obj.transform.Rotate(0.0f, 0.0f, -pullSpeed, Space.Self);
                        }

                        angleInfo += (pullSpeed * 2.0f);
                    }
                    else if (jointNum >= 1  && jointNum <= 5)
                    {
                        jointNum--;
                        
                        angleInfo = 0.0f;
                    }
                    else
                    {
                        FinishedPulling();
                        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayTearSE();
                    }
                }
            }
            
            if (returnForm || !playerScript.IsPulling() && !fastPull)
            {
                ResetForm();
            }
        }
        else
        {
            /* pointObj[0].SetActive(upLeftPoint);
            pointObj[1].SetActive(downLeftPoint);
            pointObj[2].SetActive(upRightPoint);
            pointObj[3].SetActive(downRightPoint); */
            
            this.transform.localScale = startingScale;
        }
    }
    
    public void SlowPull(float inputValue)
    {
        if ((inputValue > 0.0f && (direction == 0 || direction == 1)) || (inputValue < 0.0f && (direction == 2 || direction == 3)))
        {
            if (direction == 2 || direction == 3)
            {
                inputValue = Mathf.Abs(inputValue);
                Debug.Log(inputValue + "    " + angleInfo);
            }
            
            if (angleInfo <= maxAngle)
            {
                jointObj[jointNum].obj.transform.Rotate(0.0f, 0.0f, pullSpeed * (inputValue * 2.0f), Space.Self);
                
                if (jointNum < 4)
                {
                    jointObj[jointNum + 2].obj.transform.Rotate(0.0f, 0.0f, -pullSpeed * (inputValue * 1.0f), Space.Self);
                }
                
                if (jointNum < 3)
                {
                    jointObj[jointNum + 3].obj.transform.Rotate(0.0f, 0.0f, -pullSpeed * (inputValue * 1.0f), Space.Self);
                }

                angleInfo += (pullSpeed * (inputValue * 2.0f));
                
                if (jointNum == 0)
                {
                    nextPullSpeed = pullSpeed * inputValue;
                }
            }
            else if (jointNum > 0  && jointNum <= 5)
            {
                jointNum--;
                angleInfo = jointObj[jointNum].startingAngle.z;
                
                if (jointNum % 2 == 1)
                {
                    GameObject tapePart = Instantiate(GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>().GetTapeEffect(), jointObj[jointNum].obj.transform.position, Quaternion.identity);
                    //tapePart.GetComponent<ParticleSystem>().Play();
                }
                
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayTearSE();
            }
            else
            {
                FinishedPulling();
            }
        }
    }
    
    private void FinishedPulling()
    {
        playerScript.FinishPulling();
        
        for (int a = 0; a < collidingTapes.Count; a++)
        {
            if (collidingTapes[a] != null)
            {
                if (collidingTapes[a].transform.parent.eulerAngles.z == this.transform.parent.eulerAngles.z)
                {
                    collidingTapes[a].GetComponent<TapeScript>().SetDirection(direction);
                    collidingTapes[a].GetComponent<TapeScript>().SetPull(true);
                    collidingTapes[a].GetComponent<TapeScript>().SetSpeed(true, pullSpeed);
                }
                else
                {
                    if (fastPull)
                    {
                        if (collidingTapes[a].transform.parent.eulerAngles.z == 0)
                        {
                            float dA = tapeSize.z / Vector3.Distance(collidingTapes[a].transform.position, this.transform.position);
                            Vector3 p = this.transform.position + (collidingTapes[a].transform.position - this.transform.position) * dA; // eq: p`= p + direction * time
                            
                            var colTapeLength = collidingTapes[a].transform.GetChild(5).GetComponent<Renderer>().bounds.size.magnitude;
                            
                            var topCoordinate = collidingTapes[a].transform.position.x + (colTapeLength / 2.0f);
                            
                            var topTapeSize = topCoordinate - (p.x + (tapeSize.x / 2.0f));
                            var topTapeCenter = topCoordinate - (topTapeSize / 2.0f);
                            topTapeSize = topTapeSize / colTapeLength * collidingTapes[a].transform.localScale.magnitude;
                            
                            var botCoordinate = collidingTapes[a].transform.position.x - (colTapeLength / 2.0f);
                            
                            var botTapeSize = (colTapeLength - topTapeSize) - tapeSize.x;
                            var botTapeCenter = botCoordinate + (botTapeSize / 2.0f);
                            botTapeSize = botTapeSize / colTapeLength * collidingTapes[a].transform.localScale.magnitude;
                            
                            
                            GameObject objTemp = collidingTapes[a];
                            objTemp.transform.localScale = new Vector3(collidingTapes[a].transform.localScale.x, collidingTapes[a].transform.localScale.y, topTapeSize);
                            
                            GameObject obj1 = Instantiate(objTemp, new Vector3(topTapeCenter, collidingTapes[a].transform.position.y, collidingTapes[a].transform.position.z), Quaternion.identity);
                            obj1.transform.localRotation = Quaternion.Euler(0.0f, collidingTapes[a].transform.localRotation.eulerAngles.y, collidingTapes[a].transform.localRotation.eulerAngles.z);
                            
                            GameObject objTempTwo = collidingTapes[a];
                            objTempTwo.transform.localScale = new Vector3(collidingTapes[a].transform.localScale.x, collidingTapes[a].transform.localScale.y, botTapeSize);
                            
                            GameObject obj2 = Instantiate(objTempTwo, new Vector3(botTapeCenter, collidingTapes[a].transform.position.y, collidingTapes[a].transform.position.z), Quaternion.identity);
                            obj2.transform.localRotation = Quaternion.Euler(0.0f, collidingTapes[a].transform.localRotation.eulerAngles.y, collidingTapes[a].transform.localRotation.eulerAngles.z);
                            
                            GameObject objParent1 = new GameObject(obj1.name + "Parent");
                            objParent1.transform.position = obj1.transform.position;
                            obj1.transform.parent = objParent1.transform;
                            
                            GameObject objParent2 = new GameObject(obj2.name + "Parent");
                            objParent2.transform.position = obj2.transform.position;
                            obj2.transform.parent = objParent2.transform;
                            
                            Destroy(collidingTapes[a]);
                        }
                        else
                        {
                            float dA = tapeSize.z / Vector3.Distance(collidingTapes[a].transform.position, this.transform.position);
                            Vector3 p = this.transform.position + (collidingTapes[a].transform.position - this.transform.position) * dA; // eq: p`= p + direction * time
                            
                            var colTapeLength = collidingTapes[a].transform.GetChild(5).GetComponent<Renderer>().bounds.size.magnitude;
                            
                            var topCoordinate = collidingTapes[a].transform.position.y + (colTapeLength / 2.0f);
                            
                            var topTapeSize = topCoordinate - (p.y + (tapeSize.y / 2.0f));
                            var topTapeCenter = topCoordinate - (topTapeSize / 2.0f);
                            topTapeSize = topTapeSize / colTapeLength * collidingTapes[a].transform.localScale.magnitude;
                            
                            var botCoordinate = collidingTapes[a].transform.position.y - (colTapeLength / 2.0f);
                            
                            var botTapeSize = (colTapeLength - topTapeSize) - tapeSize.y;
                            var botTapeCenter = botCoordinate + (botTapeSize / 2.0f);
                            botTapeSize = botTapeSize / colTapeLength * collidingTapes[a].transform.localScale.magnitude;
                            
                            
                            GameObject objTemp = collidingTapes[a];
                            objTemp.transform.localScale = new Vector3(collidingTapes[a].transform.localScale.x, collidingTapes[a].transform.localScale.y, topTapeSize);
                            
                            GameObject obj1 = Instantiate(objTemp, new Vector3(collidingTapes[a].transform.position.x, topTapeCenter, collidingTapes[a].transform.position.z), Quaternion.identity);
                            obj1.transform.localRotation = Quaternion.Euler(90.0f, collidingTapes[a].transform.localRotation.eulerAngles.y, collidingTapes[a].transform.localRotation.eulerAngles.z);
                            
                            GameObject objTempTwo = collidingTapes[a];
                            objTempTwo.transform.localScale = new Vector3(collidingTapes[a].transform.localScale.x, collidingTapes[a].transform.localScale.y, botTapeSize);
                            
                            GameObject obj2 = Instantiate(objTempTwo, new Vector3(collidingTapes[a].transform.position.x, botTapeCenter, collidingTapes[a].transform.position.z), Quaternion.identity);
                            obj2.transform.localRotation = Quaternion.Euler(90.0f, collidingTapes[a].transform.localRotation.eulerAngles.y, collidingTapes[a].transform.localRotation.eulerAngles.z);
                            
                            GameObject objParent1 = new GameObject(obj1.name + "Parent");
                            objParent1.transform.position = obj1.transform.position;
                            obj1.transform.parent = objParent1.transform;
                            
                            GameObject objParent2 = new GameObject(obj2.name + "Parent");
                            objParent2.transform.position = obj2.transform.position;
                            obj2.transform.parent = objParent2.transform;
                            
                            Destroy(collidingTapes[a]);
                        }
                        
                    }
                    else
                    {
                        collidingTapes[a].GetComponent<TapeScript>().SetDirection(direction);
                        collidingTapes[a].GetComponent<TapeScript>().SetPull(true);
                        collidingTapes[a].GetComponent<TapeScript>().SetSpeed(true, pullSpeed);
                    }
                }
            }
        }
        
        if (blockScript != null)
        {
            blockScript.SetNotHolding();
            blockScript.gameObject.transform.parent = null;
        }
        
        beingPulled = false;
        startPulling = false;

        if(douzouScript != null)
        {
            douzouScript.DouzouDest();
        }

        if(spyderScript != null)
        {
            spyderScript.SpyderDest();
        }
        if (knightScript != null)
        {
            Debug.Log("ここに入った");
            knightScript.KnightRotate();
        }

        Destroy(this.gameObject);
    }
    
    private void ResetForm()
    {
        if (angleInfo > targetAngle)
        {
            jointObj[jointNum].obj.transform.Rotate(0.0f, 0.0f, -resetSpeed, Space.Self);
            angleInfo -= resetSpeed;
        }
        else if (jointNum < 5)
        {
            jointObj[jointNum].obj.transform.localRotation = jointObj[jointNum].startingAngle;
            
            jointNum++;
            
            targetAngle = jointObj[jointNum].startingAngle.z;
            angleInfo = jointObj[jointNum].obj.transform.localRotation.eulerAngles.z;
        }
        else
        {
            jointObj[jointNum].obj.transform.localRotation = jointObj[jointNum].startingAngle;
            
            beingPulled = false;
            startPulling = false;
            
            returnForm = false;
        
            for (int a = 0; a < 4; a++)
            {
                pointObj[a].SetActive(true);
            }
            
            if (direction == 2 || direction == 3)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z * -1.0f);
            }
            
            playerScript.FinishPulling();
            canPull = true;
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Tape")
        {
            collidingTapes.Add(other.gameObject);
        }
        
        if (other.gameObject.tag == "TapeBlock")
        {
            if (other.gameObject.transform.position.z > this.gameObject.transform.position.z)
            {
                blockScript = other.gameObject.GetComponent<TapeBlock>();

            }
        }

        if (other.gameObject.tag == "douzou")
        {
            if(DouzouGetflag == false)
            {
            DouzouGetflag = true;
            douzouScript = other.gameObject.GetComponent<DouzouScript>();
            }
        }

        if(other.gameObject.tag == "spyder")
        {
            SpyderGetflag = true;
            spyderScript = other.gameObject.GetComponent<SpyderScript>();
        }

        if (other.gameObject.tag == "soad")
        {
            KnightGetflag = true;
            knightScript = other.gameObject.GetComponent<KnightScript>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var tapeFront = false;
        
        if (other.gameObject.tag == "Tape" && other.gameObject.transform.root.position.z < transform.position.z)
        {
            tapeFront = true;
        }
        
        if (other.gameObject.tag == "Player" && !tapeFront)
        {
            inRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
        }

        if(other.gameObject.tag == "douzou")
        {
            DouzouGetflag = false;
        }

        if (other.gameObject.tag == "spyder")
        {
            SpyderGetflag = false;
        }

        if (other.gameObject.tag == "soad")
        {
            KnightGetflag = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    //インプットオブジェクト
    private InputManager inputManager;
    
    private AudioSource audioSource;
    private AudioManager audioManager;

    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpPower = 2.0f;
    
    [Header("デバッグ用 (これを外したら、プレイヤーは高さの2倍を飛ぶ)")]
    [SerializeField] bool jumpDebug = false;
    
    [Header("剥がす速度")]
    [SerializeField] float tearSpeed = 2.0f;
    
    [Header("剥がすまでの時間")]
    [SerializeField] float holdTime = 2.5f;
    
    [Header("ノックバック力")]
    [SerializeField] float knockBackForce = 3.0f;
    
    [Header("無敵期間")]
    [SerializeField] float invincibleDuration = 1.0f;
    
    [Header("点滅の間")]
    [SerializeField] float invincibleCycleTime = 0.1f;
    
    private bool onGround = false;              //地面についてるかどうか
    private bool grounded = false;
    private bool finishTearFlg = true;

    Rigidbody rb;   //リギッドボディー
    Collider col;   //コライダー
    
    private int direction = 0;
    private int tapeDirection = 0;
    
    private bool isPulling = false;
    private bool fastPull = false;
    private bool inRange = false;
    private bool inFreeze = false;
    private bool inPullingProcess = false;
    
    private int collectedStone = 0;
    private int stoneNumInMap;
    private bool reachedGoal = false;
    private bool firstPartGoal = true;
    
    private bool invincible = false;
    private bool canRotate = true;

    Animator playerAnimation;

    TapeScript tapeHold;
    SliderScript slideScript;
    
    GameObject[] stoneObj;
    
    MoveCamera cameraScript;
    
    EffectManager effectManager;
    
    public int GetStoneNum()
    {
        return collectedStone;
    }
    
    public void AddStone(GameObject stoneOb)
    {
        collectedStone++;
        
        for (int a = 0; a < 10; a++)
        {
            if (stoneObj[a] == null)
            {
                stoneObj[a] = stoneOb;
                Debug.Log(a + " ");
                break;
            }
        }
    }
    
    public void ReachedGoal()
    {
        reachedGoal = true;
        
        rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
    }
    
    public void GiveScript(TapeScript tapeScript, float rotation)
    {
        if (rotation == 0.0f)
        {
            tapeDirection = 0;
        }
        else if (rotation == 90.0f)
        {
            tapeDirection = 1;
        }
        else if (rotation == -90.0f)
        {
            tapeDirection = -1;
        }
    }
    
    public bool IsPulling()
    {
        return isPulling;
    }
    
    public void FinishPulling()
    {
        inFreeze = true;
        
        StartCoroutine("FreezePlayer");
    }

    private void Awake()
    {
        if (jumpDebug)
        {
            ;
        }
        else
        {
            jumpPower = this.GetComponent<Renderer>().bounds.size.y * 7.3125f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //コンポネントを取得
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        audioSource = this.GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource.clip = audioManager.GetMoveSE();
        audioSource.volume = audioManager.GetMoveVolume();
        audioSource.loop = true;
        
        playerAnimation = GetComponent<Animator>();
        
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();

        slideScript = GameObject.FindGameObjectWithTag("Slider").GetComponent<SliderScript>();
        
        stoneNumInMap = GameObject.FindGameObjectsWithTag("Stone").Length;
        stoneObj = new GameObject[10];
        
        cameraScript = FindObjectOfType<Camera>().GetComponent<MoveCamera>();
        
        effectManager = GameObject.FindGameObjectWithTag("EffectManager").GetComponent<EffectManager>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!reachedGoal)
        {
            if (!invincible)
            {
                Move();     //動く関数
            }
            
            if (rb.velocity.y >= 0)
            {
                rb.AddForce(Physics.gravity * 0.3f * rb.mass);
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(Physics.gravity * 2.2f * rb.mass);
            }
            
            RaycastHit hit;
            
            int layerMask = 3;
            layerMask = ~layerMask;
            
            int layerMask2 = 7;
            layerMask2 = ~layerMask2;
            
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1.0f, layerMask) || (Physics.Raycast(this.transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 1.0f, layerMask2)))
            {
                if (hit.distance < 0.5f)
                {
                    grounded = true;
                }
                else
                {
                    grounded = false;
                }
            }
            else
            {
                grounded = false;
            }
        }
        else
        {
            if (firstPartGoal)
            {
                Vector3 to = new Vector3(0, 180, 0);
                
                if (transform.localRotation.eulerAngles.y !=  to.y)
                {
                    transform.eulerAngles = Vector3.RotateTowards(this.transform.eulerAngles, to, 25.0f * Time.deltaTime, 2.0f);
                }
                else
                {
                    transform.eulerAngles = to;
                    
                    if (canRotate && cameraScript.InPosition())
                    {
                        StartCoroutine("GoalAnimation");
                        canRotate = false;
                    }
                }
                
                playerAnimation.SetBool("isMove", false);
                playerAnimation.SetBool("isJump", false);
                playerAnimation.SetBool("isGrab", false);
                
                audioSource.Stop();
            }
            else
            {
                Vector3 to = new Vector3(0, 90, 0);
                
                if (transform.localRotation.eulerAngles.y !=  to.y)
                {
                    transform.eulerAngles = Vector3.RotateTowards(this.transform.eulerAngles, to, 25.0f * Time.deltaTime, 2.0f);
                }
                else
                {
                    transform.eulerAngles = to;
                    
                    rb.velocity = new Vector3(speed, rb.velocity.y, 0.0f);
                    playerAnimation.SetBool("isComplete", false);
                    playerAnimation.SetBool("isClear", false);
                    playerAnimation.SetBool("isClear2", false);
                    playerAnimation.SetBool("isMove", true);
                }
            }
        }
    }
    
    void Move()
    {
        if (!isPulling)
        {
            playerAnimation.SetBool("FastPullRight", false);
            playerAnimation.SetBool("FastPullLeft", false);
            playerAnimation.SetBool("SlowPullRight", false);
            playerAnimation.SetInteger("SlowPullLeft", 0);
            playerAnimation.SetBool("FastPullUp", false);
            playerAnimation.SetBool("SlowPullUp", false);
            playerAnimation.SetBool("SlowPullDown", false);
            playerAnimation.SetBool("FastPullDown", false);
            
            playerAnimation.SetBool("isGrab", isPulling);

            if (finishTearFlg)
            {
                float movement = inputManager.GetMoveFloat();
                
                if (movement == 0.0f)
                {
                    cameraScript.nowidle();
                }
                else if (movement != 0.0f)
                {
                    cameraScript.nowmove();
                }
                
                RaycastHit hit;
                
                if (Physics.Raycast(this.transform.position + new Vector3(0.3f * movement, 1.0f, 0.0f), new Vector3(movement, 1.0f, 0.0f), out hit, 0.3f))
                {
                    if (hit.transform.gameObject.tag == "Ground" || hit.transform.gameObject.layer == 3)
                    {
                        movement = 0.0f;
                    }
                }
                
                rb.velocity = new Vector3(movement * speed, rb.velocity.y, 0.0f);
                
                bool playerMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
                playerAnimation.SetBool("isMove", playerMoving);
                
                if (playerMoving && onGround && !reachedGoal)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else
                {
                    audioSource.Stop();
                }

                if (Mathf.Sign(rb.velocity.x) <= 0.0f)
                {
                    if (transform.eulerAngles.y <= 250.0f || transform.eulerAngles.y >= 290.0f)
                    {
                        transform.Rotate(0.0f, movement * 250.0f * Time.deltaTime * 4.0f, 0.0f);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                    }
                }
                else
                {
                    if (transform.eulerAngles.y <= 70.0f || transform.eulerAngles.y >= 110.0f)
                    {
                        transform.Rotate(0.0f, movement * 250.0f * Time.deltaTime * 4.0f, 0.0f);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    }
                }
            }
            
            inPullingProcess = false;
        }
        else
        {
            Vector2 pullVec = inputManager.GetTearFloat();
            
            Vector3 to = new Vector3(0, 0, 0);
            
            var dir = 1.0f;
            
            if (transform.eulerAngles.y < 180.0f)
            {
                dir = -1.0f;
            }
            
            if (transform.eulerAngles.y !=  to.y)
            {
                transform.Rotate(new Vector3(0.0f, 100.0f * Time.deltaTime * dir, 0.0f), Space.Self);
            }
            else
            {
                transform.eulerAngles = to;
            }
            
            var pullPower = 0.0f;
            
            if (pullVec.x != 0.0f && tapeDirection == 0)
            {
                pullPower = pullVec.x;
            }
            else if (pullVec.y != 0.0f)
            {
                if (tapeDirection == 1)
                {
                    pullPower = pullVec.y;
                }
                else if (tapeDirection == -1)
                {
                    pullPower = pullVec.y * -1.0f;
                }
            }
            
            
            if (pullPower != 0.0f && !inFreeze)
            {   
                if (fastPull)
                {
                    tapeHold.SetSpeed(true, tearSpeed * 5.0f);
                    
                    if (direction == 0 || direction == 1)
                    {
                        switch (tapeDirection)
                        {
                        case 0:
                            playerAnimation.SetBool("FastPullRight", true);
                            break;
                        case 1:
                            playerAnimation.SetBool("FastPullUp", true);    
                            break;
                        case -1:
                            playerAnimation.SetBool("FastPullDown", true);
                            break;
                        }
                    }
                    else
                    {
                        switch (tapeDirection)
                        {
                        case 0:
                            playerAnimation.SetBool("FastPullRight", true);
                            break;
                        case 1:
                            playerAnimation.SetBool("FastPullDown", true);
                            break;
                        case -1:
                            playerAnimation.SetBool("FastPullUp", true);
                            break;
                        }
                    }
                    
                    inPullingProcess = true;
                }
                else
                {
                    StopCoroutine("HoldPull");
                    tapeHold.SetSpeed(false, tearSpeed);
                    tapeHold.SlowPull(pullPower);
                    slideScript.StopInc();
                    
                    if (direction == 0 || direction == 1)
                    {
                        switch (tapeDirection)
                        {
                        case 0:
                            playerAnimation.SetBool("SlowPullRight", true);
                            break;
                        case 1:
                            playerAnimation.SetBool("SlowPullUp", true);
                            break;
                        case -1:
                            playerAnimation.SetBool("SlowPullDown", true);
                            break;
                        }
                    }
                    else
                    {
                        switch (tapeDirection)
                        {
                        case 0:
                            playerAnimation.SetInteger("SlowPullLeft", 1);
                            break;
                        case 1:
                            playerAnimation.SetBool("SlowPullDown", true);
                            break;
                        case -1:
                            playerAnimation.SetBool("SlowPullUp", true);    
                            break;
                        }
                    }
                    
                    playerAnimation.SetBool("isGrab", false);
                    playerAnimation.speed = Mathf.Abs(pullPower);
                }
                
                finishTearFlg = false;
            }
            
            playerAnimation.SetBool("isMove", false);
            playerAnimation.SetBool("isJump", false);
        }
        
        slideScript.PressCondition(isPulling, holdTime);
        playerAnimation.SetBool("isGrab", isPulling);
    }
    
    public void Jump()
    {
        if (!onGround || isPulling || !grounded || reachedGoal || invincible){
            return;
        }
        
        playerAnimation.SetBool("isJump", true);
    }
    
    public void StartJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, 0.0f);
        rb.AddForce(0.0f, jumpPower, 0.0f, ForceMode.Impulse);
        audioManager.PlayJumpSE();
    }
    
    public void Pull() 
    {
        if (inRange && !isPulling && tapeHold != null && tapeHold.CanPull() && onGround && !reachedGoal && !invincible)
        {
            tapeHold.SetDirection(direction);
            tapeHold.SetPull(true);
            isPulling = true;
            fastPull = false;
            StartCoroutine("HoldPull");
        }
        else
        {
            StopCoroutine("HoldPull");
        }
    }
    
    public void StopPull()
    {
        if (isPulling && !inFreeze && !inPullingProcess)
        {
            StopCoroutine("HoldPull");
            tapeHold.StopPulling();
            tapeHold.SetDirection(direction);
            isPulling = false;
            fastPull = false;
        }
    }
    
    IEnumerator HoldPull()
    {
        yield return new WaitForSeconds(holdTime);
        
        fastPull = true;
    }
    
    IEnumerator FreezePlayer()
    {
        yield return new WaitForSeconds(0.7f);
        
        inFreeze = false;
        finishTearFlg = true;
        isPulling = false;
        fastPull = false;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "TapeBlock")
        {
            SetJump();
        }

        if (other.gameObject.tag == "MoveFloor")
        {
            SetJump();
            transform.SetParent(other.transform);
        }
        
        if ((other.gameObject.tag == "Enemy"|| other.gameObject.tag == "douzou"|| other.gameObject.tag == "spyder"  ) && !invincible)     //ここに敵のタグ入れる
        {
            if (collectedStone > 0)
            {
                if (stoneObj[collectedStone - 1] != null)
                {
                    stoneObj[collectedStone - 1].GetComponent<StoneScript>().LoseStone();
                    collectedStone--;
                }
            }
            
            audioManager.PlayEnemyHitSE();
            
            invincible = true;
            
            int children = transform.childCount;
            childObjects = new GameObject[children];
            
            for (int i = 0; i < children; ++i)
            {
                childObjects[i] =  transform.GetChild(i).gameObject;
            }
            
            Physics.IgnoreLayerCollision(0, 8, true);
            
            StartCoroutine("InvincibleTime");
            InvokeRepeating("InvincibleFlicker", 0.0f, invincibleCycleTime);
            
            Vector3 force = new Vector3(knockBackForce * 2.0f, knockBackForce * 1.5f, 0.0f);
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
    
    GameObject[] childObjects;
    
    IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(invincibleDuration);
        
        invincible = false;
        CancelInvoke();
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            childObjects[i].SetActive(true);
        }
        
        Physics.IgnoreLayerCollision(0, 8, false);
    }
    
    private void InvincibleFlicker()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            childObjects[i].SetActive(!childObjects[i].activeSelf);
        }
    }
    
    private void OnCollisionStay(Collision other) 
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "TapeBlock")
        {
            SetJump();
        }
    }
    
    private void SetJump()
    {
        playerAnimation.SetBool("isJump", false);
        onGround = true;
    }
    
    private void OnCollisionExit(Collision other) 
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "TapeBlock"){
            onGround = false;
        }
        
        if (other.gameObject.tag == "MoveFloor")
        {
            transform.SetParent(null);
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Tape")
        {
            Debug.Log("enter");
            tapeHold = other.gameObject.GetComponent<TapeScript>();
        }
        
    }
    
    private void OnTriggerStay(Collider other) 
    {
        switch (other.gameObject.tag)
        {
            case "UpPoint":
                direction = 0;
                break;
            case "DownPoint":
                direction = 1;
                break;
            case "LeftPoint":
                direction = 2;
                break;
            case "RightPoint":
                direction = 3;
                break;
        }
            
            switch (other.gameObject.tag)
            {
                case "UpPoint":
                case "DownPoint":
                case "LeftPoint":
                case "RightPoint":
                    if (tapeHold != null)
                    {
                        if (!tapeHold || tapeHold.gameObject.transform.root.position.z >= other.gameObject.transform.root.transform.position.z)
                        {
                            tapeHold.SetDirection(direction);
                            inRange = true;
                        }
                    }
                break;
            }
            
            
    }
    
    private void OnTriggerExit(Collider other) 
    {
        switch (other.gameObject.tag)
        {
            case "Tape":
                Debug.Log("exit");
                inRange = false;
                StopPull();
                tapeHold = null;
            break;
        }
    }
    
    IEnumerator GoalAnimation()
    {
        audioSource.Stop();
        
        for (int a = 0; a < collectedStone; a++)
        {
            stoneObj[a].GetComponent<StoneScript>().StartResultAnim((float)a + 0.5f);  //計算式考えて
        }
        
        inputManager.SetGoalStatus();
        audioManager.PlayResultSE();
        
        yield return new WaitForSeconds(3.0f);
        
        if (collectedStone == stoneNumInMap)
        {
            playerAnimation.SetBool("isComplete", true);
            Instantiate(effectManager.GetGoalStoneEffect(), new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z + 0.5f), Quaternion.identity);
        }
        else if (collectedStone > 0)
        {
            playerAnimation.SetBool("isClear2", true);
        }
        else
        {
            playerAnimation.SetBool("isClear", true);
        }
        
        ClearInfoScript.instance.SaveStageState(collectedStone, true);
        
        ClearInfoScript.instance.NextStageNum();
        
        audioManager.SetGameOver();
    }
    
    public void CheckAnimState()
    {
        firstPartGoal = false;
        
        for (int a = 0; a < collectedStone; a++)
        {
            stoneObj[a].GetComponent<StoneScript>().ReturnToBag();  //計算式考えて
        }
    }
}
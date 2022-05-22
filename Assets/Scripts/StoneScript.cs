using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
    Rigidbody rb;
    GameObject playerObj;

    Vector3 followOffset;
    
    [Header("プレイヤーからの距離")]
    [SerializeField] float distance = 1.0f;
    
    [Header("GET判定大きさ")]
    [SerializeField] float colSize = 1.0f;
    
    [Header("ストーンの上下距離")]
    [SerializeField] float moveDistance = 1.0f;
    
    private float rotateSpeed = 30.0f;
    private float moveSpeed = 2.0f;
    
    float t = 0.75f;
    
    private float minimum = -0.1f;
    private float maximum = 0.1f;
    
    //private float timePassed = 0.0f;
    
    private bool collect = false;
    private float resultPos;
    private bool animDoneFlg = false;
    
    private bool collected = false;
    
    private GameObject effectManager;
    private GameObject shineObj;

    public void StartResultAnim(float order)
    {
        collect = true;
        resultPos = order;
    }
    
    public bool AnimationFinished()
    {
        return animDoneFlg;
    }
    
    public void LoseStone()
    {
        collected = false;
        transform.parent = null;
        
        rb.AddForce(new Vector3((Random.Range(-2.0f, 2.0f)) * 3.0f, 2.0f, 0.0f), ForceMode.Impulse);
        
        Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), playerObj.GetComponent<Collider>(), true);
        StartCoroutine("CollectWaitTime");
    }
    
    IEnumerator CollectWaitTime()
    {
        yield return new WaitForSeconds(1.0f);
        
        Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), playerObj.GetComponent<Collider>(), false);
        collected = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        effectManager = GameObject.FindGameObjectWithTag("EffectManager");
        
        this.GetComponent<BoxCollider>().size = new Vector3(colSize, colSize, 1.0f);
        
        followOffset = new Vector3(0.0f, 1.0f, -1.5f * distance);
        
        shineObj = Instantiate(effectManager.GetComponent<EffectManager>().GetStoneEffect(), this.transform.position, Quaternion.identity);
        shineObj.transform.parent = this.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!collect)
        {
            if (!collected)
            {
                MoveStone();
            }
            else
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, followOffset, 20.0f * Time.deltaTime);
            }
            
            shineObj.SetActive(!collected);
        }
        else
        {
            var targetPos = new Vector3(resultPos, 5.0f, 0.0f);
            
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, 2.0f * Time.deltaTime);
            
            if (this.transform.localPosition == targetPos)
            {
                animDoneFlg = true;
            }
        }
    }
    
    private void MoveStone()
    {
        transform.Rotate(0.0f, -rotateSpeed * Time.deltaTime, 0.0f, Space.Self);
        
        Vector3 vel = new Vector3(rb.velocity.x, Mathf.Lerp(minimum, maximum, t) * moveDistance, rb.velocity.z);
        rb.velocity = vel * moveSpeed * 0.4f;
        
        t += 0.5f * Time.deltaTime;
        
        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player" && !collected)
        {
            this.transform.parent = playerObj.transform;
            
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayGetStoneSE();
            
            collected = true;
            
            other.gameObject.GetComponent<MovePlayer>().AddStone(this.gameObject);
            
            this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            
            GameObject getEffect = Instantiate(effectManager.GetComponent<EffectManager>().GetAchieveStoneEffect(), this.transform.position, Quaternion.identity);
        }
    }
}
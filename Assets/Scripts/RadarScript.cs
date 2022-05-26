using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarScript : MonoBehaviour
{
    [Header("見える範囲サイズ")]
    [SerializeField] float size = 5.0f;
    
    [Header("レーダー画像")]
    [SerializeField] Sprite blipPic = null;
    
    private GameObject playerObj;
    private GameObject radarObj;
    private GameObject blipObj;
    private RectTransform blipPos;
    
    BoxCollider col;
    Vector3 dir;
    Vector2[] cameraBound;
    
    private float canvasWidth;
    private float canvasHeight;
    
    private int fadeDir = 1; // the direction to fade: in = -1 or out = 1
    
    private bool inRange = false;
    private bool startFade = false;
    private bool isVisible = false;
    
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        
        radarObj = new GameObject("Radar" + this.gameObject.name);
        radarObj.gameObject.tag = "RadarCheck";
        col = radarObj.AddComponent<BoxCollider>();
        col.size = new Vector3(size * 10.0f, size * 5.0f, 1.0f);
        col.isTrigger = true;
        
        blipObj = new GameObject("RadarBlip" + this.gameObject.name);
        blipObj.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
        blipObj.tag = "RadarObj";
        blipObj.AddComponent<Image>().sprite = blipPic;
        blipObj.transform.localScale = new Vector3(3.0f, 2.0f, 1.0f);
        blipPos = blipObj.GetComponent<RectTransform>();
        
        var tempColor = blipObj.GetComponent<Image>().color;
        tempColor.a = 0.0f;
        blipObj.GetComponent<Image>().color = tempColor;
        
        cameraBound = new Vector2[2];
        canvasWidth = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.width;
        canvasHeight = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var visibleCheck = isVisible;
        isVisible = CheckVisibility();
        
        if (isVisible != visibleCheck)
        {
            startFade = true;
        }
        
        radarObj.transform.position = playerObj.transform.position;
        
        var screenCenter = FindObjectOfType<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));
        
        screenCenter = new Vector3(screenCenter.x, screenCenter.y - 1.5f, screenCenter.z);
        
        if (inRange)
        {
            if (!isVisible)
            {
                fadeDir = -1;
            }
            else
            {
                fadeDir = 1;
            }
            
            dir = (this.transform.position - screenCenter).normalized; //方向の計算

            blipPos.anchoredPosition = new Vector3(0.0f + dir.x * 1500.0f, 0.0f + dir.y * 1500.0f, 0.0f); //動かせる

            var dirRot = screenCenter - this.transform.position;   //方向の計算（2回目）
            var angle = Mathf.Atan2(dirRot.y, dirRot.x) * Mathf.Rad2Deg;    //アングルを計算
            var rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);    //アングルを決める
            blipObj.transform.rotation = rotation; //ローテーションを決める
            
            if (blipPos.anchoredPosition.x >= canvasWidth / 2.0f - 100.0f)
            {
                blipPos.anchoredPosition = new Vector2(canvasWidth / 2.0f - 100.0f, blipPos.anchoredPosition.y);
            }
            if (blipPos.anchoredPosition.x <= (canvasWidth / 2.0f - 100.0f) * -1.0f)
            {
                blipPos.anchoredPosition = new Vector2((canvasWidth / 2.0f - 100.0f) * -1.0f, blipPos.anchoredPosition.y);
            }
            if (blipPos.anchoredPosition.y >= canvasHeight / 2.0f - 100.0f)
            {
                blipPos.anchoredPosition = new Vector2(blipPos.anchoredPosition.x, canvasHeight / 2.0f - 100.0f);
            }
            if (blipPos.anchoredPosition.y <= (canvasHeight / 2.0f - 100.0f) * -1.0f)
            {
                blipPos.anchoredPosition = new Vector2(blipPos.anchoredPosition.x, (canvasHeight / 2.0f - 100.0f) * -1.0f);
            }
        }
        else
        {
            fadeDir = 1;
        }
        
        if (startFade)
        {
            FadeInOut();
        }
    }
    
    bool CheckVisibility()
    {
        var screenPos = FindObjectOfType<Camera>().WorldToScreenPoint(this.transform.position);
        var onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
        
        return onScreen && this.gameObject.GetComponent<MeshRenderer>().isVisible;
    }
    
    void FadeInOut()
    {
        Color currentColor = blipObj.GetComponent<Image>().color;
        
        if (fadeDir == 1)
        {
            if (currentColor.a > 0.0f) 
            {
                currentColor.a += ((float)fadeDir * -1.0f) * Time.deltaTime * 1.0f;
                if (currentColor.a <= 0.0f)
                {
                    startFade = false;
                    currentColor.a = 0.0f;
                }
            }
        }
        else
        {
            if (currentColor.a < 1.0f) 
            {
                currentColor.a += Mathf.Abs(fadeDir) * Time.deltaTime * 1.0f;
                if (currentColor.a >= 1.0f)
                {
                    startFade = false;
                    currentColor.a = 1.0f;
                }
            }
        }
        
        blipObj.GetComponent<Image>().color = currentColor;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "RadarCheck")
        {
            startFade = true;
            inRange = true;
        }
    }
    
    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "RadarCheck")
        {
            inRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "RadarCheck")
        {
            startFade = true;
            inRange = false;
        }
    }
}

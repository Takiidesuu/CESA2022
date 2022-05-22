using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    private InputManager inputManager;
    
    private int index = 1;
    
    private bool isPaused = false;
    private bool inputRest = false;
    
    GameObject[] menus;
    GameObject childPause;
    RectTransform arrow;
    private float arrowStartingHeight;
    
    public void SetPause()
    {
        isPaused = true;
    }
    
    private void Awake() 
    {
        isPaused = false;
    }
    
    private void Reset() 
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        childPause = this.transform.GetChild(0).gameObject;
        
        menus = new GameObject[4];
        
        for (int a = 0; a < 4; a++)
        {
            menus[a] = childPause.transform.GetChild(a).gameObject;
        }
        
        arrow = childPause.transform.GetChild(4).gameObject.GetComponent<RectTransform>();
        
        arrowStartingHeight = arrow.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            
            childPause.gameObject.SetActive(true);
            
            Vector2 upDownInput = inputManager.GetMenuMoveFloat();
            
            if (!inputRest)
            {
                if (upDownInput.y > 0.0f && index > 1)
                {
                    index--;
                    GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayCursorSE();
                }
                if (upDownInput.y < 0.0f && index < 4)
                {
                    index++;
                    GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayCursorSE();
                }
            }
            
            if (upDownInput == Vector2.zero)
            {
                inputRest = false;
            }
            else
            {
                inputRest = true;
            }
            
            arrow.localPosition = new Vector3(arrow.localPosition.x, arrowStartingHeight - (70.0f * ((float)index - 1.0f)), arrow.localPosition.z);
            
            if (inputManager.GetSelectState())
            {
                SelectOption();
            }
        }
        else
        {
            Time.timeScale = 1.0f;
            childPause.gameObject.SetActive(false);
            index = 1;
        }
    }
    
    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayPauseSE();
        }
        else
        {
            isPaused = false;
            
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayCancelSE();
            /* inputControls.Player.Enable();
            inputControls.UI.Disable(); */
        }
    }
    
    private void SelectOption()
    {
        switch (index)
        {   
            case 1:
                Debug.Log("1");
                break;
            case 2:
                
                break;
            case 3:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 4:
                Debug.Log("4");
                break;
        }
        
        if (isPaused)
        {
            Pause();
            inputManager.SwitchToPause();
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayDecideSE();
        }
    }
}

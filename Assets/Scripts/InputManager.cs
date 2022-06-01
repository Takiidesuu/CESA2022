using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private MainInputControls inputControls;
    private MovePlayer playerSc;
    private PauseScript pauseSc;
    
    //private bool jumping = false;
    private string sceneName;
    private bool gameScene = false;
    
    private float movement;
    private float CameraVec;
    private Vector2 pullVec;
    private Vector2 moveFloat;
    
    private Vector2 menuSelectFloat;
    
    private bool pauseSwitch = false;
    private bool canJump = true;
    
    private bool reachedGoal = false;
    
    public void SetGoalStatus()
    {
        reachedGoal = true;
    }
    
    private void Awake() 
    {
        inputControls = new MainInputControls();
    }
    
    private void OnEnable() 
    {
        inputControls.Player.Jump.performed += StartJump;
        inputControls.Player.Jump.Enable();
        
        inputControls.Player.Pull.performed += StartPull;
        inputControls.Player.Pull.canceled += StopPull;
        inputControls.Player.Pull.Enable();
        
        inputControls.Player.Pause.performed += Pause;
        inputControls.Player.Pause.Enable();
        
        inputControls.UI.Pause.performed += Pause;
        inputControls.UI.Pause.Enable();
        
        inputControls.UI.OK.performed += Select;
        inputControls.UI.OK.Enable();
        
        inputControls.UI.Back.performed += Cancel;
        inputControls.UI.Back.Enable();
        
        inputControls.Menu.Select.performed += MenuOK;
        inputControls.Menu.Select.Enable();
        
        inputControls.Menu.Cancel.performed += MenuCancel;
        inputControls.Menu.Cancel.Enable();
        
        inputControls.Enable();
    }
    
    private void OnDisable() 
    {
        inputControls.Disable();
    }
    
    public float GetMoveFloat()
    {
        return movement;
    }

    public float GetMoveCamera()
    {
        return CameraVec;
    }
    
    public Vector2 GetTearFloat()
    {
        return pullVec;
    }
    
    public Vector2 GetMenuMoveFloat()
    {
        return moveFloat;
    }
    
    public Vector2 GetMenuSelectFloat()
    {
        return menuSelectFloat;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName.Contains("Level") || sceneName.Contains("Stage"))
        {
            playerSc = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayer>();
            pauseSc = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseScript>();
        
            inputControls.Player.Enable();
            inputControls.Menu.Disable();
            gameScene = true;
            
            pauseSwitch = false;
        }
        else
        {
            inputControls.Menu.Enable();
            inputControls.Player.Disable();
            gameScene = false;
        }
        
        inputControls.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameScene)
        {
            movement = inputControls.Player.Move.ReadValue<float>();
            pullVec = inputControls.Player.Tear.ReadValue<Vector2>();
            moveFloat = inputControls.UI._4Direction.ReadValue<Vector2>();
            CameraVec = inputControls.Player.Camera.ReadValue<float>();
        }
        else
        {
            menuSelectFloat = inputControls.Menu._4Direction.ReadValue<Vector2>();
        }
    }
    
    private void LateUpdate() 
    {
        isSelect = false;
        CancelPressed = false;
        OKPressed = false;
    }
    
    private void Pause(InputAction.CallbackContext obj)
    {
        if (!reachedGoal)
        {
            pauseSc.Pause();
        
            SwitchToPause();
        }
    }
    
    public void SwitchToPause()
    {
        pauseSwitch = !pauseSwitch;
        
        if (pauseSwitch)
        {
            inputControls.Player.Disable();
            inputControls.UI.Enable();
        }
        else
        {
            inputControls.Player.Enable();
            inputControls.UI.Disable();
        }
    }
    
    private void StartJump(InputAction.CallbackContext obj)
    {
        playerSc.Jump();
    }
    
    private void StartPull(InputAction.CallbackContext obj)
    {
        playerSc.Pull();
    }
    
    private void StopPull(InputAction.CallbackContext obj)
    {
        playerSc.StopPull();
    }
    
    private bool isSelect = false;
    
    private void Select(InputAction.CallbackContext obj)
    {
        isSelect = true;
    }
    
    public bool GetSelectState()
    {
        return isSelect;
    }
    
    private void Cancel(InputAction.CallbackContext obj)
    {
        if (pauseSwitch)
        {
            pauseSwitch = !pauseSwitch;
            pauseSc.Pause();
            inputControls.Player.Enable();
            inputControls.UI.Disable();
        }
    }
    
    private bool OKPressed = false;
    private bool CancelPressed = false;
    
    private void MenuOK(InputAction.CallbackContext obj)
    {
        OKPressed = true;
    }
    
    public bool GetMenuOKState()
    {
        return OKPressed;
    }
    
    private void MenuCancel(InputAction.CallbackContext obj)
    {
        CancelPressed = true;
    }
    
    public bool GetMenuCancelState()
    {
        return CancelPressed;
    }
    
    private bool gameoverSwitch = false;

    public void SwitchToGameOver()
    {
        inputControls.Player.Disable();
        inputControls.UI.Enable();
        inputControls.UI.Pause.Disable();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Delegates;
using Venus.Utilities;


public class InputManager : Singleton<InputManager>
{

    #region property declarations
    public UiStateEnum UiState
    {
        get
        {
            return uiState;
        }
        set
        {
            uiState = value;
        }
    }

    public UiStateUpdateDelegate UpdateUI
    {
        get
        {
            return updateUI;
        }
        set
        {
            updateUI = value;
        }
    }

    public VoidDelegate Fire1
    {
        get
        {
            return fire1;
        }
        set
        {
            fire1 = value;
        }
    }

    public VoidDelegate Fire2
    {
        get
        {
            return fire2;
        }
        set
        {
            fire2 = value;
        }
    }

    public VoidDelegate Escape
    {
        get
        {
            return escape;
        }
        set
        {
            escape = value;
        }
    }

    public VoidDelegate Interact
    {
        get
        {
            return interact;
        }
        set
        {
            interact = value;
        }
    }

    public VoidDelegate FrameUpdate
    {
        get
        {
            return frameUpdate;
        }
        set
        {
            frameUpdate = value;
        }
    }

    public Vector3Delegate MouseLocation
    {
        get { return mouseLocation; }
        set { mouseLocation = value; }
    }

    public Vector3Delegate InputVector
    {
        get
        {
            return inputVector;
        }
        set
        {
            inputVector = value;
        }
    }

    public bool CanInteract
    {
        get
        {
            return canInteract;
        }
        set
        {
            canInteract = value;
        }
    }

    #endregion

    #region fields
    private UiStateEnum uiState;

    private UiStateUpdateDelegate updateUI;
    private VoidDelegate fire1;
    private VoidDelegate fire2;
    private VoidDelegate escape;
    private VoidDelegate interact;
    private VoidDelegate frameUpdate;
    private Vector3Delegate mouseLocation;
    private Vector3Delegate inputVector;

    private bool canInteract = true;
    public static bool gamePaused = false;
    [SerializeField]
    public GameObject PauseMenuUI;
    #endregion

    private void Start()
    {
        updateUI += ChangeUIState;
        inputVector += OnMovement;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<AudioManager>().Play("menu");
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetButtonDown("Inventory") && uiState != UiStateEnum.Trading)
        {
            FindObjectOfType<AudioManager>().Play("inventory");
            if (UiState == UiStateEnum.GamePlay)
            {
                updateUI(UiStateEnum.Inventory);
            }
            else
                updateUI(UiStateEnum.GamePlay);
        }

        if (UiState == UiStateEnum.GamePlay)
        {
            if (Input.GetButtonDown("Fire1") && fire1 != null)
            {
                fire1();
            }

            if (Input.GetButtonDown("Fire2") && fire2 != null)
            {
                fire2();
            }
        }

        if (Input.GetButtonDown("Cancel") && escape != null)
        {
            escape();
        }

        if (Input.GetButtonDown("Interact") && interact != null)
        {
            interact();
        }

        if (frameUpdate != null)
        {
            frameUpdate();
        }

       
    }

    bool walkAudioPlayed = false;

    /// <summary>
    /// Subscribed to input vector. Gets passed the horizontal and vertical axis values.
    /// </summary>
    /// <param name="vector">Vector of the input.</param>
    private void OnMovement(Vector3 vector)
    {
        if ((vector.x != 0 || vector.y != 0) && !walkAudioPlayed)
        {
            walkAudioPlayed = true;

            //play sound but remember this gets called every fixed update so make sure not to play any times thank you come again
        }
        else if (vector.x == 0 && vector.y == 0)
        {
            //Stop

            walkAudioPlayed = false;
        }
    }
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(0.2f);
        //yield on a new YieldInstruction that waits for 5 seconds.


        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;

    }
    void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    private void FixedUpdate()
    {
        #region  mousePosition

        Plane plane = new Plane(Vector3.up, PlayerManager.S_INSTANCE.player.transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitdist = 0.0f;

        if (plane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            mouseLocation(targetPoint);
        }
        #endregion   

        inputVector(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        

    }


    private void ChangeUIState(UiStateEnum uiStateEnum)
    {
        UiState = uiStateEnum;
    }
}

public enum UiStateEnum { GamePlay,Inventory,Trading,Dialog }

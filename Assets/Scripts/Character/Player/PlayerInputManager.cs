using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public PlayerManager player;
    PlayerControls playerControls;

    [Header("Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Camera Input")]
    [SerializeField] Vector2 cameraInput;    
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("Player Action Input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    public bool SprintInput => sprintInput;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnSceneChange;

        Instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {
            Instance.enabled = true;
        }
        else
        {
            Instance.enabled = false;
        }
    }
    
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

            // JAK TRZYMA SIĘ PRZYCISK TO BOOL SIĘ AKTYWUJE A JAK PUSZCZA SIĘ PRZYCISK TO BOOL SIĘ ZMIENIA NA FALSE
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }


    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleCameraMovementInput();
        HandlePlayerMovementInput();
        HandleDodgeInput();
    }

    // MOVEMEMNT
    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1f;
        }

        if (player == null)
            return;

        // PODAJEMY 0 DO FUNKCJI BO POSTAĆ NIE JEST ZABLOKOWANA NA WIDOKU NA PRZECIWNIKU
        bool isSprinting = SprintInput
            && moveAmount >= 0.5f
            && !player.isPerformingAction
            && player.playerStatsManager.currentStamina > 0;
        player.playerAnimatorManager.UpdateAnimatorVaMovementParameters(0, moveAmount, isSprinting);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    // ACTIONS
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            // FUNKCJA 
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

}

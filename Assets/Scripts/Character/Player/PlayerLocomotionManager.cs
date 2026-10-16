using System.Collections;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;
    

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float sprintingSpeed = 8.5f;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float sprintingStaminaCost = 2f;

    [Header("Dodge Settings")]
    private Vector3 rollDirection;
    [SerializeField] float rollSpeed = 7;
    [SerializeField] float rollDuration = 0.8f;
    [SerializeField] float dodgeStaminaCost = 2;
    // [SerializeField] float backStepSpeed = 2f;
    // [SerializeField] float backStepDuration = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
            return;

        GetVerticalAndHorizontalInputs();

        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float inputMoveAmount = PlayerInputManager.Instance.moveAmount;
        if (inputMoveAmount <= 0f)
            return;

        float movementSpeed;
        if (PlayerInputManager.Instance.SprintInput && inputMoveAmount >= 0.5f && !player.isPerformingAction && playerStatsManager.currentStamina > 0)
        {
            movementSpeed = sprintingSpeed;
            playerStatsManager.DrainStamina(sprintingStaminaCost * Time.deltaTime);
        }
        else if (inputMoveAmount > 0.5f)
        {
            movementSpeed = runningSpeed;
        }
        else
        {
            movementSpeed = walkingSpeed;
        }

        player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        if (!player.canRotate)
            return;

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
            return;
        
        if (playerStatsManager.currentStamina <= 0)
        {
            return;
        }
        // JAK SIĘ RUSZAMY TO ROLL, JAK STOIMY TO BACKSTEP
        if (PlayerInputManager.Instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.Instance.transform.forward * PlayerInputManager.Instance.verticalInput;
            rollDirection += PlayerCamera.Instance.transform.right * PlayerInputManager.Instance.horizontalInput;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, false);
            print("ROLL");
            StartCoroutine(PerformRoll(rollDirection));
        }
        else
        {
            // MOŻE KIEDYŚ ZNAJDĘ ANIMACJĘ DO BACKSTEP
            // player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, false);
            print("BACK STEP");
        }

        playerStatsManager.DrainStamina(dodgeStaminaCost);
    }

    #region NUMERATOR_POD_ROLLA
    // TEGO NIE TYKAĆ NA BOGA BO TO JEST DO ROOT MOTION ROLLA
    private IEnumerator PerformRoll(Vector3 direction)
    {
        float elapsedTime = 0f;
        float previousDistance = 0f;
        float totalDistance = rollSpeed * rollDuration;

        while (elapsedTime < rollDuration)
        {
            float deltaTime = Mathf.Min(Time.deltaTime, rollDuration - elapsedTime);
            elapsedTime += deltaTime;

            float progress = elapsedTime / rollDuration;
            float currentDistance = Mathf.SmoothStep(0f, totalDistance, progress);
            player.characterController.Move(direction * (currentDistance - previousDistance));
            previousDistance = currentDistance;

            yield return null;
        }
    }

    #endregion

    
}
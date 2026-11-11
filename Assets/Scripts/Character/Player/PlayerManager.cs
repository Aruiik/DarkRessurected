using System;
using UnityEngine;

public class PlayerManager : CharacterManager 
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    private PlayerUIHudManager playerUIHudManager;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    private void Start()
    {
        OnAsignCamera();
    }

    protected override void Update()
    {
        base.Update();
        // UPDATE DO ZAJMOWANIA SIĘ MOVEMENTEM
        playerLocomotionManager.HandleAllMovement();

        // UPDATE DO SPRAWDZANIA CZY MOŻNA REGENEROWAĆ STAMINĘ
        playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public void OnAsignCamera()
    {
        PlayerCamera.Instance.player = this;
        PlayerInputManager.Instance.player = this;
        playerUIHudManager = PlayerUIManager.Instance.playerUIHudManager;
        playerStatsManager.CurrentStaminaChanged -= playerUIHudManager.SetNewStaminaValue;
        playerStatsManager.CurrentStaminaChanged += playerUIHudManager.SetNewStaminaValue;
        playerStatsManager.CurrentHealthChanged -= playerUIHudManager.SetNewHealthValue;
        playerStatsManager.CurrentHealthChanged += playerUIHudManager.SetNewHealthValue;
        playerStatsManager.InitializeStats();
        playerUIHudManager.SetMaxStaminaValue(playerStatsManager.maxStamina);
        playerUIHudManager.SetMaxHealthValue(playerStatsManager.maxHealth);
    }
    

    private void OnDestroy()
    {
        if (playerUIHudManager != null)
            playerStatsManager.CurrentStaminaChanged -= playerUIHudManager.SetNewStaminaValue;
    }
    
}
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] CharacterManager character;

    [UnitHeaderInspectable("Stamina regeneration")]
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;
    [SerializeField] float staminaRegenerationAmmount = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }
    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        int stamina = 0;

        stamina = endurance * 10;

        return stamina;
    }

    public virtual void RegenerateStamina()
    {
        // NIE REGENERUJEMY STAMINY PODCZAS SPRINTU
        if (PlayerInputManager.Instance.SprintInput)
        {
            return;
        }

        if (character.isPerformingAction)
        {
            return;
        }

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (playerStatsManager.currentStamina < playerStatsManager.maxStamina)
            {
                staminaTickTimer += Time.deltaTime;
                
                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    playerStatsManager.SetCurrentStamina(playerStatsManager.currentStamina + Mathf.RoundToInt(staminaRegenerationAmmount));
                }
            }
        }
    }    

    public virtual void ResetStaminaRegenTimer(int previousStaminaAmmount, int currentStaminaAmmount)
    {
        if (currentStaminaAmmount < previousStaminaAmmount)
        {
            staminaRegenerationTimer = 0;
            staminaTickTimer = 0;
        }
    }
}

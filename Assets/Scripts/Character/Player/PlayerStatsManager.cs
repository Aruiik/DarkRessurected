using System;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    [Header("Player Stats")]
    public int endurance = 1;
    public int vigor = 1;
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }    
    public int currentStamina { get; private set; }
    public int maxStamina { get; private set; }
    
    public event Action<int, int> CurrentStaminaChanged;
    public event Action<int, int> CurrentHealthChanged;
    private float staminaChangeRemainder;
    private float healthChangeRemainder;

    public void InitializeStats()
    {
        maxStamina = CalculateStaminaBasedOnEnduranceLevel(endurance);
        maxHealth = CalculateHealthBasedOnVigorLevel(vigor);
        SetCurrentStamina(maxStamina);
        SetCurrentHealth(maxHealth);
    }

    public void SetCurrentStamina(int newValue)
    {
        newValue = Mathf.Clamp(newValue, 0, maxStamina);
        int oldValue = currentStamina;
        if (oldValue == newValue)
            return;

        currentStamina = newValue;
        if (currentStamina == 0)
            staminaChangeRemainder = 0f;

        CurrentStaminaChanged?.Invoke(oldValue, newValue);
        print(currentStamina);
        ResetStaminaRegenTimer(oldValue, newValue);
    }

    public void SetCurrentHealth(int newValue)
    {
        newValue = Mathf.Clamp(newValue, 0, maxHealth);
        int oldValue = currentHealth;
        if (oldValue == newValue)
            return;

        currentHealth = newValue;
        if (currentHealth == 0)
            healthChangeRemainder = 0f;

        CurrentHealthChanged?.Invoke(oldValue, newValue);
        print(currentHealth);
        ResetStaminaRegenTimer(oldValue, newValue);
    }

    public void DrainStamina(float amount)
    {
        if (amount <= 0f || currentStamina <= 0)
            return;

        staminaChangeRemainder += amount;
        int wholeStaminaToDrain = Mathf.FloorToInt(staminaChangeRemainder);
        if (wholeStaminaToDrain <= 0)
            return;

        staminaChangeRemainder -= wholeStaminaToDrain;
        SetCurrentStamina(currentStamina - wholeStaminaToDrain);
    }
}

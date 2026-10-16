using System;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    public int endurance = 1;
    public int currentStamina { get; private set; }
    public int maxStamina { get; private set; }
    public event Action<int, int> CurrentStaminaChanged;
    private float staminaChangeRemainder;

    public void InitializeStats()
    {
        maxStamina = CalculateStaminaBasedOnEnduranceLevel(endurance);
        SetCurrentStamina(maxStamina);
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

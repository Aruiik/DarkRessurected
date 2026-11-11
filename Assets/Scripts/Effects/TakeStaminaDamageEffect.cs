using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    [Header("Stamina Damage")]
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        if (character is PlayerManager player)
        {
            Debug.Log("Player is taking: " + staminaDamage + " stamina damage.");
            player.playerStatsManager.SetCurrentStamina(player.playerStatsManager.currentStamina - Mathf.FloorToInt(staminaDamage));
        }
    }
}

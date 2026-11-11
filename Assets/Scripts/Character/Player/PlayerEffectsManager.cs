using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            InstantCharacterEffect effect = Instantiate(effectToTest);
            ProcessInstantEffect(effect);
        }
    }
}

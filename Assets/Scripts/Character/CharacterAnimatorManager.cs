using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    public void UpdateAnimatorVaMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
    {
        float horizontalAmmount = horizontalValue;
        float verticalAmmount = verticalValue;

        if (isSprinting)
        {
            verticalAmmount = 2;
        }

        character.animator.SetFloat(horizontal, horizontalAmmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        character.animator.applyRootMotion =  applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        // TO DO UŻYCIA PRZY ANULOWANIU WYKONYWANIA ANIMACJI, NP. GDY DOSTANIEMY DMG
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
    }
}

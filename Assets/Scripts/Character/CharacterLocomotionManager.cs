using Unity.VisualScripting;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Ground & Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] float groundCheckSphereRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] protected Vector3 yVelocity;                   // SIŁA Z JAKĄ POSTAĆ BĘDZIE CIĄGNIĘTA W GÓRĘ LUB DÓŁ
    [SerializeField] protected float groundedYVelocity = -20;       // SIŁA Z JAKĄ POSTAĆ TRZYMA SIĘ ZIEMI
    protected float inAirTimer = 0;
    protected float grounderInAirTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            // TEN IF SIĘ WYKONUJE GDY NIE PRÓBUJEMY SKAKAĆ ALBO RUSZAĆ SIĘ DO GÓRY
            if (yVelocity.y <= 0)
            {
                inAirTimer = 0;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // TEN SIĘ WYKONUJE GDY NIE SKACZEMY I PRĘDKOŚĆ NIE ZOSTAŁA USTAWIONA
            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);
            if (character.isGrounded)
            {
                character.animator.SetFloat("InAirTimer", grounderInAirTimer);
            }
        }

        yVelocity.y += gravityForce * Time.deltaTime;
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    // RYSOWANIE GIZMOS DO SPRAWDZANIA SFERY Z ZIEMIĄ
    protected void OnDrawGizmos()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
}

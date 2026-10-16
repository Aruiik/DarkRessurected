using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CharacterManager : MonoBehaviour
{
    public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;



    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    protected virtual void Update()
    {
        
    }

    protected virtual void LateUpdate()
    {
        
    }



}

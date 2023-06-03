using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    private InputController inputController;
    private CharacterController characterController;
    private Animator animator;

    // for string redundancy
    private int isWalkingInt;
    private int isRunningInt;
    
    // hold player input
    private Vector2 currentInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private bool movingPressed;
    private bool runningPressed;
    private bool attackState = false;
    private bool playerDead = false;

    [SerializeField] private float rotationFactor = 1f;
    [SerializeField] private float runFactor = 3f;

    private int playerHP = 20;

    private void Awake() {
        inputController = new InputController();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingInt = Animator.StringToHash("isWalking");
        isRunningInt = Animator.StringToHash("isRunning");

        // callback functions that act as listeners
        inputController.CharacterControls.Move.started += OnInput;
        inputController.CharacterControls.Move.canceled += OnInput;
        inputController.CharacterControls.Move.performed += OnInput;

        inputController.CharacterControls.Run.started += OnRunInput;
        inputController.CharacterControls.Run.canceled += OnRunInput;

        inputController.CharacterControls.Punch.performed += context => Attack();
    }

    // context gives us the input data upon started
    private void OnInput(InputAction.CallbackContext context) {
        currentInput = context.ReadValue<Vector2>();
        currentMovement.x = currentInput.x;
        currentMovement.z = currentInput.y;

        currentRunMovement.x = currentInput.x * runFactor;
        currentRunMovement.z = currentInput.y * runFactor;

        movingPressed = currentInput.x != 0 || currentInput.y != 0; 
    }

    private void OnRunInput(InputAction.CallbackContext context) {
        runningPressed = context.ReadValueAsButton();
    }

    private void Attack() {
        // can't spam attack button
        if(!attackState) {
            animator.SetTrigger("isPunching");
        }
    }

    private void Update() {
        attackState = animator.GetCurrentAnimatorStateInfo(1).IsName("Punch");

        if(!attackState && !playerDead) {
            AnimatePlayer();
            RotatePlayer();
            ApplyGravity();

            if(runningPressed) {
                characterController.Move(currentRunMovement * Time.deltaTime);
            } else {
                characterController.Move(currentMovement * Time.deltaTime);
            }
        }
    }

    private void AnimatePlayer() {
        bool isWalking = animator.GetBool(isWalkingInt);
        bool isRunning = animator.GetBool(isRunningInt);

        if(movingPressed && !isWalking) {
            animator.SetBool(isWalkingInt, true);
        } else if(!movingPressed && isWalking) {
            animator.SetBool(isWalkingInt, false);
        }

        if((movingPressed && runningPressed) && !isRunning) {
            animator.SetBool(isRunningInt, true);
        } else if((!movingPressed || !runningPressed) && isRunning) {
            animator.SetBool(isRunningInt, false);
        }
    }

    private void RotatePlayer() {
        Vector3 lookingDirection;
        lookingDirection.x = currentMovement.x;
        lookingDirection.y = 0;
        lookingDirection.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(movingPressed) {
            Quaternion targetRotation = Quaternion.LookRotation(lookingDirection);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactor);
        }
    }

    private void ApplyGravity() {
        if(characterController.isGrounded) {
            float groundedGravity = -0.5f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        } else {
            float gravity = -9.8f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.CompareTag("Enemy Hand")) {
            playerHP--;
            GameManager.Instance.playerHealth--;

            if(playerHP == 0) {
                animator.SetBool("isDead", true);
                playerDead = true;
                Destroy(collision.gameObject);
                GameManager.Instance.LoseGame();
            }
        }
    }

    // in the new input system, every action map has to be enabled
    private void OnEnable() {
        inputController.CharacterControls.Enable();
    }

    private void OnDisable() {
        inputController.CharacterControls.Disable();
    }

}

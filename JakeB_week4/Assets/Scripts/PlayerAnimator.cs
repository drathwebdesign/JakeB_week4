using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_JUMPING = "IsJumping";

    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update() {
        animator.SetBool(IS_GROUNDED, playerMovement.IsGrounded());
        if (playerMovement.IsJumping()) {
            animator.SetTrigger(IS_JUMPING);
        }
    }
}
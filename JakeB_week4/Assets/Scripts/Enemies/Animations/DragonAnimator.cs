using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimator : MonoBehaviour {

    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_DIEING = "IsDieing";

    private bool hasDied = false;  // This flag prevents triggering the death animation multiple times

    private DragonAI dragonAI;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        dragonAI = GetComponent<DragonAI>();
    }

    private void Update() {

        if (dragonAI.IsDieing() && !hasDied) {
            // Trigger the die animation only once
            animator.SetTrigger(IS_DIEING);
            hasDied = true;
        } else if (dragonAI.IsAttacking()) {
            animator.SetTrigger(IS_ATTACKING);
        }
    }
}
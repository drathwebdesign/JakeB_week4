using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody rb;
    public float jumpForce;

    //groundCheck
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    //Gravity 
    [SerializeField] private float baseGravity = 5f;
    [SerializeField] private float maxFallSpeed = 10f;
    [SerializeField] private float fallSpeedMultiplier = 5f;

    //Double Jump
    private int doubleJumpsUsed = 0;
    private int maxDoubleJumps = 1;

    private bool isGrounded;
    //animation fields
    private bool isJumping;

    private void Awake() {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        rb = GetComponent<Rigidbody>();

        playerControls.Player.Jump.performed += ctx => Jump();
    }

        void Update()
    {
        GroundCheck();
        Gravity();
    }

    void Jump() {
        if (isGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            isGrounded = false;
        } else if (!isGrounded && doubleJumpsUsed < maxDoubleJumps) {
            DoubleJump();
        }
    }

    void GroundCheck() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        if (isGrounded) {
            isJumping = false;
            doubleJumpsUsed = 0; // Reset double jump count when grounded
        }
    }
    void Gravity() {
        if (rb.velocity.y < 0) { // Falling
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeedMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !isJumping) {
            rb.velocity += Vector3.up * Physics.gravity.y * (baseGravity - 1) * Time.deltaTime;
        }

        // Cap the fall speed
        if (rb.velocity.y < -maxFallSpeed) {
            rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed, rb.velocity.z);
        }
    }

    void DoubleJump() {
        if (!isGrounded && doubleJumpsUsed < maxDoubleJumps) {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset vertical velocity before double jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // Increase amount of jumps used
            doubleJumpsUsed++;
        }
    }

    public bool IsGrounded() {
        return isGrounded;
    }

    public bool IsJumping() {
        return isJumping;
    }
}
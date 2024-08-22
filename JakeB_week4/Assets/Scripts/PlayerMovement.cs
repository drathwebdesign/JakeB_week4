using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody rb;
    public float moveSpeed = 4f;
    public float jumpForce;
    private Vector2 inputVector;

    public float zRange = 3f;

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

    //health & invulnerability
    public int maxHealth = 5;
    private int currentHealth;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 2f;
    public Image healthBarImage;

    //attacking
    public Collider swordCollider;

    private bool isGrounded;
    //animation fields
    private bool isJumping;
    private bool isDoubleJumping;
    private bool isBlocking;
    private bool isAttacking;
    private bool isDieing;

    private void Awake() {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        playerControls.Player.Jump.performed += ctx => Jump();
        playerControls.Player.Block.performed += ctx => Block();
        playerControls.Player.Attack.performed += ctx => Attack();

        swordCollider.enabled = false;
    }

    void Start() {
        UpdateHealthUI();
    }

    void Update()
    {
        GroundCheck();
        Gravity();
        Bounds();
        HandleMovement();
    }

    void HandleMovement() {
        inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
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
            isDoubleJumping = false;
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
            isDoubleJumping = true;
            // Increase amount of jumps used
            doubleJumpsUsed++;
        }
    }

    void Bounds() {
        if (transform.position.x >= 15) {
            transform.position = new Vector3(15, transform.position.y, transform.position.z);
        }
        if (transform.position.x <= 0) {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        } else if (transform.position.z > zRange) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
        if (transform.position.z < -zRange) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }
    }

    void Block() {
        isBlocking = true;
    }

    void Attack() {
        isAttacking = true;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine() {
        swordCollider.enabled = true; // Enable the sword collider
        yield return new WaitForSeconds(0.5f); // Adjust duration as needed for the attack animation
        swordCollider.enabled = false; // Disable the sword collider after the attack
        isAttacking = false; // Reset attacking state
    }

    public void TakeDamage(int damage) {
        if (!isInvulnerable) {
            currentHealth -= damage;

            if (currentHealth <= 0) {
                Die();
            } else {
                StartCoroutine(InvulnerabilityCoroutine());
                UpdateHealthUI();
            }
        }
    }

    public void Heal(int amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        if (healthBarImage != null) {
            // Update the fill amount of the health bar
            healthBarImage.fillAmount = (float)currentHealth / maxHealth /2;
        }
    }

    private void Die() {
        Debug.Log("Player has died!");
        if (isDieing) return; // Stop Die from being called multiple times

        isDieing = true;
        playerControls.Player.Disable();
        GameManager.instance.PlayerDied();
    }

    private IEnumerator InvulnerabilityCoroutine() {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    //Animation Fields
    public bool IsGrounded() {
        return isGrounded;
    }
    public bool IsJumping() {
        return isJumping;
    }
    public bool IsDoubleJumping() {
        return isDoubleJumping;
    }
    public bool IsBlocking() {
        return isBlocking;
    }
    public bool IsAttacking() {
        return isAttacking;
    }
    public bool IsDieing() {
        return isDieing;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonAI : MonoBehaviour {

    public EnemyStats enemyStats;
    private int currentHealth;
    private Rigidbody rb;

    private SpawnManager spawnManager;
    private BackgroundScrollOffset backgroundScroll;

    //HP slider
    public Slider bossHealthSlider;
    private CanvasGroup sliderCanvasGroup;

    //Dragon Attacks

    // Attack Cooldown
    public float basicAttackRange = 10f;
    public float attackCooldown = 8f;
    private bool isAttackOnCooldown = false;
    public Collider[] clawAttackColliders; // Add a collider for the claw attack
    private Transform playerTransform; // To track the player
    public ParticleSystem flameAttackParticles;


    public GameObject[] itemPrefabs;

    //Animations
    private bool isDieing;
    private bool isAttacking;
    private bool isAttackingFlame;
    private bool isAttackingClaw;

    void Start() {
        currentHealth = enemyStats.maxHealth;
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the SpawnManager in the scene
        spawnManager = FindObjectOfType<SpawnManager>();

        // Find the BossHealthSlider in the scene
        bossHealthSlider = GameObject.Find("BossHealthSlider").GetComponent<Slider>();
        if (bossHealthSlider != null) {
            bossHealthSlider.maxValue = enemyStats.maxHealth; // Set the max value to Dragon's max health
            bossHealthSlider.value = currentHealth; // Set the initial value
            sliderCanvasGroup = bossHealthSlider.GetComponent<CanvasGroup>();

            // Show the slider initially by setting the alpha to 1
            if (sliderCanvasGroup != null) {
                sliderCanvasGroup.alpha = 1; // Fully visible
            }

            // Pause spawning as soon as the DragonAI is spawned
            if (spawnManager != null) {
                spawnManager.PauseSpawning();
            }
            // Disable the background scroll when the dragon spawns
            backgroundScroll = FindObjectOfType<BackgroundScrollOffset>();
            if (backgroundScroll != null) {
                backgroundScroll.DisableScroll();
            }
        }
    }


    void Update() {
        if (isDieing) return;
        // Update the health slider's value if it's active
        if (bossHealthSlider != null) {
            bossHealthSlider.value = currentHealth;
        }
        if (!isAttackOnCooldown) {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= basicAttackRange) {
                StartCoroutine(BasicAttack());
            } else {
                int attackChoice = Random.Range(0, 2); // 0: Flame, 1: Claw
                if (attackChoice == 0) {
                    StartCoroutine(FlameAttack());
                } else if (attackChoice == 1) {
                    StartCoroutine(ClawAttack());
                }
            }
        }
    }

    // DRAGON ATTACKS
    // Basic Attack
    public IEnumerator BasicAttack() {
        if (isDieing) yield break;
        isAttacking = true;
        isAttackOnCooldown = true;

        // Perform basic attack logic here
        Debug.Log("Dragon performs basic attack!");

        yield return new WaitForSeconds(1.2f); // Duration of the basic attack

        isAttacking = false;
    }

    //Flame Attack
    public IEnumerator FlameAttack() {
        if (isDieing || isAttackOnCooldown) yield break; // Do nothing if already dieing
        isAttackingFlame = true;
        isAttackOnCooldown = true;
        Debug.Log("Fire attack!");

        // Activate the flame attack particle system
        if (flameAttackParticles != null) {
            var emission = flameAttackParticles.emission;
            emission.rateOverTime = 250f; // Set to desired emission rate
            flameAttackParticles.Play();
        }

        yield return new WaitForSeconds(3f); // Duration

        // Deactivate the particle system after the attack
        if (flameAttackParticles != null) {
            var emission = flameAttackParticles.emission;
            emission.rateOverTime = 0f; // Stop emitting new particles
            flameAttackParticles.Stop();
        }
        isAttackingFlame = false;
        StartCooldown();
    }
    //Fire Damage method
    public void OnParticleCollision(GameObject other) {
        Debug.Log("Particle collided with: " + other.name);
        if (other.CompareTag("Player")) {
            Debug.Log("Applying damage to player");
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null) {
                playerMovement.TakeDamage(enemyStats.damage); // Apply damage to the player
            }
        }
    }

    // Claw Attack
    public IEnumerator ClawAttack() {
        if (isDieing || isAttackOnCooldown) yield break;

        isAttackingClaw = true;
        isAttackOnCooldown = true;
        Debug.Log("Claw attack!");

        // Activate all claw colliders
/*        foreach (Collider collider in clawAttackColliders) {
            if (collider != null) {
                collider.enabled = true;
            }
        }*/

        yield return new WaitForSeconds(3f); // Duration of the claw attack

        // Deactivate all claw colliders
/*        foreach (Collider collider in clawAttackColliders) {
            if (collider != null) {
                collider.enabled = false;
            }
        }*/

        isAttackingClaw = false;

        StartCooldown();
    }

    //Attack Cooldowns
    void StartCooldown() {
        isAttackOnCooldown = true;
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine() {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }

    //Generic Damage through Collider
    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            //isAttacking = true;
            {
                PlayerMovement playerMovement = collision.transform.GetComponent<PlayerMovement>();
                playerMovement.TakeDamage(enemyStats.damage); // Deal damage based on stats
            }
        }
    }

    // Method to take damage
    public void TakeDamage(int damage, Vector3 knockbackDirection, float knockbackForce) {
        if (!this.enabled || isDieing) return; // Do nothing if the AI script is disabled or the dragon is dying

        currentHealth -= damage;
        //knockback
        //rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        if (isDieing) return; // Stop Die from being called multiple times

        isDieing = true;

        DropItems();
        // Re-enable spawning of maps after DragonAI is destroyed
        if (spawnManager != null) {
            spawnManager.ResumeSpawning();
        }

        // Hide the health slider once the Dragon is defeated
        if (sliderCanvasGroup != null) {
            sliderCanvasGroup.alpha = 0; // Make the slider invisible again
        }

        // Re-enable the background scroll when the dragon dies
        if (backgroundScroll != null) {
            backgroundScroll.EnableScroll();
        }

        Destroy(gameObject, 5f);
    }

        private void DropItems() {
        float heightOffset = 1.0f; // Adjust this value as needed
        Vector3 spawnPosition = transform.position + new Vector3(0, heightOffset, 0);

        foreach (GameObject itemPrefab in itemPrefabs) {
            Instantiate(itemPrefab, spawnPosition, itemPrefab.transform.rotation);
        }
    }

    //Animation fields
    public bool IsDieing() {
        return isDieing;
    }
    public bool IsAttacking() {
        return isAttacking;
    }
    public bool IsAttackingFlame() {
        return isAttackingFlame;
    }
    public bool IsAttackingClaw() {
        return isAttackingClaw;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonAI : MonoBehaviour {

    public EnemyStats enemyStats;
    private int currentHealth;
    private Rigidbody rb;
    private SpawnManager spawnManager;
    //HP slider
    public Slider bossHealthSlider;
    private CanvasGroup sliderCanvasGroup;

    //Dragon Attacks

    // Attack Cooldown
    public float attackCooldown = 8f;
    private bool isAttackOnCooldown = false;

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
        // Find the SpawnManager in the scene
        spawnManager = FindObjectOfType<SpawnManager>();
        sliderCanvasGroup = bossHealthSlider.GetComponent<CanvasGroup>();

        // Find the DragonHealthSlider in the scene and initialize it
        bossHealthSlider = GameObject.Find("BossHealthSlider").GetComponent<Slider>();
        if (bossHealthSlider != null) {
            bossHealthSlider.maxValue = enemyStats.maxHealth; // Set the max value to Dragon's max health
            bossHealthSlider.value = currentHealth; // Set the initial value
            // Hide the slider initially by setting the alpha to 0
            if (sliderCanvasGroup != null) {
                sliderCanvasGroup.alpha = 1; // Fully transparent
            }
        }

        // Pause spawning as soon as the DragonAI is spawned
        if (spawnManager != null) {
            spawnManager.PauseSpawning();
        }
    }


    void Update() {
        if (isDieing) return;
        // Update the health slider's value if it's active
        if (bossHealthSlider != null) {
            bossHealthSlider.value = currentHealth;
        }
        // Here you can add logic to decide when to call different attacks
        // For now, we just call the flame attack manually (for example purposes)
        if (!isAttackOnCooldown) {
            StartCoroutine(FlameAttack());
        }
    }

    // DRAGON ATTACKS

    //Flame Attack
    public IEnumerator FlameAttack() {
        if (isDieing || isAttackOnCooldown) yield break; // Do nothing if already dieing

        // Activate the flame attack particle system
        if (flameAttackParticles != null) {
            var emission = flameAttackParticles.emission;
            emission.rateOverTime = 250f; // Set to desired emission rate
            flameAttackParticles.Play();
        }

        isAttackingFlame = true;

        yield return new WaitForSeconds(3f); // Duration of the flame attack

        // Deactivate the particle system after the attack
        if (flameAttackParticles != null) {
            var emission = flameAttackParticles.emission;
            emission.rateOverTime = 0f; // Stop emitting new particles
            flameAttackParticles.Stop();
        }
        // Set attack on cooldown
        isAttackOnCooldown = true;
        isAttackingFlame = false;

        // Wait for the cooldown period before allowing another attack
        yield return new WaitForSeconds(attackCooldown);

        isAttackOnCooldown = false;
    }

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

/*    // Claw Attack
    public IEnumerator ClawAttack() {
        if (isDieing || isAttackOnCooldown) yield break;

        isAttackingClaw = true;

        // Activate the claw collider to apply damage when the player is in range
        if (clawAttackCollider != null) {
            clawAttackCollider.enabled = true;
        }

        yield return new WaitForSeconds(1f); // Duration of the claw attack

        if (clawAttackCollider != null) {
            clawAttackCollider.enabled = false;
        }

        isAttackingClaw = false;
        StartCooldown();
    }

    // Basic Attack
    public IEnumerator BasicAttack() {
        if (isDieing || isAttackOnCooldown) yield break;

        isAttacking = true;

        // Basic attack logic here
        yield return new WaitForSeconds(1f); // Duration of the basic attack

        isAttacking = false;
        StartCooldown();
    }*/


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
        if (isDieing) return; // Do nothing if the is already Dieing

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
        //isAttacking = false;

        /*        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
                {
                    capsuleCollider.enabled = false;
                }*/

        DropItems();
        // Re-enable spawning of maps after DragonAI is destroyed
        if (spawnManager != null) {
            spawnManager.ResumeSpawning();
        }
        // Hide the health slider once the Dragon is defeated
        if (sliderCanvasGroup != null) {
            sliderCanvasGroup.alpha = 0; // Make the slider invisible again
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
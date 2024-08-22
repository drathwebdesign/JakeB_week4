using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public EnemyStats enemyStats;
    private int currentHealth;
    private Rigidbody rb;

    public GameObject[] itemPrefabs;

    //Animations
    private bool isDieing;
    private bool isAttacking;

    void Start()
    {
        currentHealth = enemyStats.maxHealth;
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (isDieing) return;
    }
    
    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            isAttacking = true; {
                PlayerMovement playerMovement = collision.transform.GetComponent<PlayerMovement>();
                playerMovement.TakeDamage(enemyStats.damage); // Deal damage based on stats
            }
            Debug.Log("Hit Player");
        }
    }

    // Method to take damage
    public void TakeDamage(int damage, Vector3 knockbackDirection, float knockbackForce) {
        if (isDieing) return; // Do nothing if the is already Dieing

        currentHealth -= damage;
        //knockback
        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        if (isDieing) return; // Stop Die from being called multiple times

        isDieing = true;
        isAttacking = false;

        DropItems();
        Destroy(gameObject, 1f);
    }

    private void DropItems()
    {
        foreach (GameObject itemPrefab in itemPrefabs)
        {
            Instantiate(itemPrefab, transform.position, itemPrefab.transform.rotation);
        }
    }

    //Animation fields
    public bool IsDieing() {
        return isDieing;
    }
    public bool IsAttacking() {
        return isAttacking;
    }
}
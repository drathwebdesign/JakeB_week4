using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRelay : MonoBehaviour {
    public DragonAI dragonAI;

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Weapon")) {
            // Assuming WeaponHandler is attached to the weapon
            WeaponHandler weaponHandler = collision.transform.GetComponent<WeaponHandler>();
            if (weaponHandler != null) {
                Vector3 knockbackDirection = transform.position - collision.transform.position;
                // Relay the damage to the main DragonAI script
                dragonAI.TakeDamage(weaponHandler.weaponStats.damage, knockbackDirection, weaponHandler.weaponStats.knockBack);
            }
        }
    }
}
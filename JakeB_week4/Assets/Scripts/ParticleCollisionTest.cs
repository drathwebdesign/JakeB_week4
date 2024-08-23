using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionTest : MonoBehaviour {
    void OnParticleCollision(GameObject other) {
        Debug.Log("Collision detected with: " + other.name);
    }
}
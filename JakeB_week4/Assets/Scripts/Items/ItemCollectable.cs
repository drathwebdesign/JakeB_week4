using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour {
    public Item item;
    private AudioSource audioSource;
    public AudioClip pickupSound;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            audioSource.enabled = true;
            audioSource.PlayOneShot(pickupSound);
            //audioSource.PlayOneShot(audioSource.clip);
            Destroy(gameObject, 0.25f);
        }
    }
}
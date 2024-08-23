using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEntry : MonoBehaviour {
    public float flightSpeed = 3f;
    public Transform landingPoint;
    public float landingDuration = 5f;

    //Animations
    private bool isFlying;
    private bool isLanding;

    private DragonAI dragonAI;

    void Start() {
        dragonAI = GetComponent<DragonAI>();
        if (dragonAI != null) {
            // Disable the AI script while flying
            dragonAI.enabled = false;
        }

        // Start the entrance sequence
        StartCoroutine(FlyAndLand());
    }

    private IEnumerator FlyAndLand() {
        // Fly towards the landing point
        isFlying = true;
        while (Vector3.Distance(transform.position, landingPoint.position) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, landingPoint.position, flightSpeed * Time.deltaTime);
            yield return null;
        }

        // Simulate landing (you can add more detailed animation here if necessary)
        isLanding = true;
        yield return new WaitForSeconds(landingDuration);
        isFlying = false;
        isLanding = false;
        // Re-enable the AI script after landing
        if (dragonAI != null) {
            dragonAI.enabled = true;
        }

        // Optionally, destroy this script if it's no longer needed
        Destroy(this);
    }

    //Animation fields
    public bool IsFlying() {
        return isFlying;
    }
    public bool IsLanding() {
        return isLanding;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEntry : MonoBehaviour {
    public float flightSpeed = 3f;
    public Transform landingPoint;
    public float landingDuration = 5f;
    private DragonAI dragonAI;

    //Animations
    private bool isFlying;
    private bool isLanding;

    void Start() {
        dragonAI = GetComponent<DragonAI>();

        // Start the entrance sequence
        if (landingPoint != null) {
            StartCoroutine(FlyAndLand());
        }
    }
    public void SetLandingPoint(Transform point) {
        landingPoint = point;
    }

    private IEnumerator FlyAndLand() {
        // Fly towards the landing point
        isFlying = true;
        while (Vector3.Distance(transform.position, landingPoint.position) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, landingPoint.position, flightSpeed * Time.deltaTime);
            yield return null;
        }

        //Landing
        isLanding = true;
        yield return new WaitForSeconds(landingDuration);
        isFlying = false;
        isLanding = false;
        // Re-enable the AI script after landing
        if (dragonAI != null) {
            dragonAI.enabled = true;
        }

        //destroy this script
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
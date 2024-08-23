using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float movementSpeed;
    private float destroyXPosition = -15f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.left *  movementSpeed * Time.deltaTime);
        if (transform.position.x < destroyXPosition) {
            Destroy(gameObject);
        }
    }
}
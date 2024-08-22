using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Vector3 startPosition;
    public float moveSpeed =2f;
    float bgWidth;

    void Start()
    {
        startPosition = transform.position;
        bgWidth = GetComponent<BoxCollider>().size.x;
    }


    void Update()
    {
        if(transform.position.x < startPosition.x -(bgWidth/2)){
            transform.position = startPosition;
        }

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}

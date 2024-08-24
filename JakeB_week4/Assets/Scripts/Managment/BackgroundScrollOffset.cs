using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollOffset : MonoBehaviour
{

    public float scrollSpeed =0.02f;
    private bool isScrolling = true;


    void Update()
    {
        if (isScrolling) {
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Time.time * scrollSpeed, 0);
        }
    }

    public void EnableScroll() {
        isScrolling = true;
    }

    public void DisableScroll() {
        isScrolling = false;
    }
}
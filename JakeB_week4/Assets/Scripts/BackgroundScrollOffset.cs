using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollOffset : MonoBehaviour
{

    public float scrollSpeed =0.02f;

    //public Renderer bgRenderer;

    void Start()
    {
        
    }


    void Update()
    {
        GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Time.time * scrollSpeed, 0);
        //bgRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
    }
}
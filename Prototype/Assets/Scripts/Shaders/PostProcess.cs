using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour
{
    public Material material;
    float timer;
    public float speed;
    // Start is called before the first frame update
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);

       
    }
    private void Update()
    {
        timer += Time.deltaTime;
        material.SetFloat("_Intensity", Mathf.SmoothStep(0,1, Mathf.PingPong(timer * speed, 1)));
    }

}

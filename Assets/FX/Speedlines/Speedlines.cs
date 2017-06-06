using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Speedlines : MonoBehaviour
{ 
    private Material material;

    void Awake()
    {
        material = new Material(Shader.Find("Hidden/FXSpeedlines"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
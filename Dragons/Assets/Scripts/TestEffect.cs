using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestEffect : MonoBehaviour
{
    public Material EffectMaterial;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, EffectMaterial);
    }

}

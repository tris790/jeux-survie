using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public List<Renderer> textureRenderers;

    public void DrawTexture(Texture2D texture)
    {
        foreach (var textureRenderer in textureRenderers)
        {
            Debug.Log(textureRenderer);
            textureRenderer.sharedMaterial.mainTexture = texture;
            textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }
    }
}

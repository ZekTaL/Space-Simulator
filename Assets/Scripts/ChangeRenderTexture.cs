using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages to change the depth buffer with a render texture
/// </summary>
public class ChangeRenderTexture : MonoBehaviour
{
    /// <summary>
    /// Raw Image for the render texture
    /// </summary>
    [SerializeField] RawImage raw;
    /// <summary>
    /// Camera that will display the render texture
    /// </summary>
    [SerializeField] Camera cam;

    private void Start() => ChangeDepthBuffer(PlayerPrefs.GetInt("DepthBuffer", 32));

    /// <summary>
    /// Function that will change the depth buffer of the render texture
    /// </summary>
    /// <param name="_bit">Choose between 16 or 32 bit depth buffer</param>
    public void ChangeDepthBuffer(int _bit)
    {
        if (cam.targetTexture != null) cam.targetTexture.Release();

        RenderTexture texture;

        switch (_bit)
        {
            case 16:
                texture = new RenderTexture(Screen.width, Screen.height, 16);
                break;

            case 32:
                texture = new RenderTexture(Screen.width, Screen.height, 32);
                break;

            default: return;
        }

        cam.targetTexture = texture;
        raw.texture = texture;
    }
}
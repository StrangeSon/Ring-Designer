using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RenderScaleController : MonoBehaviour
{
    public UniversalRenderPipelineAsset URPAsset;

    private float initialScale;

    private void Awake()
    {
        initialScale = URPAsset.renderScale;
        URPAsset.renderScale = 1f;
    }

    private void OnDestroy()
    {
        URPAsset.renderScale = initialScale;
    }


}

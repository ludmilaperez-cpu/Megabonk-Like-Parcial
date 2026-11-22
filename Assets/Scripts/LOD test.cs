using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CurvedWorldCullingFix : MonoBehaviour
{
    [Tooltip("Factor de expansión del frustum. Súbelo si siguen desapareciendo en los bordes.")]
    [SerializeField] private float frustumExpansion = 1.5f;

    private Camera cam;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
    {
        if (renderingCamera != cam) return;

        
        Matrix4x4 p = cam.projectionMatrix;
        p.m00 /= frustumExpansion;
        p.m11 /= frustumExpansion;
        cam.cullingMatrix = p * cam.worldToCameraMatrix;
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
    {
        if (renderingCamera != cam) return;

        
        cam.ResetCullingMatrix();
    }
}

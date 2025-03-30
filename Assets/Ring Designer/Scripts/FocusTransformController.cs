using UnityEngine;

public class FocusTransformController : MonoBehaviour
{
    public Transform FocusTransform;
    public LayerMask ProductLayer;
    public float FocusDuration = 0.1f;

    private Vector3 _velocity;

    void Update()
    {
        // Create ray from center of screen
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        Vector3 targetPosition = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, ProductLayer))
        {
            targetPosition = hit.point;
        }

        FocusTransform.position = Vector3.SmoothDamp(FocusTransform.position, targetPosition, ref _velocity, FocusDuration);
    }
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace RingDesigner
{
    public class MotionAdaptivePerformance : MonoBehaviour
    {
        public Camera Camera;

        [Header("Motion Thresholds")]
        [SerializeField] float highMotionSpeed = 2.5f;
        [SerializeField] float lowMotionSpeed = 0.15f;
        [SerializeField] float highAngularSpeed = 90f;
        [SerializeField] float lowAngularSpeed = 8f;

        [Header("Resolution")]
        [SerializeField] float minScale = 0.5f;
        [SerializeField] float maxScale = 1.0f;
        [SerializeField] float scaleLerpSpeed = 4f;

        [Header("Frame Rate")]
        [SerializeField] int minFPS = 20;
        [SerializeField] int maxFPS = 60;
        [SerializeField] float fpsLerpSpeed = 3f;

        Vector3 previousPosition;
        Quaternion previousRotation;
        float currentScale = 1f;
        float currentTargetFPS = 60f;

        void Awake()
        {
            if (Camera == null)
                Camera = GetComponent<Camera>();

            previousPosition = Camera.transform.position;
            previousRotation = Camera.transform.rotation;
            Application.targetFrameRate = maxFPS;
            QualitySettings.vSyncCount = 0;
        }

        void LateUpdate()
        {
            float deltaTime = Time.unscaledDeltaTime;
            if (deltaTime <= 0f) return;

            Vector3 positionDelta = Camera.transform.position - previousPosition;
            float linearSpeed = positionDelta.magnitude / deltaTime;

            float angularDelta = Quaternion.Angle(previousRotation, Camera.transform.rotation);
            float angularSpeed = angularDelta / deltaTime;

            previousPosition = Camera.transform.position;
            previousRotation = Camera.transform.rotation;

            float linearT = Mathf.InverseLerp(lowMotionSpeed, highMotionSpeed, linearSpeed);
            float angularT = Mathf.InverseLerp(lowAngularSpeed, highAngularSpeed, angularSpeed);
            float motionT = Mathf.Clamp01(Mathf.Max(linearT, angularT));

            bool isInteracting = IsInteracting();
            bool isMoving = motionT > 0.01f;

            if (isMoving)
            {
                currentScale = minScale;
            }
            else
            {
                currentScale = Mathf.MoveTowards(currentScale, maxScale, scaleLerpSpeed * deltaTime);
            }

            if (isInteracting)
            {
                currentTargetFPS = maxFPS;
            }
            else
            {
                float targetFPS = Mathf.Lerp(minFPS, maxFPS, motionT);
                currentTargetFPS = Mathf.MoveTowards(currentTargetFPS, targetFPS, fpsLerpSpeed * deltaTime);
            }

            Application.targetFrameRate = Mathf.RoundToInt(currentTargetFPS);
            ScalableBufferManager.ResizeBuffers(currentScale, currentScale);
        }

        bool IsInteracting()
        {
            if (Mouse.current != null)
            {
                if (Mouse.current.leftButton.isPressed ||
                    Mouse.current.rightButton.isPressed ||
                    Mouse.current.middleButton.isPressed)
                    return true;

                if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f)
                    return true;

                if (Mouse.current.scroll.ReadValue().sqrMagnitude > 0.01f)
                    return true;
            }

            if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
                return true;

            if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
                return true;

            return false;
        }

        void OnDisable()
        {
            ScalableBufferManager.ResizeBuffers(1f, 1f);
            Application.targetFrameRate = maxFPS;
        }
    }
}
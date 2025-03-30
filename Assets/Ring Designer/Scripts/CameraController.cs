using UnityEngine;
using UnityEngine.InputSystem;

namespace RingDesigner
{
    public class CameraController : MonoBehaviour
    {
        public Transform target; // Target to orbit around

        [field: SerializeField, Range(1f, 100f)]
        public float Sensitivity { get; private set; } = 20f;

        [field: SerializeField, Range(0.01f, 0.1f)]
        public float SmoothTime { get; private set; } = 0.05f;

        [field: SerializeField, Range(0.85f, 0.99f)]
        public float InertiaDamping { get; private set; } = 0.95f; // Controls inertia decay

        private Vector3 _currentRotation;
        private Vector3 _targetRotation;
        private Vector3 _rotationVelocity;
        private Vector2 _previousPointerPosition;
        private Vector2 _inertiaVelocity;
        private bool _isDragging;
        private InputAction _lookAction;

        private void Awake()
        {
            _lookAction = new InputAction("Look", InputActionType.Value, "<Pointer>/delta");
            _lookAction.Enable();
        }

        private void Start()
        {
            if (target == null)
            {
                Debug.LogError("Target not assigned!");
                enabled = false;
                return;
            }
            _currentRotation = transform.eulerAngles;
            _targetRotation = _currentRotation;
        }

        private void Update()
        {
            Vector2 pointerDelta = _lookAction.ReadValue<Vector2>();
            bool isPressing = Mouse.current.leftButton.isPressed || (Touchscreen.current?.primaryTouch.press.isPressed ?? false);

            if (isPressing)
            {
                if (!_isDragging)
                {
                    _isDragging = true;
                    _previousPointerPosition = Mouse.current.leftButton.isPressed
                        ? Mouse.current.position.ReadValue()
                        : Touchscreen.current.primaryTouch.position.ReadValue();
                    _inertiaVelocity = Vector2.zero; // Reset inertia when starting a new drag
                }
                else
                {
                    Vector2 pointerPosition = Mouse.current.leftButton.isPressed
                        ? Mouse.current.position.ReadValue()
                        : Touchscreen.current.primaryTouch.position.ReadValue();

                    Vector2 delta = pointerPosition - _previousPointerPosition;
                    _previousPointerPosition = pointerPosition;

                    _targetRotation.y += delta.x * Sensitivity * 0.01f;
                    _targetRotation.x -= delta.y * Sensitivity * 0.01f;
                    _targetRotation.x = Mathf.Clamp(_targetRotation.x, -80f, 80f);

                    _inertiaVelocity = delta * (Sensitivity * 0.01f); // Capture movement speed for inertia
                }
            }
            else
            {
                _isDragging = false;

                // Apply inertia when not dragging
                _targetRotation.y += _inertiaVelocity.x;
                _targetRotation.x -= _inertiaVelocity.y;
                _targetRotation.x = Mathf.Clamp(_targetRotation.x, -80f, 80f);

                // Decay inertia over time
                _inertiaVelocity *= InertiaDamping;
            }

            _currentRotation = Vector3.SmoothDamp(_currentRotation, _targetRotation, ref _rotationVelocity, SmoothTime);
            transform.rotation = Quaternion.Euler(_currentRotation);
            transform.position = target.position - transform.forward * 5f; // Keeps distance from target
        }

        private void OnDestroy()
        {
            _lookAction.Disable();
            _lookAction.Dispose();
        }
    }
}

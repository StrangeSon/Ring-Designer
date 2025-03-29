using UnityEngine;
using UnityEngine.InputSystem;

namespace RingDesigner
{
    public class CameraController : MonoBehaviour
    {
        public Transform target; // Target to orbit around
        public float rotationSpeed = 5f;
        public float smoothTime = 0.1f;

        private Vector3 _currentRotation;
        private Vector3 _targetRotation;
        private Vector3 _rotationVelocity;
        private bool _isDragging;
        private Vector2 _previousPointerPosition;
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

            if (Mouse.current.leftButton.isPressed || (Touchscreen.current?.primaryTouch.press.isPressed ?? false))
            {
                if (!_isDragging)
                {
                    _isDragging = true;
                    _previousPointerPosition = Mouse.current.leftButton.isPressed
                        ? Mouse.current.position.ReadValue()
                        : Touchscreen.current.primaryTouch.position.ReadValue();
                }
                else
                {
                    Vector2 pointerPosition = Mouse.current.leftButton.isPressed
                        ? Mouse.current.position.ReadValue()
                        : Touchscreen.current.primaryTouch.position.ReadValue();

                    Vector2 delta = pointerPosition - _previousPointerPosition;
                    _previousPointerPosition = pointerPosition;

                    _targetRotation.y += delta.x * rotationSpeed * Time.deltaTime;
                    _targetRotation.x -= delta.y * rotationSpeed * Time.deltaTime;
                    _targetRotation.x = Mathf.Clamp(_targetRotation.x, -80f, 80f);
                }
            }
            else
            {
                _isDragging = false;
            }

            _currentRotation = Vector3.SmoothDamp(_currentRotation, _targetRotation, ref _rotationVelocity, smoothTime);
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

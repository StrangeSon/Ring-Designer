using UnityEngine;

namespace RingDesigner
{
    public class PlanarReflectionProbeForCameraController : MonoBehaviour
    {
        [field: SerializeField]
        public Camera Camera { get; private set; }

        [field: SerializeField]
        public ReflectionProbe ReflectionProbe { get; private set; }

        // Update is called once per frame
        void Update()
        {
            ReflectionProbe.transform.position = new Vector3(
                x: Camera.transform.position.x,
                y: -Camera.transform.position.y,
                z: Camera.transform.position.z
            );
        }
    }
}
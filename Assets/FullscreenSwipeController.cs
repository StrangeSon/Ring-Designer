using UnityEngine;

namespace RingDesigner
{
    [ExecuteAlways]
    public class FullscreenSwipeController : MonoBehaviour
    {
        public Material FullscreenSwipeMaterial;

        [Range(0f, 1f)]
        public float Slide = 0f;

        [Range(0f, 1f)]
        public float Effect = 0f;

        private static readonly int SlideID = Shader.PropertyToID("_Slide");
        private static readonly int EffectID = Shader.PropertyToID("_Effect");

        void Update()
        {
            FullscreenSwipeMaterial.SetFloat(SlideID, Slide);
            FullscreenSwipeMaterial.SetFloat(EffectID, Effect);
        }
    }
}
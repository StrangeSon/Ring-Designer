using UnityEngine;

namespace RingDesigner
{
    public sealed class LimitFPS : MonoBehaviour
    {
        public bool Limit;
        public bool UseRefreshRate = true;
        public int Frequency = 60;

        void OnEnable()
        {
            Execute();
        }

        private void OnValidate()
        {
            Execute();
        }

        void Execute()
        {
            if (!Limit)
            {
                Application.targetFrameRate = -1;
            }
            else
            {
                if (UseRefreshRate)
                {
                    Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
                }
                else
                {
                    Application.targetFrameRate = Frequency;
                }
            }
        }

        private void OnDisable()
        {
            Application.targetFrameRate = -1;
        }
    }
}
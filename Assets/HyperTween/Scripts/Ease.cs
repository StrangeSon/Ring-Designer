using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperSpark.HyperTween
{
    public static class Ease
    {
        public static float InSine(float start, float end, float time)
        {
            end -= start;
            return -end * Mathf.Cos(time / 1f * (Mathf.PI / 2)) + end + start;
        }

        public static float OutSine(float start, float end, float time)
        {
            end -= start;
            return end * Mathf.Sin(time / 1f * (Mathf.PI / 2)) + start;
        }

        public static float InOutSine(float start, float end, float time)
        {
            end -= start;
            return -end / 2f * (Mathf.Cos(Mathf.PI * time / 1f) - 1) + start;
        }

        public static float InQuad(float start, float end, float time)
        {
            end -= start;
            return end * time * time + start;
        }

        public static float OutQuad(float start, float end, float time)
        {
            end -= start;
            return -end * time * (time - 2) + start;
        }

        public static float InOutQuad(float start, float end, float time)
        {
            time /= 0.5f;
            end -= start;
            if (time < 1)
                return end / 2 * time * time + start;
            time--;
            return -end / 2 * (time * (time - 2) - 1) + start;
        }

        public static float InCubic(float start, float end, float time)
        {
            end -= start;
            return end * time * time * time + start;
        }

        public static float OutCubic(float start, float end, float time)
        {
            time--;
            end -= start;
            return end * (time * time * time + 1) + start;
        }

        public static float InOutCubic(float start, float end, float time)
        {
            time /= 0.5f;
            end -= start;
            if (time < 1) 
                return end / 2 * time * time * time + start;
            time -= 2;
            return end / 2 * (time * time * time + 2) + start;
        }

        public static float InQuart(float start, float end, float time)
        {
            end -= start;
            return end * time * time * time * time + start;
        }

        public static float OutQuart(float start, float end, float time)
        {
            time--;
            end -= start;
            return -end * (time * time * time * time - 1) + start;
        }

        public static float InOutQuart(float start, float end, float time)
        {
            end -= start;
            time /= 0.5f;
            if (time < 1) 
                return end / 2 * time * time * time * time + start;
            time -= 2;
            return -end / 2 * (time * time * time * time - 2) + start;
        }

        public static float InQuint(float start, float end, float time)
        {
            end -= start;
            return end * time * time * time * time * time + start;
        }

        public static float OutQuint(float start, float end, float time)
        {
            time--;
            end -= start;
            return end * (time * time * time * time * time + 1) + start;
        }

        public static float InOutQuint(float start, float end, float time)
        {
            time /= .5f;
            end -= start;
            if (time < 1) 
                return end / 2 * time * time * time * time * time + start;
            time -= 2;
            return end / 2 * (time * time * time * time * time + 2) + start;
        }

        public static float InElastic(float start, float end, float time)
        {
            end -= start;
            float s = 1.70158f;
            float p = 0;
            float a = end;
            if (time == 0) return start;
            if ((time /= 1f) == 1) return start + end;
            if (p == 0) p = 1 * 0.3f;
            if (a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }
            return -(a * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time * 1f - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float OutElastic(float start, float end, float time)
        {
            end -= start;
            float s = 1.70158f;
            float p = 0;
            float a = end;
            if (time == 0) return start;
            if ((time /= 1f) == 1) return start + end;
            if (p == 0) p = 1 * 0.3f;
            if (a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }
            return a * Mathf.Pow(2, -10 * time) * Mathf.Sin((time * 1f - s) * (2 * Mathf.PI) / p) + end + start;
        }

        public static float InOutElastic(float start, float end, float time)
        {
            end -= start;
            float s = 1.70158f;
            float p = 0;
            float a = end;
            if (time == 0) return start;
            if ((time /= 0.5f) == 2) return start + end;
            if (p == 0) p = 0.3f * 1.5f;
            if (a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }
            if (time < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time * 1f - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (time -= 1)) * Mathf.Sin((time * 1f - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyperSpark.Utility;

namespace HyperSpark.HyperTween
{
    public sealed class HyperTweenManager : MonoBehaviour
    {
        public static HyperTweenManager Instance => Singleton<HyperTweenManager>.Instance;

        public HashSet<Tween<float>> FloatTweens = new HashSet<Tween<float>>();

        void Update()
        {
            foreach (Tween<float> tween in FloatTweens)
            {
                if (!tween.IsComplete)
                    tween.Update();
            }
        }
    }
}
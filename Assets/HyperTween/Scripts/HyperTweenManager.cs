using System.Collections.Generic;
using UnityEngine;
using HyperSpark.Utility;

namespace HyperSpark.HyperTween
{
    public sealed class HyperTweenManager : Singleton<HyperTweenManager>
    {
        private HashSet<Tween<float>> floatTweens = new HashSet<Tween<float>>();

        public void AddTween(FloatTween tween)
        {
            floatTweens.Add(tween);
        }

        public void RemoveTween(FloatTween tween)
        {
            floatTweens.Remove(tween);
        }

        void Update()
        {
            foreach (Tween<float> tween in floatTweens)
            {
                if (!tween.IsComplete)
                    tween.Update();
            }
        }
    }
}
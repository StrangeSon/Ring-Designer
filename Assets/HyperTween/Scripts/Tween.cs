using HyperSpark.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperSpark.HyperTween
{
    public abstract class Tween<T>
    {
        public TweenTimer Timer;
        public float From;
        private float to;
        public float To
        {
            get => to;
            set
            {
                State.RemoveState(TweenState.Complete);
                to = value;
            }
        }
        public float Duration => Timer.Duration;
        public TweenState State;
        public bool IsBackwards => State.Is(TweenState.Backwards);
        public bool IsPaused => State.Is(TweenState.Paused);
        public bool IsComplete => State.Is(TweenState.Complete);

        public Action<T> OnUpdate;
        public Action OnComplete;
        public Func<float, float, float, float> CurveFunc;
        public Func<bool> StopFunc;

        public bool IsPlaying => !IsComplete && !Timer.ExpiredOrNotRunning;
        protected float PauseAlpha;
        public float Alpha 
        {
            get
            {
                if (IsComplete)
                    return 1f;
                else if (IsPaused)
                    return PauseAlpha;
                else
                    return Timer.Alpha ?? 0f;
            }
        }

        public enum SearchStrategy
        {
            BinarySearch,
            Secant,
            // LinearInterpolation (Dubious use case, bad performance)
        }

        public void Play(bool fromStart = false, bool backwards = false)
        {
            if (fromStart)
            {
                State.RemoveState(TweenState.Paused);
                Timer = TweenTimer.CreateFromSeconds(Timer.Duration);
            }
            else if (IsPaused)
            {
                State.RemoveState(TweenState.Paused);
                Timer = TweenTimer.CreateFromSeconds(Timer.Duration);
                Timer.StartWithPercentOffset(PauseAlpha);
            }
            else if (IsComplete && ((!backwards && !IsBackwards) || (backwards && IsBackwards)))
                return;
            State.RemoveState(TweenState.Complete);
            bool previousBackwards = IsBackwards;
            if (backwards)
                State.AddState(TweenState.Backwards);
            else
                State.RemoveState(TweenState.Backwards);
            StartTimer(previousBackwards);
        }

        public abstract void Update();
        public abstract void Complete();
        public abstract float Evaluate();
        public abstract void Pause();
        public abstract void Stop();

        public float GetAlphaFromValue(float targetValue, bool backwards = false, SearchStrategy searchStrategy = SearchStrategy.BinarySearch)
        {
            float playbackAlpha = 1f;

            // Precursory check to see if Tween playhead is at beginning or end
            if (IsComplete)
                if (!backwards)
                    if (targetValue == To)
                        return 1f;
                else
                    if (targetValue == From)
                        return 0f;

            switch (searchStrategy)
            {
                case SearchStrategy.BinarySearch:
                    playbackAlpha = BinarySearch();
                    break;

                    float BinarySearch()
                    {
                        // target value is the value for which we want to find alpha for
                        float epsilon = 0.001f;

                        // Repeatedly reduce the range (minAlpha to maxAlpha) as we converge to the desired alpha
                        float minAlpha = 0f;
                        float maxAlpha = 1f;

                        // Perform bisection binary search to find the playback alpha
                        while (maxAlpha - minAlpha > epsilon)
                        {
                            // Recursively sample the curve at the midpoint until we find approximate alpha
                            float sampleAlpha = (minAlpha + maxAlpha) * 0.5f;
                            float sampleValue = CurveFunc(From, To, sampleAlpha);
                            if (sampleValue > targetValue)
                                maxAlpha = sampleAlpha;
                            else
                                minAlpha = sampleAlpha;
                        }

                        return (minAlpha + maxAlpha) * 0.5f;
                    }

                case SearchStrategy.Secant:
                    playbackAlpha = Secant();
                    break;

                    float Secant()
                    {
                        float epsilon = 0.001f;

                        // Initial guesses for alpha
                        float alpha0 = 0f;
                        float alpha1 = 1f;

                        while (Mathf.Abs(alpha1 - alpha0) > epsilon)
                        {
                            float sampleValue0 = CurveFunc(From, To, alpha0);
                            float sampleValue1 = CurveFunc(From, To, alpha1);

                            // Calculate the secant slope
                            float secantSlope = (sampleValue1 - sampleValue0) / (alpha1 - alpha0);

                            // Calculate the next alpha using the secant method
                            float nextAlpha = alpha1 - (sampleValue1 - targetValue) / secantSlope;

                            // Update the guesses for the next iteration
                            alpha0 = alpha1;
                            alpha1 = nextAlpha;
                        }

                        return alpha1;
                    }

            }

            return playbackAlpha;
        }

        private void StartTimer(bool previousBackwards)
        {
            if (Timer.IsRunning)
            {
                bool changed = IsBackwards != previousBackwards;
                if (!changed)
                    return;

                float currentValue = CurveFunc(To, From, Alpha);
                var alphaOffset = GetAlphaFromValue(currentValue);
                Timer.StartWithPercentOffset(alphaOffset);
            }
            else
                Timer.Start();
        }
    }
}
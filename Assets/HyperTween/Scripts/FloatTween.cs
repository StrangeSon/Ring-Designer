using System;
using UnityEngine;
using HyperSpark.Utility;

namespace HyperSpark.HyperTween
{
    public sealed class FloatTween : Tween<float>
    {
        public FloatTween(float from, float to, float duration, Action<float> onUpdate, Func<float, float, float, float> curveFunc = null, Action onComplete = default, Func<bool> stopFunc = null)
        {
            this.Timer = TweenTimer.CreateFromSeconds(duration);
            this.To = to;
            this.From = from;
            this.OnUpdate = onUpdate;
            this.CurveFunc = curveFunc ?? DefaultCurveFunc;
            this.OnComplete = onComplete;
            this.StopFunc = stopFunc;
            HyperTweenManager.Instance.FloatTweens.Add(this);
        }

        ~FloatTween()
        {
            HyperTweenManager.Instance.FloatTweens.Remove(this);
        }

        private static float DefaultCurveFunc(float start, float end, float t) => Mathf.Lerp(start, end, t);


        public override void Update()
        {
            if (StopFunc != null && StopFunc())
                return;

            if (IsComplete)
                return;

            if (Timer.Expired)
            {
                OnUpdate(IsBackwards ? From : To);
                State.AddState(TweenState.Complete);
                OnComplete?.Invoke();
                return;
            }
            if (Timer.IsRunning)
            {
                float value = Evaluate();
                OnUpdate(value);
                State.RemoveState(TweenState.Complete);
            }
        }

        public override void Complete()
        {
            State.RemoveState(TweenState.Paused);
            Timer = new TweenTimer()
            {
                ExpirationTime = Time.time,
                StartTime = Timer.StartTime,
                Duration = Timer.Duration
            };
            OnUpdate(IsBackwards ? From : To);
            State.AddState(TweenState.Complete);
            OnComplete?.Invoke();
        }

        public override void Pause()
        {
            if (IsComplete)
                return;

            PauseAlpha = Alpha;
            Timer = new TweenTimer { Duration = Timer.Duration };
            State.RemoveState(TweenState.Complete);
            State.AddState(TweenState.Paused);
        }

        public override void Stop()
        {
            Timer = new TweenTimer { Duration = Timer.Duration };
            OnUpdate(From);
            State.RemoveState(TweenState.Paused);
            State.RemoveState(TweenState.Backwards);
            State.RemoveState(TweenState.Complete);
        }

        public override float Evaluate()
        {
            return IsBackwards ? CurveFunc(To, From, Alpha) : CurveFunc(From, To, Alpha);
        }

    }
}
using UnityEngine;

namespace HyperSpark.Utility
{
    public struct TweenTimer
    {
        public float ExpirationTime;
        public float StartTime;
        public float Duration;

        public TweenTimer(float duration)
        {
            this.ExpirationTime = 0;
            this.StartTime = 0;
            this.Duration = duration;
        }

        public void Start()
        {
            this.ExpirationTime = Time.time + Duration;
            this.StartTime = Time.time;
        }

        public void StartWithPercentOffset(float percent)
        {
            float offset = Duration * percent;
            this.ExpirationTime = Time.time + Duration - offset;
            this.StartTime = Time.time - offset;
        }

        public bool IsRunning => Time.time < ExpirationTime;
        public bool Expired => StartTime > 0 && Time.time >= ExpirationTime;
        public bool ExpiredOrNotRunning => ExpirationTime == 0 || Expired;
        public float? RemainingSeconds => IsRunning ? ExpirationTime - Time.time : null;
        public float? ElapsedSeconds => IsRunning ? Time.time - StartTime : null;
        public float? Alpha => IsRunning ? ElapsedSeconds.Value / (ElapsedSeconds.Value + RemainingSeconds.Value) : null;
        public float? InverseAlpha => IsRunning ? RemainingSeconds.Value / (ElapsedSeconds.Value + RemainingSeconds.Value) : null;

        public static TweenTimer CreateFromSeconds(float seconds) => new TweenTimer(seconds);

        public static TweenTimer None => new TweenTimer(0);
    }
}

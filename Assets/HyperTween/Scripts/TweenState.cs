namespace HyperSpark.HyperTween
{
    [System.Flags]
    public enum TweenState
    {
        Backwards = 1 << 0,
        Paused = 1 << 1,
        Complete = 1 << 2,
    }
}
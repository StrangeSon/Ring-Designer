using System.Runtime.CompilerServices;

namespace HyperSpark.HyperTween
{
    public static class TweenStateExtensions
    {
        /// <summary>
        /// Checks if the vehicle state has flag for state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is(this TweenState self, TweenState flag)
        {
            return (self & flag) == flag;
        }

        /// <summary>
        /// Checks if the vehicle state does not have flag for state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNot(this TweenState self, TweenState flag)
        {
            return (self & flag) != flag;
        }

        /// <summary>
        /// Adds a flag to the vehicle state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddState(this ref TweenState self, TweenState flag)
        {
            self |= flag;
        }

        /// <summary>
        /// Removes a flag from the vehicle state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveState(this ref TweenState self, TweenState flag)
        {
            self &= ~flag;
        }
    }
}
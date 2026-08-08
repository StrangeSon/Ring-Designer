using UnityEngine;
using Unity.Scripting.LifecycleManagement;

namespace HyperSpark.Utility
{
    /// <summary>
    /// Generic MonoBehaviour singleton for Unity 6.6+ (Domain Reload disabled + CoreCLR prep).
    /// </summary>
    public abstract partial class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [AutoStaticsCleanup]
        static T instance;

        [AutoStaticsCleanup]
        static bool isQuitting;

        public static T Instance
        {
            get
            {
                if (isQuitting)
                    return null;

                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                    if (instance == null)
                    {
                        var gameObject = new GameObject($"[Singleton] {typeof(T).Name}");
                        instance = gameObject.AddComponent<T>();
                        DontDestroyOnLoad(gameObject);
                    }
                }

                return instance;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            isQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this as T)
                instance = null;
        }
    }
}
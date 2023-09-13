using UnityEngine;

namespace Milo.Singleton
{
    /// <summary>
    /// Singleton and DontDestroyOnLoad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PersistentSingletonMB<T> : SingletonMB<T> where T : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}

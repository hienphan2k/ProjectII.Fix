#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

namespace Milo.Singleton
{
    public class PersistentSingletonMBPunCallbacks<T> : SingletonMBPunCallbacks<T> where T : MonoBehaviourPunCallbacks
    {
        protected virtual void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}

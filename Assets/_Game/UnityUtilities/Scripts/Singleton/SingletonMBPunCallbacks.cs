using UnityEngine;

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

namespace Milo.Singleton
{
    public class SingletonMBPunCallbacks<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }

                return instance;
            }
        }
    }
}

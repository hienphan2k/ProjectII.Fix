using System.Collections.Generic;
using UnityEngine;

namespace Milo.Cache
{
    public class CacheCollider<T>
    {
        private static Dictionary<Collider, T> cache = new Dictionary<Collider, T>();

        public static T Get(Collider collider)
        {
            if (!cache.ContainsKey(collider))
            {
                cache.Add(collider, collider.GetComponent<T>());
            }

            return cache[collider];
        }
    }
}

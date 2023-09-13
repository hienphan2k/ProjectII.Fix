using System.Collections.Generic;
using UnityEngine;

namespace Milo.Cache
{
    public class CacheCollider2D<T>
    {
        private static Dictionary<Collider2D, T> cache = new Dictionary<Collider2D, T>();

        public static T Get(Collider2D collider2D)
        {
            if (!cache.ContainsKey(collider2D))
            {
                cache.Add(collider2D, collider2D.GetComponent<T>());
            }

            return cache[collider2D];
        }
    }
}

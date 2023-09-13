using UnityEngine;

using Milo.Cache;

namespace Milo.ExtensionMethod
{
    public static class ColliderEM
    {
        public static T GetComponentFromCache<T>(this Collider collider)
        {
            return CacheCollider<T>.Get(collider);
        }

        public static T GetComponentFromCache<T>(this Collider2D collider2D)
        {
            return CacheCollider2D<T>.Get(collider2D);
        }
    }
}

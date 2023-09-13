using System.Collections.Generic;
using UnityEngine;

namespace Milo.Pooling
{
    public static class ObjectPool
    {
        public abstract class Member : MonoBehaviour
        {
            public Pool pool;
        }

        public class Pool
        {
            private Member prefab;
            private Queue<Member> inactive;

            public Pool(Member prefab, int amount)
            {
                this.prefab = prefab;
                inactive = new Queue<Member>();

                Member member;

                for (int i = 0; i < amount; i++)
                {
                    member = Object.Instantiate(prefab);
                    member.gameObject.SetActive(false);
                    member.pool = this;
                    inactive.Enqueue(member);
                }
            }

            public Member Spawn()
            {
                Member member;

                if (inactive.Count == 0)
                {
                    member = Object.Instantiate(prefab);
                    member.pool = this;
                }
                else
                {
                    member = inactive.Dequeue();
                }

                member.gameObject.SetActive(true);
                return member;
            }

            public void Despawn(Member member)
            {
                member.gameObject.SetActive(false);
                inactive.Enqueue(member);
            }
        }

        private const int DEFAULT_AMOUNT = 10;

        private static Dictionary<Member, Pool> pools = new Dictionary<Member, Pool>();

        public static void Load(Member prefab, int amount = DEFAULT_AMOUNT)
        {
            if (!pools.ContainsKey(prefab))
            {
                pools.Add(prefab, new Pool(prefab, amount));
            }
        }

        public static Member Spawn(Member prefab)
        {
            if (!pools.ContainsKey(prefab))
            {
                Load(prefab, 1);
            }

            return pools[prefab].Spawn();
        }

        public static T Spawn<T>(Member prefab) where T : Member
        {
            return Spawn(prefab) as T;
        }

        public static void Despawn(Member member)
        {
            if (member.pool != null)
            {
                member.pool.Despawn(member);
            }
            else
            {
                Object.Destroy(member.gameObject);
            }
        }
    }
}

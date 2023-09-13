using System.Collections.Generic;

namespace Milo.ExtensionMethod
{
    public static class EnumerableEM
    {
        private static System.Random random = new System.Random();

        /// <summary>
        /// Shuffle an array or a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        public static void Shuffle<T>(this IList<T> ts)
        {
            int i = ts.Count;
            int j;

            while (i > 1)
            {
                i--;
                j = random.Next(i + 1);
                T t = ts[j];
                ts[j] = ts[i];
                ts[i] = t;
            }
        }
    }
}

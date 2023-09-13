using System.Collections;
using UnityEngine;

namespace Milo.Utilities
{
    public class Timer
    {
        public delegate void Task();

        /// <summary>
        /// Delay method call by starting a coroutine
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="delay"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Coroutine Schedule(MonoBehaviour mb, float delay, Task task)
        {
            if (mb != null && mb.gameObject.activeInHierarchy)
            {
                return mb.StartCoroutine(DoTask(delay, task));
            }

            return null;
        }

        private static IEnumerator DoTask(float delayTime, Task task)
        {
            yield return new WaitForSeconds(delayTime);
            task?.Invoke();
        }
    }
}

using UnityEngine;

namespace Milo.ExtensionMethod
{
    public static class TransformEM
    {
        /// <summary>
        /// Change transform.position.x value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        public static void ChangePositionX(this Transform transform, float x)
        {
            Vector3 position = transform.position;
            position.x = x;
            transform.position = position;
        }

        /// <summary>
        /// Change transform.position.y value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        public static void ChangePositionY(this Transform transform, float y)
        {
            Vector3 position = transform.position;
            position.y = y;
            transform.position = position;
        }

        /// <summary>
        /// Change transform.position.z value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        public static void ChangePositionZ(this Transform transform, float z)
        {
            Vector3 position = transform.position;
            position.z = z;
            transform.position = position;
        }

        /// <summary>
        /// Change transform.localPosition.x value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        public static void ChangeLocalPositionX(this Transform transform, float x)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.x = x;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// Change transform.localPosition.y value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        public static void ChangeLocalPositionY(this Transform transform, float y)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.y = y;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// Change transform.localPosition.z value
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        public static void ChangeLocalPositionZ(this Transform transform, float z)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.z = z;
            transform.localPosition = localPosition;
        }

        /// <summary>
        /// Destroy all children of an object
        /// </summary>
        /// <param name="transform"></param>
        public static void ClearAllChildren(this Transform transform)
        {
            if (transform.childCount == 0) return;

            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Destroy all children of an object in edit mode
        /// </summary>
        /// <param name="transform"></param>
        public static void ClearAllChildrenEditMode(this Transform transform)
        {
            if (transform.childCount == 0) return;

            int count = transform.childCount;

            for (int i = 0; i < count; i++)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}

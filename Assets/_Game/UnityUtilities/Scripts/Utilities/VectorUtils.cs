using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Milo.Utilities
{
    public class VectorUtils
    {
        public static Vector2 WorldToCanvasCameraExpand(Vector3 position, float baseWidth = 1080f, float baseHeight = 1920f)
        {
            float matchX = baseWidth;
            float matchY = baseHeight;
            float baseRatio = baseWidth / baseHeight;
            float screenRatio = (float)Screen.width / Screen.height;

            Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);

            if (screenRatio < baseRatio)
            {
                matchX = baseWidth;
                matchY = matchX / screenRatio;
            }
            else if (screenRatio > baseRatio)
            {
                matchY = baseHeight;
                matchX = matchY * screenRatio;
            }

            float convertRatio = matchX / Screen.width;

            return screenPoint * convertRatio - new Vector2(matchX, matchY) * 0.5f;
        }

        public List<Vector2> GenerateListVector2OnCircle(Vector2 center, int amount, float radius)
        {
            List<Vector2> path = new List<Vector2>();
            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * 2f / amount;
                path.Add(center + new Vector2(Mathf.Cos(angle) * 0.5f, Mathf.Sin(angle) * 0.5f) * radius);
            }
            return path;
        }

        public List<Vector3> GenerateListVector3OnCircle(Vector3 center, int amount, float radius)
        {
            List<Vector3> path = new List<Vector3>();
            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * 2f / amount;
                path.Add(center + new Vector3(Mathf.Cos(angle) * 0.5f, Mathf.Sin(angle) * 0.5f, 0f) * radius);
            }
            return path;
        }

#if UNITY_EDITOR
        public static Vector3 EventSceneViewMousePosition(Event eventGUI, SceneView sceneView)
        {
            Vector3 mousePosition = eventGUI.mousePosition;
            float pixels = EditorGUIUtility.pixelsPerPoint;
            mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * pixels;
            mousePosition.x *= pixels;

            return mousePosition;
        }
#endif
    }
}

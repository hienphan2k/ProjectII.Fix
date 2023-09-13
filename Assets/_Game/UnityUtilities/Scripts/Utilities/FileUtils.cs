using System.IO;
using UnityEngine;

namespace Milo.Utilities
{
    public class FileUtils
    {
        public static T ReadJsonDataFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("Error: File doesn't exist !!!");
                return default(T);
            }

            string jsonData = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(jsonData);
            return data;
        }

        public static void SaveJsonDataToFile<T>(T data, string filePath)
        {
            if (data == null)
            {
                Debug.LogError("Error: Data is null !!!");
                return;
            }

            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonData);
        }
    }
}

using UnityEditor;
using UnityEditor.SceneManagement;

namespace Milo.Editor
{
    public class SceneSwitcher
    {
        private const string PATH_TO_SCENES_FOLDER = "Assets/_Game/Scenes/";

        [MenuItem("Scenes/Test #`")]
        public static void OpenTest()
        {
            OpenScene("Test");
        }

        [MenuItem("Scenes/Test #`", true)]
        public static bool OpenTestValidate()
        {
            return OpenSceneValidate("Test");
        }

        [MenuItem("Scenes/Connect #1")]
        public static void OpenConnect()
        {
            OpenScene("Connect");
        }

        [MenuItem("Scenes/Connect #1", true)]
        public static bool OpenConnectValidate()
        {
            return OpenSceneValidate("Connect");
        }

        [MenuItem("Scenes/Game #2")]
        public static void OpenGame()
        {
            OpenScene("Game");
        }

        [MenuItem("Scenes/Game #2", true)]
        public static bool OpenGameValidate()
        {
            return OpenSceneValidate("Game");
        }

        private static void OpenScene(string sceneName)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(PATH_TO_SCENES_FOLDER + sceneName + ".unity");
            }
        }

        private static bool OpenSceneValidate(string sceneName)
        {
            return System.IO.File.Exists(PATH_TO_SCENES_FOLDER + sceneName + ".unity");
        }
    }
}

using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Settings settings;

    private bool isAdmin;
    private Scene currentScene;

    public Scene CurrentScene => currentScene;
    public Settings Settings => settings;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DataManager.Instance.LoadData();
    }

    public bool IsAdmin()
    {
        return isAdmin;
    }

    public void ActiveAdminMode()
    {
        isAdmin = true;
        DataManager.Instance.InitAdminData();
    }

    public void PhotonLoadLevel(Scene scene)
    {
        if (scene == currentScene) return;
        DOTween.KillAll();
        PhotonNetwork.LoadLevel((int)scene);
        currentScene = scene;
    }
}

[System.Serializable]
public class Settings
{
    public bool isLocalTest;
}

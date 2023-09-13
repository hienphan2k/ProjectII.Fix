using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Realtime;
using Milo.Singleton;

public class DataManager : MonoBehaviour
{
    [System.Serializable]
    public class GameData
    {
        public int accountId;
        public int missionCompletedAmount;

        [Header("Player Stat")]
        public float mentalHealth;
        public float recoveryIndex;

        [Space(10)]
        public PlayerInfo info;
        public Dictionary<int, AcquaintanceData> acquaintanceDict;

        public GameData()
        {
            accountId = 0;
            missionCompletedAmount = 0;

            mentalHealth = 50f;
            recoveryIndex = 0f;

            info = new PlayerInfo(false);
            acquaintanceDict = new Dictionary<int, AcquaintanceData>();
        }

        public bool HasAcquaintance(int viewID)
        {
            return acquaintanceDict.ContainsKey(viewID);
        }

        public void AddAcquaintance(int viewID)
        {
            acquaintanceDict.Add(viewID, new AcquaintanceData());
        }

        public AcquaintanceData GetAcquaintance(int viewID)
        {
            return acquaintanceDict[viewID];
        }
    }

    [System.Serializable]
    public class AdminData
    {
        public List<Player> photonPlayerList;

        public AdminData()
        {
            photonPlayerList = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            photonPlayerList.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            photonPlayerList.Remove(player);
        }
    }

    [System.Serializable]
    public class AcquaintanceData
    {
        public float point;
        public PlayerInfo info;

        public AcquaintanceData()
        {
            point = 0.8f;
            info = new PlayerInfo(true);
        }
    }

    public UnityAction<float> OnMentalHealthChanged;
    public UnityAction<float> OnRecoveryIndexChanged;

    public static DataManager Instance { get; private set; }
    public string[] randomPhrases;

    [SerializeField] private GameData data;
    [SerializeField] private AdminData superData;

    public GameData Data { get => data; set => data = value; }
    public AdminData SuperData { get => superData; set => superData = value; }

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void LoadData()
    {
        Data = new GameData();
        randomPhrases = File.ReadLines("Assets/_Game/Resources/Texts/EndGame.txt").ToArray();
    }

    public void SaveData()
    {

    }

    public void InitAdminData()
    {
        SuperData = new AdminData();
    }

    public bool HasAcquaintance(int viewID)
    {
        return data.HasAcquaintance(viewID);
    }

    public void AddAcquaintance(int viewID)
    {
        data.AddAcquaintance(viewID);
    }

    public PlayerInfo GetAcquaintanceInfo(int viewID)
    {
        return data.GetAcquaintance(viewID).info;
    }

    public void ChangeMentalHealth(float amount)
    {
        float mh = data.mentalHealth + amount;

        if (mh < 0f)
        {
            mh = 0f;

            // Character gone crazy & player lose
        }
        else
        {
            mh = 100f;
        }

        data.mentalHealth = mh;
        OnMentalHealthChanged?.Invoke(mh);
    }

    public void ChangeRecoveryIndex(float amount)
    {
        float ri = data.recoveryIndex + amount;

        if (ri < 0f)
        {
            ri = 0f;

            // Lose
        }
        else
        {
            ri = 100f;
        }

        data.recoveryIndex = ri;
        OnRecoveryIndexChanged?.Invoke(ri);
    }
}

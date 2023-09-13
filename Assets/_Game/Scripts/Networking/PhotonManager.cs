using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;
using Milo.Singleton;
using Milo.Utilities;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }

    [SerializeField] private List<RoomInfo> listRoomInfo;

    public UnityAction<List<RoomInfo>> UpdateRoomListAction;

    public List<RoomInfo> ListRoomInfo { get => listRoomInfo; }

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Callbacks
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.InLobby == true) return;

        Debug.Log("PhotonManager: Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("PhotonManager: Joined lobby");
        UIManager.Instance.PanelConnect.OnConnected();
        if (GameManager.Instance.IsAdmin())
        {
            Timer.Schedule(this, 0.1f, () =>
            {
                UIManager.Instance.ShowJoin();
            });
        }
        else
        {
            UIManager.Instance.ShowInfo();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        GameManager.Instance.PhotonLoadLevel(Scene.Game);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("create fail");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("PhotonManager: Join room failed");
        Debug.Log($"PhotonManager: {returnCode} {message}");

        // Handle join room failed
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        GameManager.Instance.PhotonLoadLevel(Scene.Connect);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = roomList.Count - 1; i >= 0; i--)
        {
            if (roomList[i].RemovedFromList == true) roomList.RemoveAt(i);
        }

        listRoomInfo = roomList;
        UpdateRoomListAction?.Invoke(listRoomInfo);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (GameManager.Instance.IsAdmin())
        {
            DataManager.Instance.SuperData.RemovePlayer(otherPlayer);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (GameManager.Instance.IsAdmin())
        {
            DataManager.Instance.SuperData.AddPlayer(newPlayer);
        }
    }
    #endregion

    public void ConnectToPhotonServer()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings();
        UIManager.Instance.PanelConnect.OnConnecting();
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}


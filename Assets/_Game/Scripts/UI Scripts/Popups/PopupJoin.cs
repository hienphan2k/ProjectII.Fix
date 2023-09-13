using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using Photon.Realtime;

public class PopupJoin : PopupBase
{
    [SerializeField] private Transform circleTF;
    [SerializeField] private GameObject joinBtnObj;
    [SerializeField] private GameObject createBtnObj;

    [Header("Join")]
    [SerializeField] private GameObject objJoin;
    [SerializeField] private Transform contentTF;
    [SerializeField] private RoomListItem itemPrefab;
    [SerializeField] private List<RoomListItem> listItem;

    [Header("Create")]
    [SerializeField] private GameObject objCreate;
    [SerializeField] private Transform warningTF;
    [SerializeField] private TMP_Text warningTMP;
    [SerializeField] private TMP_InputField createRoomIF;

    private string roomName;
    private Tweener circleTweener;
    private Sequence warningSequence;

    public override void Show()
    {
        base.Show();

        if (GameManager.Instance.IsAdmin() == false)
        {
            createBtnObj.SetActive(GameManager.Instance.Settings.isLocalTest);
        }

        PhotonManager.Instance.UpdateRoomListAction += UpdateRoomListUI;
        UpdateRoomListUI(PhotonManager.Instance.ListRoomInfo);

        createRoomIF.text = "Room 000";
    }

    public void UpdateRoomListUI(List<RoomInfo> listRoomInfo)
    {
        if (listItem.Count < listRoomInfo.Count)
        {
            for (int i = listItem.Count; i < listRoomInfo.Count; i++)
            {
                listItem.Add(Instantiate(itemPrefab, contentTF));
            }
        }

        for (int i = 0; i < listRoomInfo.Count; i++)
        {
            listItem[i].gameObject.SetActive(true);
            listItem[i].RoomName = listRoomInfo[i].Name;
            int temp = i;
            listItem[i].Button.onClick.AddListener(() => ChooseARoom(temp));
        }

        if (listItem.Count > listRoomInfo.Count)
        {
            for (int i = listRoomInfo.Count; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChooseARoom(int i)
    {
        roomName = listItem[i].RoomName;
    }

    public void CircleAnimation()
    {
        circleTF.gameObject.SetActive(true);
        if (circleTweener == null)
        {
            circleTweener = circleTF.DORotate(Vector3.back * 360f, 1.5f, RotateMode.FastBeyond360);
            circleTweener.SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            circleTweener.SetAutoKill(false);
        }
        else circleTweener.Restart();
    }

    #region UI Callbacks
    public void ButtonBack()
    {
        Close();
        UIManager.Instance.ShowDetail();
    }

    public void ButtonOpenJoin()
    {
        if (objJoin.activeSelf) return;
        objCreate.SetActive(false);
        objJoin.SetActive(true);
    }

    public void ButtonOpenCreate()
    {
        if (objCreate.activeSelf) return;
        objJoin.SetActive(false);
        objCreate.SetActive(true);
    }

    public void ButtonJoinRoom()
    {
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonManager.Instance.JoinRoom(roomName);
            CircleAnimation();
        }
    }

    public void ButtonCreateRoom()
    {
        if (!string.IsNullOrEmpty(createRoomIF.text))
        {
            PhotonManager.Instance.CreateRoom(createRoomIF.text);
            CircleAnimation();
        }
        else
        {
            warningTF.gameObject.SetActive(true);
            if (warningSequence == null)
            {
                warningSequence = DOTween.Sequence();
                warningSequence.Join(warningTF.DOLocalMoveY(150f, 0.75f));
                warningSequence.Join(warningTMP.DOFade(0f, 0.75f));
                warningSequence.SetEase(Ease.Linear).SetAutoKill(false);
                warningSequence.OnComplete(() => { warningTF.gameObject.SetActive(false); });
                warningSequence.Play();
            }
            else warningSequence.Restart();
        }
    }
    #endregion
}

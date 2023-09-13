using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;

public class PopupScore : PopupBase
{
    [SerializeField] private TMP_Text scoreTmp;

    public override void Show()
    {
        base.Show();

        scoreTmp.text = DataManager.Instance.Data.missionCompletedAmount.ToString();
    }

    #region UI Callbacks
    public void ButtonLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}

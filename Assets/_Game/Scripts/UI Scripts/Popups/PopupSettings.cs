using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PopupSettings : PopupBase
{
    #region UI Callbacks
    public void ButtonLeaveRoom()
    {
        Close();

        UIManager.Instance.ShowScore();
    }

    public void ButtonBack()
    {
        Close();
    }
    #endregion
}

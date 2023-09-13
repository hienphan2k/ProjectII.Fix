using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PopupInfo : PopupBase
{
    [SerializeField] private TMP_InputField nameIF;
    [SerializeField] private TMP_Dropdown roleDropdown;

    public override void Show()
    {
        base.Show();
        nameIF.text = DataManager.Instance.Data.info.name;
        roleDropdown.value = (int)DataManager.Instance.Data.info.role - 1;
    }

    #region UI Callbacks
    public void OnNameChanged()
    {
        DataManager.Instance.Data.info.name = nameIF.text;
    }

    public void OnRoleChanged()
    {
        DataManager.Instance.Data.info.role = (PlayerRole)(roleDropdown.value + 1);
    }

    public void ButtonNext()
    {
        Close();
        UIManager.Instance.ShowDetail();
    }

    public void ButtonAdmin()
    {
        GameManager.Instance.ActiveAdminMode();
        UIManager.Instance.ShowJoin();
    }
    #endregion
}

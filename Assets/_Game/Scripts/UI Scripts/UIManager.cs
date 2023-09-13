using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Milo.Singleton;

public class UIManager : SingletonMB<UIManager>
{
    [Header("Dependencies")]
    [SerializeField] private GameObject blocker;
    [SerializeField] private Transform panelCanvasTF;
    [SerializeField] private Transform popupCanvasTF;

    [Header("Panels")]
    [SerializeField] PanelConnect panelConnect;
    [SerializeField] PanelGame panelGame;

    [Header("Popups")]
    [SerializeField] private PopupInfo prefabInfo;
    [SerializeField] private PopupDetail prefabDetail;
    [SerializeField] private PopupJoin prefabJoin;
    [SerializeField] private PopupSettings prefabSettings;
    [SerializeField] private PopupTalk prefabTalk;
    [SerializeField] private PopupSignup prefabSignup;
    [SerializeField] private PopupLogin prefabLogin;
    [SerializeField] private PopupMiniMap prefabMiniMap;
    [SerializeField] private PopupMissionProgress prefabMissionProgress;
    [SerializeField] private PopupChallengeProgress prefabChallengeProgress;
    [SerializeField] private PopupMission prefabMission;
    [SerializeField] private PopupScore prefabScore;

    public PanelConnect PanelConnect { get => panelConnect; }
    public PanelGame PanelGame { get => panelGame; }

    public PopupInfo PopupInfo { get; private set; }
    public PopupDetail PopupDetail { get; private set; }
    public PopupJoin PopupJoin { get; private set; }
    public PopupSettings PopupSettings { get; private set; }
    public PopupTalk PopupTalk { get; private set; }
    public PopupSignup PopupSignup { get; private set; }
    public PopupLogin PopupLogin { get; private set; }
    public PopupMiniMap PopupMiniMap { get; private set; }
    public PopupMissionProgress PopupMissionProgress { get; private set; }
    public PopupChallengeProgress PopupChallengeProgress { get; private set; }
    public PopupMission PopupMission { get; private set; }
    public PopupScore PopupScore { get; private set; }

    #region Show Popup
    public void ShowInfo()
    {
        if (PopupInfo == null) PopupInfo = Instantiate(prefabInfo, popupCanvasTF);
        PopupInfo.Show();
    }

    public void ShowDetail()
    {
        if (PopupDetail == null) PopupDetail = Instantiate(prefabDetail, popupCanvasTF);
        PopupDetail.Show();
    }

    public void ShowJoin()
    {
        if (PopupJoin == null) PopupJoin = Instantiate(prefabJoin, popupCanvasTF);
        PopupJoin.Show();
    }

    public void ShowSettings()
    {
        if (PopupSettings == null) PopupSettings = Instantiate(prefabSettings, popupCanvasTF);
        PopupSettings.Show();
    }

    public void ShowTalk()
    {
        if (PopupTalk == null) PopupTalk = Instantiate(prefabTalk, popupCanvasTF);
        PopupTalk.Show();
    }

    public void ShowSignup()
    {
        if (PopupSignup == null) PopupSignup = Instantiate(prefabSignup, popupCanvasTF);
        PopupSignup.Show();
    }

    public void ShowLogin()
    {
        if (PopupLogin == null) PopupLogin = Instantiate(prefabLogin, popupCanvasTF);
        PopupLogin.Show();
    }

    public void ShowMiniMap()
    {
        if (PopupMiniMap == null) PopupMiniMap = Instantiate(prefabMiniMap, popupCanvasTF);
        PopupMiniMap.Show();
    }

    public void ShowMissionProgress()
    {
        if (PopupMissionProgress == null) PopupMissionProgress = Instantiate(prefabMissionProgress, popupCanvasTF);
        PopupMissionProgress.Show();
    }

    public void ShowChallengeProgress()
    {
        if (PopupChallengeProgress == null) PopupChallengeProgress = Instantiate(prefabChallengeProgress, popupCanvasTF);
        PopupChallengeProgress.Show();
    }

    public void ShowMission()
    {
        if (PopupMission == null) PopupMission = Instantiate(prefabMission, popupCanvasTF);
        PopupMission.Show();
    }

    public void ShowScore()
    {
        if (PopupScore == null) PopupScore = Instantiate(prefabScore, popupCanvasTF);
        PopupScore.Show();
    }
    #endregion

    public void SetActiveBlockInput(bool isActive)
    {
        blocker.SetActive(isActive);
    }
}

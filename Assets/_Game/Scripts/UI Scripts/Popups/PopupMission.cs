using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMission : PopupBase
{
    [SerializeField] private MissionDataUI missionDataUIPrefab;
    [SerializeField] private Transform contentTf;

    private List<MissionDataUI> missionList = new List<MissionDataUI>();

    public override void Show()
    {
        base.Show();

        if (missionList.Count != 0) return;

        PatientMissionDataSO patientMission = MissionManager.Instance.MissionDataSO;

        for (int i = 0; i < patientMission.missions.Length; i++)
        {
            MissionDataUI missionDataUI = Instantiate(missionDataUIPrefab, contentTf);
            missionDataUI.Init(patientMission.missions[i]);
            missionList.Add(missionDataUI);
        }
    }
}

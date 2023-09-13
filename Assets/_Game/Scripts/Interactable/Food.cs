using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Interactable
{
    private PatientMissionType missionType = PatientMissionType.Eat;

    public override void InteractWithPlayer(PlayerManager player)
    {
        for (int i = 0; i < MissionManager.Instance.MissionTimerList.Count; i++)
        {
            MissionTimer missionTimer = MissionManager.Instance.MissionTimerList[i];

            if (missionTimer.MissionData.missionType == missionType && missionTimer.State == MissionState.Doing)
            {
                missionTimer.SetState(MissionState.Done);
                UIManager.Instance.ShowMissionProgress();
                UIManager.Instance.PopupMissionProgress.SetMission(missionType);
            }
        }
    }
}

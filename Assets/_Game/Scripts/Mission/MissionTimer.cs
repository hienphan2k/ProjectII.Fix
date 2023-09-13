using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTimer : MonoBehaviour
{
    private PatientMission missionData;
    private MissionState state;

    public PatientMission MissionData => missionData;
    public MissionState State => state;

    private void Update()
    {
        switch (state)
        {
            case MissionState.Pending:
                if (MissionManager.Instance.PlayTime >= missionData.timeStart)
                {
                    state = MissionState.Doing;
                }
                break;
            case MissionState.Doing:
                if (MissionManager.Instance.PlayTime >= missionData.timeOver)
                {
                    state = MissionState.Fail;
                }
                break;
            case MissionState.Done:
                break;
            case MissionState.Fail:
                break;
            default:
                break;
        }
    }

    public void SetState(MissionState state)
    {
        this.state = state;
    }

    public void Init(PatientMission missionData)
    {
        this.missionData = missionData;

        state = MissionState.Pending;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Milo.Utilities;

public class MissionDataUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTmp;
    [SerializeField] private TMP_Text timeStartTmp;
    [SerializeField] private TMP_Text timeOverTmp;

    public void Init(PatientMission missionData)
    {
        nameTmp.text = missionData.missionType.ToString();
        timeStartTmp.text = TimeUtils.SecondsToString_HMS(missionData.timeStart);
        timeOverTmp.text = TimeUtils.SecondsToString_HMS(missionData.timeOver);
    }
}

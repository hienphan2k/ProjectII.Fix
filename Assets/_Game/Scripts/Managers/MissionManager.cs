using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Milo.Singleton;

public class MissionManager : SingletonMB<MissionManager>
{
    [SerializeField] private MissionTimer missionTimerPrefab;
    [SerializeField] private PatientMissionDataSO missionDataSO;
    [SerializeField] private PatientChallengeDataSO challengeDataSO;

    private float playTime;
    private Transform tf;
    private List<MissionTimer> missionTimerList = new List<MissionTimer>();

    public float PlayTime => playTime;
    public PatientMissionDataSO MissionDataSO => missionDataSO;
    public List<MissionTimer> MissionTimerList => missionTimerList;

    private void Awake()
    {
        playTime = 0f;
        tf = transform;

        MissionTimer missionTimer;
        for (int i = 0; i < missionDataSO.missions.Length; i++)
        {
            missionTimer = Instantiate(missionTimerPrefab, tf);
            missionTimer.Init(missionDataSO.missions[i]);
            missionTimerList.Add(missionTimer);
        }
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }
}

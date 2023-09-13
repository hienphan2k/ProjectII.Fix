using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Mission Data/Patient")]
public class PatientMissionDataSO : ScriptableObject
{
    public PatientMission[] missions;
}

[System.Serializable]
public class PatientMission
{
    public float timeStart;
    public float timeOver;
    public Vector3 missionPos;
    public PatientMissionType missionType;
}

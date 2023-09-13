using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/DoctorMissionSO")]
public class DoctorMissionDataSO : ScriptableObject
{
    
}

[System.Serializable]
public class DoctorMission
{
    public float timeStart;
    public float timeOver;
    public Vector3 missionPos;
    //public DoctorMiss missionType;
}

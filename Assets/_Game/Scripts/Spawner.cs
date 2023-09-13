using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [SerializeField] private MiniMapCamera miniMapCamera;
    [SerializeField] private GameObject playerPack;
    [SerializeField] private GameObject doctorMale;
    [SerializeField] private GameObject doctorFemale;
    [SerializeField] private GameObject patientMale;
    [SerializeField] private GameObject patientFemale;
    [SerializeField] private GameObject patientRelativeMale;
    [SerializeField] private GameObject patientRelativeFemale;
    [SerializeField] private GameObject adminPack;

    [Header("Junctions")]
    [SerializeField] private List<Junction> junctions;

    [Header("Test AGV")]
    [SerializeField] private GameObject agvPrefab;
    [SerializeField] private Transform agvPoint1;
    [SerializeField] private Transform agvPoint2;
    [SerializeField] private Transform agvPoint3;
    [SerializeField] private Transform agvPoint4;

    public static PlayerManager LocalPlayer { get; private set; }
    public static Admin Admin { get; private set; }

    private void Start()
    {
        if (GameManager.Instance.IsAdmin())
        {
            StartAsAdmin();
        }
        else
        {
            StartAsNormalPlayer();
        }
    }

    private void StartAsAdmin()
    {
        Admin = Instantiate(adminPack, transform.position, Quaternion.identity).GetComponent<Admin>();

        Vector3[] path = new Vector3[2];

        path[0] = agvPoint1.position;
        path[1] = agvPoint3.position;

        GameObject obj = PhotonNetwork.Instantiate(agvPrefab.name, junctions[1].transform.position, Quaternion.identity);
        AGV agv = obj.GetComponent<AGV>();
        // agv.velocity = 5.5f;
        agv.currentJunction = junctions[1]; // lay vi tri hien tai vao agv
        agv.GetGraph(this.junctions); // lay reference graph vao agv
        agv.Deliver(28); // di chuyen den junction 28

        obj = PhotonNetwork.Instantiate(agvPrefab.name, junctions[6].transform.position, Quaternion.identity);
        agv = obj.GetComponent<AGV>();
        agv.currentJunction = junctions[6];
        agv.GetGraph(this.junctions);
        agv.Deliver(30);

        path[0] = agvPoint2.position;
        path[1] = agvPoint4.position;
        
    }

    private void StartAsNormalPlayer()
    {
        object[] infos = DataManager.Instance.Data.info.ToObjs();

        GameObject playerObj = null;

        if (DataManager.Instance.Data.info.gender == Gender.Male)
        {
            switch (DataManager.Instance.Data.info.role)
            {
                case PlayerRole.Patient:
                    playerObj = PhotonNetwork.Instantiate(patientMale.name, transform.position, Quaternion.identity, 0, infos);
                    break;
                case PlayerRole.Doctor:
                    playerObj = PhotonNetwork.Instantiate(doctorMale.name, transform.position, Quaternion.identity, 0, infos);
                    break;
                case PlayerRole.PatientRelative:
                    playerObj = PhotonNetwork.Instantiate(patientRelativeMale.name, transform.position, Quaternion.identity, 0, infos);
                    break;
            }
        }
        else
        {
            switch (DataManager.Instance.Data.info.role)
            {
                case PlayerRole.Patient:
                    playerObj = PhotonNetwork.Instantiate(patientFemale.name, transform.position, Quaternion.identity, 0, infos);
                    break;
                case PlayerRole.Doctor:
                    playerObj = PhotonNetwork.Instantiate(doctorFemale.name, transform.position, Quaternion.identity, 0, infos);
                    break;
                case PlayerRole.PatientRelative:
                    playerObj = PhotonNetwork.Instantiate(patientRelativeFemale.name, transform.position, Quaternion.identity, 0, infos);
                    break;
            }
        }

        if (playerObj != null)
        {
            LocalPlayer = playerObj.GetComponentInChildren<PlayerManager>();

            miniMapCamera.SetPlayerTransform(LocalPlayer.Transform);
        }
        else
        {
            Debug.LogError("Spawner: playerObj is null");
        }
    }
}

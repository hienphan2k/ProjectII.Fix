using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class Admin : MonoBehaviour
{
    private const float UPDATE_DELAY = 2f;

    [SerializeField] private GameObject botPrefab;

    private List<Player> PhotonPlayerList => DataManager.Instance.SuperData.photonPlayerList;
    private List<PlayerManager> currentList = new List<PlayerManager>();
    private List<PlayerManager> previousList = new List<PlayerManager>();
    private List<int> disconnectedList = new List<int>();
    private List<Vector3> previousPosList = new List<Vector3>();

    private void Start()
    {
        StartCoroutine(UpdatePlayerPositionRoutine());
    }

    private IEnumerator UpdatePlayerPositionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(UPDATE_DELAY);

        while (true)
        {
            previousList = currentList;

            previousPosList = new List<Vector3>();
            for (int i = 0; i < previousList.Count; i++)
            {
                previousPosList.Add(previousList[i].Transform.position);
            }

            yield return wait;

            PlayerManager[] allPM = FindObjectsOfType<PlayerManager>();
            currentList = allPM.ToList();

            disconnectedList = new List<int>();

            for (int i = 0; i < previousList.Count; i++)
            {
                if (currentList.Contains(previousList[i]) == false)
                {
                    disconnectedList.Add(i);
                }
            }

            if (disconnectedList.Count > 0)
            {
                for (int i = 0; i < disconnectedList.Count; i++)
                {
                    Debug.Log("player leave");
                    Debug.Log(previousPosList[disconnectedList[i]]);
                    PhotonNetwork.Instantiate(botPrefab.name, previousPosList[disconnectedList[i]], Quaternion.identity);
                }
            }

            if (allPM.Length > 0)
            {
                for (int i = 0; i < allPM.Length; i++)
                {
                    Debug.Log(i + allPM[i].Transform.position.ToString());
                }
            }
        }
    }
}

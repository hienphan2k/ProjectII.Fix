using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text roomNameTMP;

    public string RoomName { get => roomNameTMP.text; set => roomNameTMP.text = value; }
    public Button Button { get => button; }

    private void Start()
    {
        if (button == null) button = GetComponent<Button>();
    }
}

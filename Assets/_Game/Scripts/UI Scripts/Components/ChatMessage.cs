using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageTMP;

    private void Start()
    {
        if (messageTMP == null) messageTMP = GetComponent<TMP_Text>();
    }

    public void OnInit(string playerName, string message)
    {
        messageTMP.text = playerName + ": " + message;
    }
}

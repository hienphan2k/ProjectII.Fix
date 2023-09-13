using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class InformationItem : MonoBehaviour
{
    [SerializeField] private TMP_Text informationTMP;

    private void Start()
    {
        if (informationTMP == null) informationTMP = GetComponent<TMP_Text>();
    }

    public void OnInit(string infoType, string info)
    {
        informationTMP.text = infoType + ": " + info;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelConnect : PanelBase
{
    [SerializeField] private TMP_Text connectTMP;

    private void Start()
    {
        UIManager.Instance.ShowLogin();
    }

    public void OnConnecting()
    {
        connectTMP.gameObject.SetActive(true);
        StartCoroutine(IEThreeDotAnimation());
    }

    private IEnumerator IEThreeDotAnimation()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            switch (i++ % 4)
            {
                case 0:
                    connectTMP.text = "CONNECTING";
                    break;
                case 1:
                    connectTMP.text = "CONNECTING.";
                    break;
                case 2:
                    connectTMP.text = "CONNECTING..";
                    break;
                case 3:
                    connectTMP.text = "CONNECTING...";
                    break;
                default:
                    break;
            }
        }
    }

    public void OnConnected()
    {
        StopCoroutine(IEThreeDotAnimation());
        connectTMP.gameObject.SetActive(false);
    }
}

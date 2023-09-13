using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;
using Milo.Utilities;

public class PanelGame : PanelBase
{
    [SerializeField] private GameObject objCrosshair;
    [SerializeField] private TMP_Text nameTmp;
    [SerializeField] private TMP_Text timeTmp;

    [Header("Stat")]
    [SerializeField] private Image mhFillImage;
    [SerializeField] private Image riFillImage;
    [SerializeField] private TMP_Text currentMhTmp;
    [SerializeField] private TMP_Text currentRiTmp;

    private void Start()
    {
        nameTmp.text = DataManager.Instance.Data.info.name;

        DataManager.Instance.OnMentalHealthChanged += OnMentalHealthDataChanged;
        DataManager.Instance.OnRecoveryIndexChanged += OnRecoveryIndexDataChanged;
        OnMentalHealthDataChanged(DataManager.Instance.Data.mentalHealth);
        OnRecoveryIndexDataChanged(DataManager.Instance.Data.recoveryIndex);
    }

    private void Update()
    {
        int time = Mathf.FloorToInt(MissionManager.Instance.PlayTime);
        timeTmp.text = TimeUtils.SecondsToString_HMS(time);
    }

    public void SetActiveCrosshair(bool isActive)
    {
        objCrosshair.SetActive(isActive);
    }

    public void OnMentalHealthDataChanged(float mh)
    {
        float percent = mh / 100f;
        mhFillImage.fillAmount = percent;
        currentMhTmp.text = Mathf.RoundToInt(percent * 100f).ToString();
    }

    public void OnRecoveryIndexDataChanged(float ri)
    {
        float percent = ri / 100f;
        riFillImage.fillAmount = percent;
        currentRiTmp.text = Mathf.RoundToInt(percent * 100f).ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class PopupMissionProgress : PopupBase
{
    [SerializeField] private TMP_Text titleTMP;
    [SerializeField] private Image fillImage;

    private Tweener progressTweener;

    public override void Show()
    {
        base.Show();

        titleTMP.text = "eating";
        fillImage.fillAmount = 0;
        progressTweener = DOVirtual.Float(0f, 1f, 5f, value =>
        {
            fillImage.fillAmount = value;
        }).OnComplete(() =>
        {
            Debug.Log("Complete Mission");
            DataManager.Instance.Data.missionCompletedAmount++;
            Close();
        });
    }

    public void SetMission(PatientMissionType type)
    {
        switch (type)
        {
            case PatientMissionType.CheckedByDoctor:
                titleTMP.text = "Checking By Doctor";
                break;
            case PatientMissionType.Eat:
                titleTMP.text = "Eating";
                break;
            case PatientMissionType.Medicines:
                titleTMP.text = "Medicines";
                break;
            default:
                break;
        }
    }

    #region UI Callbacks
    public void ButtonBack()
    {
        DOTween.Kill(progressTweener);
        Close();
    }
    #endregion
}

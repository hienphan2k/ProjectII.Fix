using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class PopupChallengeProgress : PopupBase
{
    [SerializeField] private TMP_Text titleTMP;
    [SerializeField] private Image fillImage;

    private Tweener progressTweener;

    public override void Show()
    {
        base.Show();

        titleTMP.text = "Pooping :))";
        fillImage.fillAmount = 0;
        progressTweener = DOVirtual.Float(0f, 1f, 5f, value =>
        {
            fillImage.fillAmount = value;
        }).OnComplete(() =>
        {
            Debug.Log("Challenge Mission");
            Close();
        });
    }

    #region UI Callbacks
    public void ButtonBack()
    {
        DOTween.Kill(progressTweener);
        Close();
    }
    #endregion
}

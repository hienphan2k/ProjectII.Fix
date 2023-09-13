using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class InterestListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text interestTMP;
    [SerializeField] private Image buttonImage;

    private Interest interest;

    public void OnInstantiate(Interest interest)
    {
        this.interest = interest;
        interestTMP.text = interest.ToString();
        buttonImage.color = DataManager.Instance.Data.info.interests.Contains(interest) ? Color.gray : Color.white;
    }

    public void OnClick()
    {
        if (!DataManager.Instance.Data.info.interests.Contains(interest))
        {
            DataManager.Instance.Data.info.interests.Add(interest);
            buttonImage.color = Color.gray;
        }
        else
        {
            DataManager.Instance.Data.info.interests.Remove(interest);
            buttonImage.color = Color.white;
        }
    }
}

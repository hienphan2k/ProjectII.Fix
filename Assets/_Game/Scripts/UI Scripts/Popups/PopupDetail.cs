using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PopupDetail : PopupBase
{
    [Header("Gender")]
    [SerializeField] private GameObject maleCheckObj;
    [SerializeField] private GameObject femaleCheckObj;
    [SerializeField] private GameObject otherCheckObj;

    [Header("Age")]
    [SerializeField] private Slider ageSlider;
    [SerializeField] private TMP_InputField ageIF;

    [Header("Marital Status")]
    [SerializeField] private TMP_Dropdown maritalStatusDropdown;

    [Header("Interests")]
    [SerializeField] private Transform contentTransform;
    [SerializeField] private InterestListItem itemPrefab;

    public override void Show()
    {
        base.Show();

        maleCheckObj.SetActive(DataManager.Instance.Data.info.gender == Gender.Male);
        femaleCheckObj.SetActive(DataManager.Instance.Data.info.gender == Gender.Female);
        otherCheckObj.SetActive(DataManager.Instance.Data.info.gender == Gender.Other);

        ageSlider.value = DataManager.Instance.Data.info.age;
        ageIF.text = DataManager.Instance.Data.info.age.ToString();

        maritalStatusDropdown.value = (int)DataManager.Instance.Data.info.ms - 1;

        List<Interest> interests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();
        for (int i = 0; i < interests.Count; i++)
        {
            Instantiate(itemPrefab, contentTransform).OnInstantiate(interests[i]);
        }
    }

    #region UI Callbacks
    public void ButtonMale()
    {
        maleCheckObj.SetActive(true);
        femaleCheckObj.SetActive(false);
        otherCheckObj.SetActive(false);
        DataManager.Instance.Data.info.gender = Gender.Male;
    }

    public void ButtonFemale()
    {
        maleCheckObj.SetActive(false);
        femaleCheckObj.SetActive(true);
        otherCheckObj.SetActive(false);
        DataManager.Instance.Data.info.gender = Gender.Female;
    }

    public void ButtonOther()
    {
        maleCheckObj.SetActive(false);
        femaleCheckObj.SetActive(false);
        otherCheckObj.SetActive(true);
        DataManager.Instance.Data.info.gender = Gender.Other;
    }

    public void OnAgeSliderChanged()
    {
        ageIF.text = ageSlider.value.ToString();
        DataManager.Instance.Data.info.age = (int)ageSlider.value;
    }

    public void OnAgeIFChanged()
    {
        int age;
        if (int.TryParse(ageIF.text, out age) && (age >= 0 && age <= 150))
        {
            ageSlider.value = age;
            DataManager.Instance.Data.info.age = age;
        }
        else
        {
            ageIF.text = ageSlider.value.ToString();
        }
    }

    public void OnMaritalStatusChanged()
    {
        DataManager.Instance.Data.info.ms = (MaritalStatus)(maritalStatusDropdown.value + 1);
    }

    public void ButtonBack()
    {
        Close();
        UIManager.Instance.ShowInfo();
    }

    public void ButtonNext()
    {
        Close();
        UIManager.Instance.ShowJoin();
    }
    #endregion
}

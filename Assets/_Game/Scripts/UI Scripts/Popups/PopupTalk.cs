using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Photon.Pun;

public class PopupTalk : PopupBase
{
    [Header("Message")]
    [SerializeField] private Transform messageContent;
    [SerializeField] private TMP_InputField messageIF;
    [SerializeField] private ChatMessage messagePrefab;
    [SerializeField] private float autoChatDelay = 3f;

    [Header("Information")]
    [SerializeField] private Transform informationContent;
    [SerializeField] private TMP_Text infoTitleTMP;
    [SerializeField] private InformationItem infoItemPrefab;
    [SerializeField] private float showInfoDelay = 5f;

    private List<InformationItem> cacheInfoItems = new List<InformationItem>();

    private Coroutine infoCoroutine;
    private Coroutine talkCoroutine;

    public override void Show()
    {
        base.Show();

        infoTitleTMP.text = Spawner.LocalPlayer.OtherPhotonView.Owner.NickName + "'s Information";

        for (int i = 0; i < cacheInfoItems.Count; i++)
        {
            Destroy(cacheInfoItems[i]);
        }
    }

    public override void Close()
    {
        base.Close();
        if (infoCoroutine != null) StopCoroutine(infoCoroutine);
        if (talkCoroutine != null) StopCoroutine(talkCoroutine);
    }

    public void ShowOtherInfo(PhotonView otherPV)
    {
        if (!DataManager.Instance.HasAcquaintance(otherPV.ViewID))
        {
            DataManager.Instance.AddAcquaintance(otherPV.ViewID);
        }

        infoCoroutine = StartCoroutine(IEShowOtherInfo(otherPV));
    }

    private IEnumerator IEShowOtherInfo(PhotonView otherPV)
    {
        PlayerInfo newInfo = PlayerInfo.Objs2Info(otherPV.InstantiationData);

        ShowInformation("name", Spawner.LocalPlayer.OtherPhotonView.Owner.NickName);

        if (DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).role == PlayerRole.None)
        {
            yield return new WaitForSeconds(showInfoDelay);
            ShowInformation("role", newInfo.role.ToString());
            DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).role = newInfo.role;
        }
        else
        {
            yield return null;
            ShowInformation("role", newInfo.role.ToString());
        }

        if (DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).gender == Gender.None)
        {
            yield return new WaitForSeconds(showInfoDelay);
            ShowInformation("gender", newInfo.gender.ToString());
            DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).gender = newInfo.gender;
        }
        else
        {
            yield return null;
            ShowInformation("gender", newInfo.gender.ToString());
        }

        if (DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).age == 0)
        {
            yield return new WaitForSeconds(showInfoDelay);
            ShowInformation("age", newInfo.age.ToString());
            DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).age = newInfo.age;
        }
        else
        {
            yield return null;
            ShowInformation("age", newInfo.age.ToString());
        }

        if (DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).ms == MaritalStatus.None)
        {
            yield return new WaitForSeconds(showInfoDelay);
            ShowInformation("marital status", newInfo.ms.ToString());
            DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).ms = newInfo.ms;
        }
        else
        {
            yield return null;
            ShowInformation("marital status", newInfo.ms.ToString());
        }

        for (int i = 0; i < DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).interests.Count; i++)
        {
            ShowInformation("interested in", DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).interests[i].ToString());
        }

        for (int i = DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).interests.Count; i < newInfo.interests.Count; i++)
        {
            yield return new WaitForSeconds(showInfoDelay);
            ShowInformation("interested in", newInfo.interests[i].ToString());
            DataManager.Instance.GetAcquaintanceInfo(otherPV.ViewID).interests.Add(newInfo.interests[i]);
        }

        ShowInformation("status", "completed");
    }

    private void ShowInformation(string infoType, string info)
    {
        InformationItem item = Instantiate(infoItemPrefab, informationContent);
        item.OnInit(infoType, info);
        cacheInfoItems.Add(item);
    }

    public void StartAutoChat()
    {
        talkCoroutine = StartCoroutine(IEAutoChat());
    }

    private IEnumerator IEAutoChat()
    {
        SendChatMessage("Hello");
        while (true)
        {
            yield return new WaitForSeconds(autoChatDelay);
            int random = Random.Range(0, DataManager.Instance.randomPhrases.Length);
            SendChatMessage(DataManager.Instance.randomPhrases[random]);
        }
    }

    public void ReceiveMessage(string message)
    {
        Instantiate(messagePrefab, messageContent).OnInit(Spawner.LocalPlayer.OtherPhotonView.Owner.NickName, message);
    }

    private void SendChatMessage(string message)
    {
        Instantiate(messagePrefab, messageContent).OnInit(DataManager.Instance.Data.info.name, message);
        Spawner.LocalPlayer.SendChatMessage(message);
    }

    #region UI Callbacks
    public void ButtonSend()
    {
        SendChatMessage(messageIF.text);
    }

    public void ButtonStopTalking()
    {
        Close();
        Spawner.LocalPlayer.StopTalking();
        //Spawner.PM.CalculateMH();
    }
    #endregion
}

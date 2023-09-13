using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PopupLogin : PopupBase
{
    [SerializeField] TMP_InputField usernameIF;
    [SerializeField] TMP_InputField passwordIF;
    [SerializeField] TMP_Text notiTmp;

    private void Start()
    {
        passwordIF.contentType = TMP_InputField.ContentType.Password;
    }

    public override void Show()
    {
        base.Show();

        notiTmp.text = "";
    }

    #region UI Callbacks
    public void OnClickLogin()
    {
        //PhotonManager.Instance.ConnectToPhotonServer();
        //Close();
        //return;

        string username = usernameIF.text;
        string password = passwordIF.text;

        Credential cred = new Credential(username, password);

        if (username.Length < 6 || username.Length > 20)
        {
            notiTmp.text = "Username's length >= 6 and <= 20 characters";
        }
        else if (password.Length < 6 || password.Length > 100)
        {
            notiTmp.text = "Password's length >= 6 and <= 20 characters";
        }
        else
        {
            NetworkManager.Instance.Login(cred, (int id, int role) =>
            {
                DataManager.Instance.Data.accountId = id;

                if (role == 0)
                {
                    GameManager.Instance.ActiveAdminMode();
                    PhotonManager.Instance.ConnectToPhotonServer();
                }
                else
                {
                    PhotonManager.Instance.ConnectToPhotonServer();
                }

                Close();
            }, (LoginError error) =>
            {
                switch (error)
                {
                    case LoginError.NetworkError:
                        notiTmp.text = "Network Error";
                        break;
                    case LoginError.ServerError:
                        notiTmp.text = "Server Error";
                        break;
                    case LoginError.AccountInvalid:
                        notiTmp.text = "Account Invalid";
                        break;
                    default:
                        break;
                }
            });
        }
    }

    public void OnClickSignup()
    {
        UIManager.Instance.ShowSignup();
        Close();
    }
    #endregion
}


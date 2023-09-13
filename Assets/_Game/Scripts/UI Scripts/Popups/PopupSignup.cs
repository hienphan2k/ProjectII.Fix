using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupSignup : PopupBase
{
    [SerializeField] private TMP_InputField usernameIF;
    [SerializeField] private TMP_InputField passwordIF;
    [SerializeField] private TMP_InputField retypeIF;
    [SerializeField] private TMP_Text notiTmp;

    private void Start()
    {
        passwordIF.contentType = TMP_InputField.ContentType.Password;
        retypeIF.contentType = TMP_InputField.ContentType.Password;
    }

    public override void Show()
    {
        base.Show();

        notiTmp.text = "";
    }

    #region UI Events
    public void OnClickSignup()
    {
        string username = usernameIF.text;
        string password = passwordIF.text;
        string retype = retypeIF.text;

        if (username.Length < 6 || username.Length > 20)
        {
            notiTmp.text = "Username's length >= 6 and <= 20 characters";
        }
        else if (password.Length < 6 || password.Length > 100)
        {
            notiTmp.text = "Password's length >= 6 and <= 20 characters";
        }
        else if (string.Equals(password, retype) == false)
        {
            notiTmp.text = "Password doesn't match";
        }
        else
        {
            Credential cred = new Credential(username, password);
            NetworkManager.Instance.Signup(cred, (int id) =>
            {
                Debug.Log(id);
            }, (SignupError error) =>
            {
                switch (error)
                {
                    case SignupError.NetworkError:
                        notiTmp.text = "Network Error";
                        break;
                    case SignupError.ServerError:
                        notiTmp.text = "Server Error";
                        break;
                    case SignupError.UsernameExist:
                        notiTmp.text = "Username already existed";
                        break;
                    default:
                        break;
                }
            });
        }
    }

    public void OnClickLogin()
    {
        UIManager.Instance.ShowLogin();
        Close();
    }
    #endregion
}

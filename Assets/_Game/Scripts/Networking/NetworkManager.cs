using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private const string SERVER_ERROR = "server_error";
    private const string USERNAME_EXISTED = "username_existed";
    private const string ACCOUNT_INVALID = "account_invalid";

    public static NetworkManager Instance { get; private set; }
    [SerializeField] private string url;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Signup(Credential cred, UnityAction<int> OnSuccess, UnityAction<SignupError> OnFail)
    {
        StartCoroutine(IESignup(url + "/signup", cred, OnSuccess, OnFail));
    }

    private IEnumerator IESignup(string url, Credential cred, UnityAction<int> OnSuccess, UnityAction<SignupError> OnFail)
    {
        var jsonData = JsonUtility.ToJson(cred);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                OnFail?.Invoke(SignupError.NetworkError);
            }
            else
            {
                if (request.isDone)
                {
                    string result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);

                    if (int.TryParse(result, out int id))
                    {
                        OnSuccess?.Invoke(id);
                    }
                    else
                    {
                        if (string.Equals(result, USERNAME_EXISTED))
                        {
                            OnFail?.Invoke(SignupError.UsernameExist);
                        }
                        else if (string.Equals(result, SERVER_ERROR))
                        {
                            OnFail?.Invoke(SignupError.ServerError);
                        }
                    }
                }
                else
                {
                    OnFail?.Invoke(SignupError.NetworkError);
                }
            }
        }
    }

    public void Login(Credential cred, UnityAction<int, int> OnSuccess, UnityAction<LoginError> OnFail)
    {
        StartCoroutine(IELogin(url + "/login", cred, OnSuccess, OnFail));
    }

    private IEnumerator IELogin(string url, Credential cred, UnityAction<int, int> OnSuccess, UnityAction<LoginError> OnFail)
    {
        var jsonData = JsonUtility.ToJson(cred);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                OnFail?.Invoke(LoginError.NetworkError);
            }
            else
            {
                if (request.isDone)
                {
                    string result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    string[] results = result.Split('-');

                    Debug.Log(result);

                    if (results.Length == 2
                        && int.TryParse(results[0], out int id)
                        && int.TryParse(results[1], out int role))
                    {
                        Debug.Log(id);
                        Debug.Log(role);

                        OnSuccess?.Invoke(id, role);
                    }
                    else
                    {
                        if (string.Equals(result, ACCOUNT_INVALID))
                        {
                            OnFail?.Invoke(LoginError.AccountInvalid);
                        }
                        else if (string.Equals(result, SERVER_ERROR))
                        {
                            OnFail?.Invoke(LoginError.ServerError);
                        }
                    }
                }
                else
                {
                    Debug.Log("Error!");
                    OnFail?.Invoke(LoginError.NetworkError);
                }
            }
        }
    }
}

public enum SignupError
{
    NetworkError,
    ServerError,
    UsernameExist,
}

public enum LoginError
{
    NetworkError,
    ServerError,
    AccountInvalid,
}

public class Credential
{
    public string username;
    public string password;

    public Credential(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

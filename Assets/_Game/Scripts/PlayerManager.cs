using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Milo.ExtensionMethod;

public class PlayerManager : MonoBehaviour, ICheckCollision
{
    [Header("Move & Look")]
    

    [Header("Components")]
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Transform cachedTransform;
    [SerializeField] private Transform eyesTransform;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerVisual playerVisual;

    [Header("Dependencies")]
    [SerializeField] private GameObject inputHandlerObj;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private Camera mainCamera;

    private float moveSpeed = 10.0f;
    private float rotationSpeed = 1.0f;
    private float topClamp = 89f;
    private float botClamp = -89f;

    private float targetPitch;
    private float countTimeMH;
    private float countTimeRI;
    private bool isStop;
    private bool isFreshAir;
    private bool isOpening;
    private bool canMove = true;

    public Transform Transform { get => cachedTransform; }
    public PhotonView OtherPhotonView { get; private set; }
    public PlayerManager OtherPlayerManager { get; private set; }

    #region MonoBehaviour Callbacks
    private void Start()
    {
        if (photonView.IsMine == false)
        {
            Destroy(inputHandlerObj);
            Destroy(mainCameraObj);
            return;
        }

        inputHandlerObj.SetActive(true);
        mainCameraObj.SetActive(true);
        playerVisual.EnableTransparent();
        photonView.Owner.NickName = DataManager.Instance.Data.info.name;
    }

    private void Update()
    {
        if (photonView.IsMine == false) return;

        Move();

        if (inputHandler.isLook)
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Instance.ShowSettings();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isOpening)
            {
                isOpening = false;
                UIManager.Instance.PopupMiniMap.Close();
            }
            else
            {
                isOpening = true;
                UIManager.Instance.ShowMiniMap();
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            UIManager.Instance.ShowMission();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SendTalkRequest();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AcceptTalkRequest();
            UIManager.Instance.ShowTalk();
        }
    }

    private void LateUpdate()
    {
        if (photonView.IsMine == false) return;

        Rotate();
    }

    private void OnMouseOver()
    {
        if (photonView.IsMine == true) return;

        playerVisual.OnMouseOverPlayer();
    }

    private void OnMouseExit()
    {
        if (photonView.IsMine == true) return;

        playerVisual.OnMouseExitPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FreshArea"))
        {
            isFreshAir = !isFreshAir;
            Debug.Log("throw fresh area");
        }
    }
    #endregion

    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void Move()
    {
        if (!canMove) return;

        if (inputHandler.move != Vector2.zero)
        {
            Vector3 inputDirection = cachedTransform.right * inputHandler.move.x + cachedTransform.forward * inputHandler.move.y;
            controller.Move(inputDirection.normalized * moveSpeed * Time.deltaTime);
            playerVisual.ChangeAnimation(PlayerAnimation.Run);

            if (isStop)
            {
                countTimeMH = 0f;
                countTimeRI = 0f;
                isStop = false;
            }

            countTimeMH += Time.deltaTime;
            countTimeRI += Time.deltaTime;

            if (countTimeMH > 5f)
            {
                //if (isFreshAir && recoveryIndex < 100) UIManager.Instance.PanelGame.UpdateRI(++recoveryIndex);
                //else if (!isFreshAir && recoveryIndex < 80) UIManager.Instance.PanelGame.UpdateRI(++recoveryIndex);
                //countTimeMH = 0f;
            }
        }
        else
        {
            playerVisual.ChangeAnimation(PlayerAnimation.Idle);

            if (!isStop)
            {
                countTimeMH = 0f;
                countTimeRI = 0f;
                isStop = true;
            }

            countTimeMH += Time.deltaTime;
            countTimeRI += Time.deltaTime;

            //if (countTimeMH > 5f)
            //{
            //    if (mentalHealth > 0) UIManager.Instance.PanelGame.UpdateMH(--mentalHealth);
            //    countTimeMH = 0f;
            //}

            //if (countTimeRI > 5f)
            //{
            //    if (recoveryIndex > 0) UIManager.Instance.PanelGame.UpdateRI(--recoveryIndex);
            //    countTimeRI = 0f;
            //}
        }
    }

    private void Rotate()
    {
        if (inputHandler.look.sqrMagnitude < 0.0001f) return;
        targetPitch += inputHandler.look.y * rotationSpeed;
        if (targetPitch < -360f) targetPitch += 360f;
        else if (targetPitch > 360f) targetPitch -= 360f;
        targetPitch = Mathf.Clamp(targetPitch, botClamp, topClamp);
        eyesTransform.localRotation = Quaternion.Euler(targetPitch, 0f, 0f);
        cachedTransform.Rotate(Vector3.up * inputHandler.look.x * rotationSpeed);
    }

    private void Interact()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, Constants.INTERACTABLE_LAYER))
        {
            if (hit.collider == null) return;
            OtherPhotonView = hit.collider.GetComponentFromCache<PhotonView>();

            if (OtherPhotonView != null)
            {
                if ((transform.position - OtherPhotonView.transform.position).sqrMagnitude < 15)
                {
                    OtherPlayerManager = (OtherPhotonView.Owner.TagObject as GameObject).GetComponentInChildren<PlayerManager>();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.InteractWithPlayer(this);
                }
            }
        }
    }

    public void CalculateMH()
    {
        PlayerInfo myInfo = DataManager.Instance.Data.info;
        PlayerInfo otherInfo = DataManager.Instance.GetAcquaintanceInfo(OtherPhotonView.ViewID);

        float percent = 0.8f;
        if (otherInfo.gender != Gender.None)
            percent = myInfo.gender == otherInfo.gender ? percent + 0.05f : percent - 0.05f;
        if (otherInfo.age > 0)
            percent = Mathf.Abs(myInfo.age - otherInfo.age) <= 10 ? percent + 0.05f : percent - 0.05f;
        if (otherInfo.ms != MaritalStatus.None)
            percent = myInfo.ms == otherInfo.ms ? percent + 0.05f : percent - 0.05f;

        int same = 0;
        for (int i = 0; i < otherInfo.interests.Count; i++)
        {
            if (myInfo.interests.Contains(otherInfo.interests[i])) same++;
        }

        if (same > 0) percent += same * 0.02f;
        Debug.Log("Increase percent: " + percent.ToString());

        //float random = Random.Range(0f, 100f);
        //if (random >= 0f && random <= percent * 100f)
        //{
        //    Debug.Log("Increase");
        //    if (mentalHealth < 95) mentalHealth += 5;
        //}
        //else
        //{
        //    Debug.Log("Decrease");
        //    if (mentalHealth > 5) mentalHealth -= 5;
        //}
        //UIManager.Instance.PanelGame.UpdateMH(mentalHealth);
    }

    #region RPC
    public void SendTalkRequest()
    {
        OtherPlayerManager.ReceiveTalkRequest(photonView.Owner);
    }

    private void ReceiveTalkRequest(Player sender)
    {
        photonView.RPC(nameof(PunRPCReceiveTalkRequest), photonView.Owner, sender);
    }

    [PunRPC]
    private void PunRPCReceiveTalkRequest(Player sender)
    {
        Debug.Log("Receive request");

        OtherPhotonView = (sender.TagObject as GameObject).GetComponentInChildren<PhotonView>();
        OtherPlayerManager = (sender.TagObject as GameObject).GetComponentInChildren<PlayerManager>();
    }

    public void AcceptTalkRequest()
    {
        OtherPlayerManager.StartTalk();
    }

    private void StartTalk()
    {
        photonView.RPC(nameof(PunRPCStartTalk), photonView.Owner);
    }

    [PunRPC]
    private void PunRPCStartTalk()
    {
        OpenTalkPopup();
    }

    public void OpenTalkPopup()
    {
        UIManager.Instance.ShowTalk();
        UIManager.Instance.PopupTalk.ShowOtherInfo(OtherPhotonView);
        UIManager.Instance.PopupTalk.StartAutoChat();
        UIManager.Instance.PopupSettings?.Close();
    }

    public void SendChatMessage(string message)
    {
        OtherPlayerManager.ReceiveMessage(message);
    }

    private void ReceiveMessage(string message)
    {
        photonView.RPC(nameof(PunRPCReceiveMessage), photonView.Owner, message);
    }

    [PunRPC]
    private void PunRPCReceiveMessage(string message)
    {
        UIManager.Instance.PopupTalk.ReceiveMessage(message);
    }

    public void StopTalking()
    {
        OtherPlayerManager.ReceiveStopTalking();
        CalculateMH();
    }

    private void ReceiveStopTalking()
    {
        photonView.RPC(nameof(PunRPCReceiveStopTalking), photonView.Owner);
    }

    [PunRPC]
    private void PunRPCReceiveStopTalking()
    {
        UIManager.Instance.PopupTalk.Close();
        CalculateMH();
    }
    #endregion
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Milo.ExtensionMethod;

public class MiniMapCamera : MonoBehaviour
{
    public static UnityAction<bool> SetActiveExpandAction;

    [SerializeField] private GameObject miniMapCameraObj;
    [SerializeField] private GameObject miniMapExpandCameraObj;

    private Transform playerTransform;
    private Transform cachedTransform;
    private Vector3 cameraPosition;

    private void Awake()
    {
        cachedTransform = transform;
        cameraPosition = cachedTransform.position;
    }

    private void OnEnable()
    {
        SetActiveExpandAction += SetActiveExpand;
    }

    private void OnDisable()
    {
        SetActiveExpandAction -= SetActiveExpand;
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        cameraPosition.x = playerTransform.position.x;
        cameraPosition.z = playerTransform.position.z;
        cachedTransform.position = cameraPosition;

        Vector3 temp = cachedTransform.eulerAngles;
        temp.y = playerTransform.eulerAngles.y;
        cachedTransform.eulerAngles = temp;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public void SetActiveExpand(bool isActive)
    {
        miniMapCameraObj.SetActive(!isActive);
        miniMapExpandCameraObj.SetActive(isActive);
    }
}

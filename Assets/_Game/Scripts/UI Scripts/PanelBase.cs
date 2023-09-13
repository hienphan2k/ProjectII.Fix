using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    public virtual void Open()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }
}

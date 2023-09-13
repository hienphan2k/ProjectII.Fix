using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public virtual void Show()
    {
        if (!gameObject.activeSelf) 
        {
            gameObject.SetActive(true);
            OpenAnimation();
        }

        if (GameManager.Instance.CurrentScene == Scene.Game)
        {
            Spawner.LocalPlayer.CanMove(false);
            InputHandler.SetActiveLookAction(false);
            UIManager.Instance.PanelGame.SetActiveCrosshair(false);
        }
    }

    public virtual void Close()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            CloseAnimation();
        }

        if (GameManager.Instance.CurrentScene == Scene.Game)
        {
            Spawner.LocalPlayer.CanMove(true);
            InputHandler.SetActiveLookAction(true);
            UIManager.Instance.PanelGame.SetActiveCrosshair(true);
        }
    }

    protected virtual void OpenAnimation() { }

    protected virtual void CloseAnimation() { }
}

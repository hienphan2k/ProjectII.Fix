using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMiniMap : PopupBase
{
    public override void Show()
    {
        base.Show();
        MiniMapCamera.SetActiveExpandAction(true);
    }

    public override void Close()
    {
        base.Close();
        MiniMapCamera.SetActiveExpandAction(false);
    }
}

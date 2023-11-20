using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideDebug : MonoBehaviour
{
    public ViewController DebugView;

    [ContextMenu("ShowDebugView")]
    public void ShowDebugView()
    {
        UISystem.Instance.ShowView(DebugView);
    }

    [ContextMenu("HideDebugView")]
    public void HideDebugView()
    {
        UISystem.Instance.HideView(DebugView);
    }
}

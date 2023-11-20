using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEventListener : MonoBehaviour
{
    [SerializeField]
    private IdContainerGameEvent _DebuggerEvent;

    private void OnEnable()
    {
        _DebuggerEvent.Subscribe(DebugCallback);
    }

    private void DebugCallback(GameEvent evt)
    {
        IdContainerGameEvent idContGameEvt = (IdContainerGameEvent)evt;
        Debug.Log(idContGameEvt.IdContainer.Id);
    }
}

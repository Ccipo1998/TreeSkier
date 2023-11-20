using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEventInvoker : MonoBehaviour
{
    [SerializeField]
    private IdContainerGameEvent _DebuggerEvent;

    [SerializeField]
    private idContainer _DebugIdContainer;

    [ContextMenu("InvokeEvent")]
    public void InvokeEvent()
    {
        _DebuggerEvent.IdContainer = _DebugIdContainer;
        _DebuggerEvent?.Invoke();
    }
}

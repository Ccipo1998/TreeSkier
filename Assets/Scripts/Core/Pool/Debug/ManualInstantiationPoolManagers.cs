using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualInstantiationPoolManagers : MonoBehaviour
{
    [SerializeField]
    private string _SceneName;

    [ContextMenu("DebugInstantiatePools")]

    public void DebugInstantiatePools()
    {
        PoolingSystem.Instance.SetupPoolManagersForScene(_SceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
    [SerializeField]
    private Spawner _Spawner;

    [ContextMenu("DebugSpawnerSpawn")]

    public void DebugSpawnerSpawn()
    {
        _Spawner.SpawnSnowFlakes(3);
    }
}

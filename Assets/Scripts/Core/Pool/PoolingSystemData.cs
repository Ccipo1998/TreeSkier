using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolingSystemData", menuName = "ScriptableObjects/PoolingSystemData", order = 1)]

public class PoolingSystemData : ScriptableObject
{
    public List<PoolManagerBinding> PoolManagerBindings;
}

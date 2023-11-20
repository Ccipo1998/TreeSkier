using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameObjectInstantiate : OnEnterUnit
{
    // Bolt syntax
    [DoNotSerialize]
    public ValueInput GameObjectPrefab;

    protected override void VariablesDefinition()
    {
        GameObjectPrefab = ValueInput<GameObject>("Game object prefab", null);
    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        GameObject obj = arg.GetValue<GameObject>(GameObjectPrefab);
        if (obj != null)
            GameObject.Instantiate(obj);

        return OutputTrigger;
    }
}


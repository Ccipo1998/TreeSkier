using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneUnloading : OnEnterUnit
{
    protected override void VariablesDefinition()
    {
    }
    protected override ControlOutput OnEnter(Flow arg)
    {
        // launch loading
        TravelSystem.Instance.SceneUnload();

        return OutputTrigger;
    }
}

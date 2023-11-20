using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneLoading : OnEnterUnit
{
    // Bolt syntax
    [DoNotSerialize]
    public ValueInput SceneToLoad;

    protected override void VariablesDefinition()
    {
        SceneToLoad = ValueInput<string>("Scene to load", string.Empty);
    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        string sceneToLoad = arg.GetValue<string>(SceneToLoad);

        // launch loading
        TravelSystem.Instance.SceneLoad(sceneToLoad);

        return OutputTrigger;
    }
}

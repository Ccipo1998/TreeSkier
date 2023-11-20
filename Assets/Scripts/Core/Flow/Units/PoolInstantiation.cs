using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolInstantiation : OnEnterUnit
{
    // Bolt syntax
    [DoNotSerialize]
    public ValueInput TargetScene;

    protected override void VariablesDefinition()
    {
        TargetScene = ValueInput<string>("Target scene of the pool", string.Empty);
    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        string sceneTarget = arg.GetValue<string>(TargetScene);

        AudioSystem.Instance.SetupAudioClipForScene(sceneTarget);

        // launch pool manager for the target scene
        PoolingSystem.Instance.SetupPoolManagersForScene(sceneTarget);

        TravelSystem.Instance.SetIsReadyToLoad(false);

        return OutputTrigger;
    }
}

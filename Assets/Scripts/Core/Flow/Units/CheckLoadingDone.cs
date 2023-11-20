using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckLoadingDone : OnUpdateUnit
{
    protected override void VariablesDefinition() { }

    protected override ControlOutput OnUpdate(Flow arg)
    {
        if (TravelSystem.Instance.IsLoadingDone())
        {
            // get scene loaded name
            string scenePath = BoltFlowSystem.Instance.GetFSMvariable<string>("SCENE_TO_LOAD");

            // launch flow system trigger
            BoltFlowSystem.Instance.TriggerFSMevent(BoltFlowSystem.Instance.FormatSceneNameFSMtrigger(scenePath));
        }

        return OutputTrigger;
    }
}

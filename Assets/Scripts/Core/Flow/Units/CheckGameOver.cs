using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckGameOver : OnUpdateUnit
{
    [DoNotSerialize]
    private ValueInput OnGameOverFlowEvent;

    protected override void VariablesDefinition()
    {
        OnGameOverFlowEvent = ValueInput<idContainer>("Game over flow event name", null);
    }

    protected override ControlOutput OnUpdate(Flow arg)
    {
        if (LevelSystem.Instance.IsGameOver())
        {
            string eventName = arg.GetValue<string>(OnGameOverFlowEvent);

            // game over
            BoltFlowSystem.Instance.TriggerFSMevent(eventName);
        }

        return OutputTrigger;
    }
}

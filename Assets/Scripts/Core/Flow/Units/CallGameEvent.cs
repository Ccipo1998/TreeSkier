using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CallGameEvent : OnEnterUnit
{
    [DoNotSerialize]
    public ValueInput GameEventToCall;

    protected override void VariablesDefinition()
    {
        GameEventToCall = ValueInput<GameEvent>("Game event to call", null);
    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        GameEvent evt = arg.GetValue<GameEvent>(GameEventToCall);
        evt?.Invoke();

        return OutputTrigger;
    }
}

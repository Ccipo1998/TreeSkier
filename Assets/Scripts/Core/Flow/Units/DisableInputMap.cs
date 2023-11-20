using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisableInputMap : OnEnterUnit
{
    [DoNotSerialize]
    public ValueInput IdInputProvider;

    protected override void VariablesDefinition()
    {
        IdInputProvider = ValueInput<idContainer>("Input provider id", null);
    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        idContainer provider = arg.GetValue<idContainer>(IdInputProvider);
        if (provider != null)
            PlayerController.Instance.DisableInputProvider(provider.Id);

        return OutputTrigger;
    }
}

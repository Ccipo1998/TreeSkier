using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowUI : OnEnterUnit
{
    // Bolt syntax
    [DoNotSerialize]
    public ValueInput UIviewController;

    protected override void VariablesDefinition()
    {
        UIviewController = ValueInput<GameObject>("UI view controller", null);
    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        ViewController view = arg.GetValue<ViewController>(UIviewController);

        if (view == null)
            return OutputTrigger;

        UISystem.Instance.ShowView(view);

        return OutputTrigger;
    }
}
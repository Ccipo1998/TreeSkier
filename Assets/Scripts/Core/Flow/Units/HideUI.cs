using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideUI : OnExitUnit
{
    // Bolt syntax
    [DoNotSerialize]
    public ValueInput UIviewController;
    [DoNotSerialize]
    public ValueInput prova;

    protected override void VariablesDefinition()
    {
        UIviewController = ValueInput<GameObject>("UI view controller", null);
    }

    protected override ControlOutput OnExit(Flow arg)
    {
        ViewController view = arg.GetValue<ViewController>(UIviewController);

        if (view == null)
            return OutputTrigger;

        UISystem.Instance.HideView(view);

        return OutputTrigger;
    }
}

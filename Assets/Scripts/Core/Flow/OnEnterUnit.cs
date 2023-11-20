using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class OnEnterUnit : Unit
{
    // Bolt syntax
    [DoNotSerialize]
    public ControlInput InputTrigger; // on enter event

    // Bolt syntax
    [DoNotSerialize]
    public ControlOutput OutputTrigger; // on exit event

    protected override void Definition()
    {
        InputTrigger = ControlInput(string.Empty, OnEnter);
        OutputTrigger = ControlOutput(string.Empty);

        VariablesDefinition();
    }

    protected abstract void VariablesDefinition();

    protected abstract ControlOutput OnEnter(Flow arg);
}

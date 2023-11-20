using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// this class represents the Boot Unit inside the GameFlow FSM
public class BootOnUpdate : OnUpdateUnit
{
    // Bolt syntax
    [DoNotSerialize]
    public ValueInput InitialSceneName;

    // Bolt syntax
    [DoNotSerialize]
    public ValueInput NextEventToTrigger;

    // flow control variables
    private bool _systemsStarted = false;
    private bool nextTriggerFired = false;

    // definition of input and output events
    protected override void VariablesDefinition()
    {
        InitialSceneName = ValueInput<string>("First scene to load", string.Empty);
        NextEventToTrigger = ValueInput<string>("Name of the event to fire at the end", string.Empty);
    }

    // template for input trigger
    protected override ControlOutput OnUpdate(Flow arg)
    {
        string sceneToLoad = arg.GetValue<string>(InitialSceneName);
        string nextEvent = arg.GetValue<string>(NextEventToTrigger);

        if (SystemsCoordinator.Instance != null && !_systemsStarted)
        {
            SystemsCoordinator.Instance.StartSystems();
            
            _systemsStarted = true;
        }

        if (SystemsCoordinator.Instance != null && SystemsCoordinator.Instance.AreAllSystemsReady() && !nextTriggerFired)
        {
            //BoltFlowSystem.Instance.SetFSMvariable("SCENE_TO_LOAD", sceneToLoad);
            BoltFlowSystem.Instance.TriggerFSMevent(nextEvent);
            
            nextTriggerFired = true;
        }

        return OutputTrigger;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoltFlowSystem : Singleton<BoltFlowSystem>, ISystem
{
    [SerializeField]
    private int _Priority;

    public int Priority { get => _Priority; }

    [SerializeField]
    private StateMachine _FSM;

    public void Setup()
    {
        // notify systems coordinator
        SystemsCoordinator.Instance.SystemReady(this);
    }

    // triggering a general event in the flow FSM
    public void TriggerFSMevent(string eventName)
    {
        _FSM.TriggerUnityEvent(eventName);
    }

    public string FormatSceneNameFSMtrigger(string sceneName)
    {
        return sceneName.ToUpper() + "_SCENE_LOADED";
    }

    // setting a variable in the flow FSM
    public void SetFSMvariable(string variableName, object variableValue)
    {
        // inside bolt -> we work only with App level variables (design choice)
        Variables.Application.Set(variableName, variableValue);
    }

    // getting a variable value in the flow FSM
    public T GetFSMvariable<T>(string variableName) // <T> to auto cast
    {
        // inside bolt -> we work only with App level variables (design choice)
        return Variables.Application.Get<T>(variableName);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Event/GameEvent", order = 1)]
public class GameEvent : ScriptableObject // base class to use to derivate events of various types
{
    private Action<GameEvent> _onEventTriggered; // for subscription

    public void Invoke()
    {
        _onEventTriggered?.Invoke(this); // this because if this event has parameters, we pass them
    }

    public void Subscribe(Action<GameEvent> cbk) // cbk (callback) is a method pointer
    {
        _onEventTriggered += cbk;
    }

    public void Unsubscribe(Action<GameEvent> cbk)
    {
        _onEventTriggered -= cbk;
    }

    // because scriptable objects are persistent, so we need to clear all the delegates at the end of the game
    public void Clear()
    {
        foreach (Delegate deleg in _onEventTriggered.GetInvocationList())
            _onEventTriggered -= (Action<GameEvent>)deleg;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsSystem : Singleton<GameEventsSystem>, ISystem
{
    [SerializeField]
    private int _Priority;
    public int Priority => _Priority;

    private HashSet<GameEvent> _HotEvents;

    public void Setup()
    {
        _HotEvents = new HashSet<GameEvent>();
        SystemsCoordinator.Instance.SystemReady(this);
    }

    public void AddHotEvent(GameEvent evt)
    {
        _HotEvents.Add(evt);
    }

    public void RemoveHotEvent(GameEvent evt)
    {
        _HotEvents.Remove(evt);
    }
}

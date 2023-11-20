using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemsCoordinator : Singleton<SystemsCoordinator>
{
    // list of systems
    private List<ISystem> _systems;
    // systems status (ready or not)
    private Dictionary<ISystem, bool> _systemsStatus;

    // the number of systems that have notified ready status
    private int _systemsReady;

    // store and setup all the systems
    public void StartSystems()
    {
        Application.targetFrameRate = 60;

        // initializations
        _systems = new List<ISystem>();
        _systemsStatus = new Dictionary<ISystem, bool>();
        _systemsReady = 0;

        // get starting systems
        List<GameObject> startObjects = FindObjectsOfType<GameObject>().ToList();
        foreach (GameObject obj in startObjects)
        {
            ISystem sys = obj.GetComponent<ISystem>();
            if (sys != null)
            {
                _systems.Add(sys);
                _systemsStatus.Add(sys, false);
            }
        }

        // ordering starting systems by priority
        _systems = _systems.OrderByDescending(s => s.Priority).ToList();

        // store and setup systems
        foreach (ISystem sys in _systems)
            sys.Setup();
    }

    // notify ready system
    public void SystemReady(ISystem sys)
    {
        if (!_systemsStatus.ContainsKey(sys))
        {
            Debug.Log("Systems setup error");

            return;
        }

        _systemsStatus[sys] = true;
        ++_systemsReady;

        //CheckAllSystemsReady();
    }

    // check if all the systems are ready (called by the flow system)
    public bool AreAllSystemsReady()
    {
        if (_systemsReady == _systemsStatus.Count)
            return true;

        return false;
    }
}

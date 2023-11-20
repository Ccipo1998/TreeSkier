using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] // to draw in editor
public class PoolManagerBinding // model
{
    public string SceneName;
    public List<PoolManager> PoolManagers;
}

public class PoolingSystem : Singleton<PoolingSystem>, ISystem
{
    [SerializeField]
    private int _Priority;

    public int Priority => _Priority;

    [SerializeField]
    private PoolingSystemData _PoolingData;

    // corrispondence scene <-> managers
    private Dictionary<string, List<PoolManager>> _sceneManagersDict;

    // id -> pool manager
    private Dictionary<string, PoolManager> _currentPoolManagerDict;

    private bool _isLoadingDone;
    private int _countManagerReady;
    public void Setup()
    {
        _countManagerReady = 0;
        _isLoadingDone = true;
        _sceneManagersDict = new Dictionary<string, List<PoolManager>>();
        _currentPoolManagerDict = new Dictionary<string, PoolManager>();

        foreach (PoolManagerBinding binding in _PoolingData.PoolManagerBindings)
            _sceneManagersDict.Add(binding.SceneName, new List<PoolManager>(binding.PoolManagers)); // new list because scriptable objects can be modified at runtime and the new changes remain

        SystemsCoordinator.Instance.SystemReady(this);
    }

    // setup the pool managers for a target scene
    public void SetupPoolManagersForScene(string sceneTarget) // called by the flow system
    {
        _isLoadingDone = false;
        _countManagerReady = 0;
        if (!_sceneManagersDict.ContainsKey(sceneTarget))
        {
            _isLoadingDone = true;
            return;
        }
       

        // to avoid setupping pools for more than one scene
        if (_currentPoolManagerDict.Count > 0)
            DestroyCurrentManagers();

        List<PoolManager> requestedManagers = _sceneManagersDict[sceneTarget];
        foreach (PoolManager manager in requestedManagers) // this manager is a prefab, we need to instantiate it
        {
            PoolManager currentManager = Instantiate(manager, gameObject.transform);
            _currentPoolManagerDict.Add(currentManager.Id, currentManager);
        }
        foreach (PoolManager manager in _currentPoolManagerDict.Values) // this manager is a prefab, we need to instantiate it
        {
            manager.Setup();
        }

    }
    public void FinishManagerSetup(string manager)
    {
        if (!_currentPoolManagerDict.ContainsKey(manager))
        {
            Debug.LogError("UNLOADED");
            return;
        }
        ++_countManagerReady;

        CheckAllManagerReady();
    }
    private void CheckAllManagerReady()
    {
        if (_countManagerReady == _currentPoolManagerDict.Count)
        {
            _isLoadingDone = true;
        }
    }
    public bool IsLoadingDone()
    {
        return _isLoadingDone;
    }

    // destroy all pool managers
    public void DestroyCurrentManagers()
    {
        foreach (string id in _currentPoolManagerDict.Keys)
        {
            Destroy(_currentPoolManagerDict[id].gameObject);
        }

        _currentPoolManagerDict.Clear();
    }

    public PoolManager GetPoolManager(idContainer id)
    {
        if (!_currentPoolManagerDict.ContainsKey(id.Id))
        {
            Debug.LogError("ERROR: Pool Manager not found with id: " + id.Id);
            return null;
        }

        return _currentPoolManagerDict[id.Id];
    }
}

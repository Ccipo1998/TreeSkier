using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>, ISystem
{
    [SerializeField]
    private List<InputProvider> _ProviderList;

    private Dictionary<string, InputProvider> _providerDictionary;

    [SerializeField]
    private int _Priority;
    public int Priority => _Priority;

    /*
    protected override void OnAwake() {
        _providerDictionary = new Dictionary<string, InputProvider>();
        foreach (InputProvider provider in _ProviderList) {
            _providerDictionary.Add(provider.Id.Id, provider);
        }
    }
    */

    public void Setup()
    {
        _providerDictionary = new Dictionary<string, InputProvider>();
        foreach (InputProvider provider in _ProviderList)
        {
            _providerDictionary.Add(provider.Id.Id, provider);
            // disabled as default
            provider.gameObject.SetActive(false);
        }

        // notify systems coordinator
        SystemsCoordinator.Instance.SystemReady(this);
    }

    public T GetInput<T>(string id) where T : InputProvider{
        if(!_providerDictionary.ContainsKey(id)) {
            return null;
        }
        return _providerDictionary[id] as T;
    }

    public bool EnableInputProvider(string id) {
        if (!_providerDictionary.ContainsKey(id)) {
            return false;
        }

        if (_providerDictionary[id] == null)
            return false;

        _providerDictionary[id].gameObject.SetActive(true);
        return true;
    }

    public bool DisableInputProvider(string id) {
        if (!_providerDictionary.ContainsKey(id)) {
            return false;
        }

        if (_providerDictionary[id] == null)
            return false;

        _providerDictionary[id].gameObject.SetActive(false);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UISystem : Singleton<UISystem>, ISystem
{
    [SerializeField]
    private int _Priority;
    public int Priority => _Priority;

    [SerializeField]
    private IdContainerGameEvent _ViewChangedStateEvent;
    public IdContainerGameEvent ViewChangedStateEvent => _ViewChangedStateEvent;

    // to store all the already instatiated UIs (id <-> view)
    private Dictionary<string, ViewController> _viewControllerDictionary;

    // where to spawn the new views
    [SerializeField]
    private GameObject _SpawnPoint;

    public void Setup()
    {
        _viewControllerDictionary = new Dictionary<string, ViewController>();
        SystemsCoordinator.Instance.SystemReady(this);
    }

    public ViewController ShowView(ViewController controller)
    {
        idContainer id = controller.IdContainer;
        if (_viewControllerDictionary.ContainsKey(id.Id))
        {
            Debug.LogWarning("Tentativo di instanziamento di una View già istanziata con id: " + id.Id);
            return _viewControllerDictionary[id.Id];
        }

        ViewController newController = Instantiate(controller, _SpawnPoint.transform);
        _viewControllerDictionary.Add(id.Id, newController);

        newController.Setup(id);
        newController.State = ViewController.ViewState.Showing;

        return newController;
    }

    public void HideView(ViewController controller)
    {
        idContainer id = controller.IdContainer;
        if (!_viewControllerDictionary.ContainsKey(id.Id))
        {
            Debug.LogError("Tentativo di distruzione di una View non istanziata con id: " + id.Id);
            return;
        }

        ViewController view = _viewControllerDictionary[id.Id];
        view.State = ViewController.ViewState.Hiding;
        StartCoroutine(WaitUntilViewHidden(view));
    }

    public void ImmediateHideView(ViewController controller)
    {
        idContainer id = controller.IdContainer;
        if (!_viewControllerDictionary.ContainsKey(id.Id))
        {
            Debug.LogError("Tentativo di distruzione di una View non istanziata con id: " + id.Id);
            return;
        }

        ViewController view = _viewControllerDictionary[id.Id];
        view.State = ViewController.ViewState.Hidden;
        _viewControllerDictionary.Remove(controller.IdContainer.Id);
        Destroy(controller.gameObject);
    }

    public void HideAllViews()
    {
        foreach (string id in _viewControllerDictionary.Keys)
        {
            ViewController view = _viewControllerDictionary[id];
            view.State = ViewController.ViewState.Hidden;
            Destroy(view.gameObject);
        }

        _viewControllerDictionary.Clear();
    }

    public ViewController GetViewController(idContainer id)
    {
        if (!_viewControllerDictionary.ContainsKey(id.Id))
            return null;

        return _viewControllerDictionary[id.Id];
    }

    private IEnumerator WaitUntilViewHidden(ViewController controller)
    {
        yield return new WaitUntil(() => { return controller.State == ViewController.ViewState.Hidden; });
        _viewControllerDictionary.Remove(controller.IdContainer.Id);
        Destroy(controller.gameObject);
    }
}

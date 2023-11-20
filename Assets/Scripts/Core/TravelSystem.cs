using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelSystem : Singleton<TravelSystem>, ISystem
{
    public delegate void TravelCompleteDelegate();
    public TravelCompleteDelegate _OnTravelComplete;

    //flag if travelSystem has to wait to change the scene
    public bool CanChangeScene;

    [SerializeField]
    private string _LoadingSceneName;

    private string _currentSceneName;
    private string _targetSceneName;

    // flag -> loading coroutine running
    private bool _isLoadingDone;
    private bool _isUnloadingDone;
    private bool _isReadyToLoad;

    [SerializeField]
    private int _Priority;

    public int Priority => _Priority;

    public void Setup()
    {
        // initializations
        CanChangeScene = true;
        _isLoadingDone = true;
        _isUnloadingDone = true;
        _isReadyToLoad = true;

        _currentSceneName = SceneManager.GetActiveScene().name;

        // notify systems coordinator
        SystemsCoordinator.Instance.SystemReady(this);
    }

    public void SceneLoad(string scenePath) {
        _isLoadingDone = false;

        StartCoroutine(Load(scenePath));
    }
    public void SceneUnload()
    {
        // the loading has been called
        _isLoadingDone = false;
        _isUnloadingDone = false;
        StartCoroutine(Unload());
    }

    public bool IsLoadingDone()
    {
        return _isLoadingDone;
    }
    public bool IsUnloadingDone()
    {
        return _isUnloadingDone;
    }

    public void SetIsReadyToLoad(bool isReadyToLoad)
    {
        _isReadyToLoad = isReadyToLoad;
    }
    private IEnumerator Unload()
    {
        AsyncOperation loading_load = SceneManager.LoadSceneAsync(_LoadingSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return loading_load.isDone; });

        AsyncOperation current = SceneManager.UnloadSceneAsync(_currentSceneName);
        yield return new WaitUntil(() => { return current.isDone; });

        _isUnloadingDone = true;
    }

    private IEnumerator Load(string scenePath) {
        _targetSceneName = scenePath;

        yield return new WaitUntil(() => { return _isReadyToLoad == true; });

        AsyncOperation target = SceneManager.LoadSceneAsync(_targetSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return target.isDone; });

        _currentSceneName = _targetSceneName;
        _targetSceneName = string.Empty;

        AsyncOperation loading_unload = SceneManager.UnloadSceneAsync(_LoadingSceneName);
        yield return new WaitUntil(() => { return loading_unload.isDone; });

        // the loading is done
        _isLoadingDone = true;

        _OnTravelComplete?.Invoke();
    }
}

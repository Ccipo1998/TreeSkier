using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private idContainer _Id;

    public string Id => _Id.Id; // getter

    // the objects in the pool
    [SerializeField]
    private PoolableObject _PoolableObjectPrefab;

    [SerializeField]
    private int _PoolSize;

    // the number of objects to instantiate per frame (to avoid freezing)
    [SerializeField]
    private int _ObjectsPerFrame;

    // list of poolable objects of the manager
    private Queue<PoolableObject> _poolableQueue;

    // setup the pool by instantiating the objects
    public void Setup()
    {
        StartCoroutine(SetupAsync());
    }

    private IEnumerator SetupAsync()
    {
        _poolableQueue = new Queue<PoolableObject>();

        // exit the coroutine if there is not a prefab to instantiate
        if (_PoolableObjectPrefab == null)
        {
            PoolingSystem.Instance.FinishManagerSetup(Id);

            yield break;
        }

        PoolableObject poolableInstantiated;
        for (int i = 1; i <= _PoolSize; ++i)
        {
            poolableInstantiated = Instantiate(_PoolableObjectPrefab, gameObject.transform); // children of the pool manager (same transform)
            _poolableQueue.Enqueue(poolableInstantiated);

            poolableInstantiated.gameObject.SetActive(false); // we disable the objects of the pool

            if (i % _ObjectsPerFrame == 0)
                yield return null;
        }
        PoolingSystem.Instance.FinishManagerSetup(Id);
    }

    public T RequestPooledObject<T>() where T : PoolableObject
    {
        if (_poolableQueue.Count == 0)
        {
            /*Debug.LogError("Request on empty queue for object: " + gameObject.name);

            return null;*/
            PoolableObject poolableInstantiated = Instantiate(_PoolableObjectPrefab, gameObject.transform);
            _poolableQueue.Enqueue(poolableInstantiated);
            poolableInstantiated.gameObject.SetActive(false); // we disable the objects of the pool
        }

        T requestedObject = _poolableQueue.Dequeue() as T;
        requestedObject.gameObject.SetActive(true);
        requestedObject.Setup();

        return requestedObject;
    }

    public void ReturnPooledObject(PoolableObject requestedObject)
    {
        requestedObject.Clear();
        requestedObject.gameObject.SetActive(false);

        _poolableQueue.Enqueue(requestedObject);
    }
}

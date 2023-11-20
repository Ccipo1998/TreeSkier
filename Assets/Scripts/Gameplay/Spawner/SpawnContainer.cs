using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnContainer : PoolableObject
{
    [SerializeField]
    private idContainer _PoolManagerId;
    [Header("Game Events")]
    [SerializeField]
    private SpawnContainerGameEvent _InScopeGameEvent;
    [SerializeField]
    private SpawnContainerGameEvent _OutOfScopeGameEvent;

    [SerializeField]
    private Rigidbody2D _MyRigidbody2D;

    private List<SlopeObject> _spawnedObjects;

    private bool _inScope;

    public override void Setup()
    {
        _spawnedObjects = new List<SlopeObject>();
        _inScope = false;
    }

    public override void Clear()
    {
        foreach (SlopeObject obj in _spawnedObjects)
            BackToPool(obj);
            

        _spawnedObjects.Clear();
    }

    public idContainer GetPoolManagerId()
    {
        return _PoolManagerId;
    }

    public void SetSpawnedObjects(List<SlopeObject> spawnedObjects)
    {
        _spawnedObjects = spawnedObjects;

        foreach (SlopeObject obj in _spawnedObjects)
            obj.transform.parent = transform;
    }

    public List<SlopeObject> GetSpawnedObjects()
    {
        return _spawnedObjects;
    }

    #region MOVEMENT

    protected void Update()
    {
        _MyRigidbody2D.transform.position += LevelSystem.Instance.GetVelocity() * Time.deltaTime;

        if (_inScope)
        {
            // check if the slope object is out of scope
            Vector3 outOfScope = LevelSystem.Instance.GetOutOfScopePoint();
            if (transform.position.y >= outOfScope.y)
            {
                // call out of scope event
                OutOfScope();
            }
        }
        else
        {
            Vector3 inScope = LevelSystem.Instance.GetInScopePoint();
            if (transform.position.y >= inScope.y)
            {
                _inScope = true;
                InScope();
            }
        }
    }

    #endregion MOVEMENT

    #region INTERNALS

    private void InScope()
    {
        // in scope game event
        _InScopeGameEvent.Container = this;
        _InScopeGameEvent?.Invoke();
    }

    private void OutOfScope()
    {
        // out of scope game event
        _OutOfScopeGameEvent.Container = this;
        _OutOfScopeGameEvent?.Invoke();
    }

    private void BackToPool(SlopeObject slopeObj)
    {
        idContainer idManager = slopeObj.GetPoolManagerId();
        PoolManager manager = PoolingSystem.Instance.GetPoolManager(idManager);

        if (manager != null)
        {
            // back to pool
            manager.ReturnPooledObject(slopeObj);

            slopeObj.transform.parent = manager.transform;
        }
            
    }

    #endregion INTERNALS
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlopeObject : PoolableObject
{
    [Header("Pool")]
    [SerializeField]
    protected idContainer _PoolManagerId;

    public override void Clear()
    {
        transform.position = PoolingSystem.Instance.GetPoolManager(_PoolManagerId).transform.position;
    }

    public idContainer GetPoolManagerId()
    {
        return _PoolManagerId;
    }

    // Set the position of the slope object
    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, .0f);
    }

    // Call a game event when hitting the skier
    protected abstract void Hit();

    // call a game event when exiting the hit
    protected abstract void ExitHit();

    #region COLLISION

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Hit();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitHit();
    }

    #endregion
}

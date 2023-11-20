using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    // to setup the object in the pool
    public virtual void Setup() { }

    // to return to base state the object in the pool
    public virtual void Clear() { }
}

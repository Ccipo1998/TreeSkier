using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISystem
{
    public int Priority { get; }

    public void Setup() { }
}

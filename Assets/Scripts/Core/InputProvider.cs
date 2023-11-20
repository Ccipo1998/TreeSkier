using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    public delegate void OnFloatDelegate(float value);
    public delegate void OnVoidDelegate();
    public delegate void OnVector2Delegate(Vector2 value);

    [Header("Input Provider")]
    [SerializeField]
    protected idContainer _Id;

    public idContainer Id => _Id;

    
}

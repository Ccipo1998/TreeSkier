using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeScriptableObject", menuName = "ScriptableObjects/TypeScriptableObject", order = 1)]
public class TypeScriptableObject : ScriptableObject
{
    public idContainer MyID;

    [Header("In order if more than one (S, M, L)")]
    public List<idContainer> IdPoolManagers;

    [Range(0.1f,0.9f)]
    public float Rarity;
}

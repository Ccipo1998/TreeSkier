using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdAudio", menuName = "ScriptableObjects/IdAudio", order = 1)]
public class IdAudio : ScriptableObject
{
    public idContainer Id;
    public AudioClip AudioClip;
    public bool IsMusic;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdAudioSystemData", menuName = "ScriptableObjects/IdAudioSystemData", order = 1)]
public class IdAudioSystemData : ScriptableObject
{
    public List<AudioListBinding> IdAudioBindings;
}

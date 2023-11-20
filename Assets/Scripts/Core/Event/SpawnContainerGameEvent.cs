using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnContainerGameEvent", menuName = "Event/SpawnContainerGameEvent", order = 1)]
public class SpawnContainerGameEvent : GameEvent
{
    public SpawnContainer Container;
}

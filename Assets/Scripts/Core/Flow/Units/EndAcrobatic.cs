using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndAcrobatic : OnExitUnit
{
    protected override void VariablesDefinition()
    {

    }

    protected override ControlOutput OnExit(Flow arg)
    {
        // check acrobatic result -> assign bonus (points) or malus (damage)
        LevelSystem.Instance.ApplyAcrobaticJumpResult();

        // despawn snowflakes
        LevelSystem.Instance.DespawnSnokflakes();

        return OutputTrigger;
    }
}

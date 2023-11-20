using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartAcrobatic : OnEnterUnit
{
    protected override void VariablesDefinition()
    {

    }

    protected override ControlOutput OnEnter(Flow arg)
    {
        // spawn snowflakes
        LevelSystem.Instance.SpawnSnowflakes();

        return OutputTrigger;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoolLoadingDone : OnUpdateUnit
{
    protected override void VariablesDefinition()
    {
    }

    protected override ControlOutput OnUpdate(Flow arg)
    {
        

        if (PoolingSystem.Instance.IsLoadingDone() && TravelSystem.Instance.IsUnloadingDone())
        {
            TravelSystem.Instance.SetIsReadyToLoad(true);
        }

        return OutputTrigger;
    }
}

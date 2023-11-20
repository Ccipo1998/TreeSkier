using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionViewController : ViewController
{
    public void Close()
    {
        UISystem.Instance.HideView(this);
    }

    protected override void OnHiding()
    {
        State = ViewState.Hidden;
    }
}

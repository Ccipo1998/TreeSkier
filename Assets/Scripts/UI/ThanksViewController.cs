using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewController;

public class ThanksViewController : ViewController
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

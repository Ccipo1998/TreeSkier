using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseViewController : ViewController
{
    [SerializeField]
    private GameEvent _OnRestartEvent;
    [SerializeField]
    private GameEvent _OnResumeEvent;

    public void Resume()
    {
        _OnResumeEvent?.Invoke();

        UISystem.Instance.HideView(this);
    }

    protected override void OnHiding()
    {
        State = ViewState.Hidden;
    }

    public void Restart()
    {
        _OnRestartEvent?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField]
    protected idContainer _IdContainer;
    public idContainer IdContainer => _IdContainer;

    // the possible states of the controller (showing and hiding for the animations)
    public enum ViewState { NONE, Showing, Hiding, Hidden, Shown };

    protected ViewState _State;
    public ViewState State
    {
        get { return _State; }

        set
        { 
            _State = value;

            switch (_State)
            {
                case ViewState.Showing:
                    OnShowing();
                    break;

                case ViewState.Hiding:
                    OnHiding();
                    break;

                case ViewState.Shown:
                    OnShown();
                    break;

                case ViewState.Hidden:
                    OnHidden();
                    break;

                default:
                    Debug.LogError("Incorrect view state in View Controller");
                    break;
            }

            UISystem.Instance.ViewChangedStateEvent.IdContainer = _IdContainer;
            UISystem.Instance.ViewChangedStateEvent.Invoke();
        }
    }

    public void Setup(idContainer id)
    {
        _IdContainer = id;
        OnSetup();
    }

    // 
    protected virtual void OnShown() { }
    protected virtual void OnHidden() { }
    protected virtual void OnShowing() { }
    protected virtual void OnHiding() { }
    protected virtual void OnSetup() { }
}

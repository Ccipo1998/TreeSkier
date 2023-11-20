using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuViewController : ViewController
{
    [SerializeField]
    private OptionViewController _OptionViewController;
    [SerializeField]
    private ThanksViewController _ThanksViewController;

    [SerializeField]
    private string _OnStartGameFlowEvent;

    [SerializeField]
    private string _StartGameSceneTarget;

    [SerializeField]
    private Animator _StartButtonAnimator;

    [SerializeField]
    private Button _OptionButton;
    [SerializeField]
    private Button _ThanksButton;

    private void StartGame()
    {
        // change flow state
        BoltFlowSystem.Instance.SetFSMvariable("SCENE_TO_LOAD", _StartGameSceneTarget);
        BoltFlowSystem.Instance.TriggerFSMevent(_OnStartGameFlowEvent);
    }

    public void StartButton()
    {
        // animation
        StartCoroutine(WaitForAnimationAndStart());

        // disabling raycast of main menu
        _OptionButton.interactable = false;
    }

    private IEnumerator WaitForAnimationAndStart()
    {
        // wait for animation to start
        yield return new WaitUntil(() => _StartButtonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= .0f);

        // wait for animation to finish
        yield return new WaitUntil(() => _StartButtonAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        UISystem.Instance.ImmediateHideView(this);

        StartGame();
    }

    public void OpenOptions()
    {
        if (_OptionViewController == null) // == null
            return;

        UISystem.Instance.ShowView(_OptionViewController);
    }

    public void OpenThanks()
    {
        if (_ThanksViewController == null)
            return;
        UISystem.Instance.ShowView(_ThanksViewController);
    }
}

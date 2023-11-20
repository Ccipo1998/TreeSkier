using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ViewController;

public class AnimatedViewController : ViewController
{
    [SerializeField]
    protected Animator _Animator;

    [SerializeField]
    private bool _SkipAnimation;

    protected override void OnHiding()
    {
        if (_SkipAnimation)
        {
            State = ViewState.Hidden;
            return;
        }

        StartCoroutine(WaitForAnimationToFinish());
    }

    protected override void OnShowing() { }

    private IEnumerator WaitForAnimationToFinish()
    {
        // wait for animation to finish
        yield return new WaitUntil(() => _Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);

        State = ViewState.Hidden;
    }

}

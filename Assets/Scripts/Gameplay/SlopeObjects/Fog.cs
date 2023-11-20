using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fog : SlopeObject
{
    [SerializeField]
    private Animator _MyAnimator;
    [SerializeField]
    private RawImage _MyImage;
    [SerializeField]
    private float _BaseDuration;

    private float _playTime;
    public override void Setup() {
        StartAnimation(_BaseDuration + LevelSystem.Instance.GetDifficulty());
    }
    public void StartAnimation(float time)
    {
        _playTime = time;
        _MyAnimator.SetBool("EndAnimation", false);
        _MyAnimator.SetBool("Start", true);
        StartCoroutine(TimerAnimation());
    }
    private IEnumerator TimerAnimation()
    {
        
        yield return new WaitForSecondsRealtime(_playTime);
        EndAnimation();
    }
    private void EndAnimation()
    {
        _MyAnimator.SetBool("EndAnimation", true);

    }
    
    protected override void Hit()
    {
    }

    protected override void ExitHit()
    {
    }
}

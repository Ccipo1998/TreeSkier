using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaygroundViewController : ViewController
{
    [SerializeField]
    private string _OnPauseFlowEvent;

    [Header("UI elements")]
    [SerializeField]
    private List<GameObject> _Hearts;

    [SerializeField]
    private TextMeshProUGUI _ScoreValue;

    [SerializeField]
    private RectTransform _SnowMask;

    [SerializeField]
    private GameObject _X2Timer;

    [SerializeField]
    private RectTransform _X2Mask;

    [SerializeField]
    private GameObject _JumpTimer;

    [SerializeField]
    private RectTransform _JumpTimerMask;

    private int _HeartIndex = 2;

    private float _snowHeight;
    private float _x2Height;
    private float _jumptimerWidth;

    protected override void OnSetup()
    {
        // take masks height or width
        _snowHeight = _SnowMask.rect.height;
        _x2Height = _X2Mask.rect.height;
        _jumptimerWidth = _JumpTimerMask.rect.width;


        // set snow mask to zero
        _SnowMask.sizeDelta = new Vector2(_SnowMask.sizeDelta.x, .0f);

        // disable X2
        _X2Timer.SetActive(false);
        // disable jump timer
        _JumpTimer.SetActive(false);
    }

    public void OpenPause()
    {
        BoltFlowSystem.Instance.TriggerFSMevent(_OnPauseFlowEvent);
    }

    public void HideHeart()
    {
        // disable
        _Hearts[_HeartIndex].SetActive(false);

        // update
        --_HeartIndex;
    }

    public void ShowHeart()
    {
        // show and update
        _Hearts[++_HeartIndex].SetActive(true);
    }

    public void SetPoints(int points)
    {
        _ScoreValue.text = points.ToString();
    }

    public void UpdateSnowMask(float snowballCharge)
    {
        _SnowMask.sizeDelta = new Vector2(_SnowMask.sizeDelta.x, snowballCharge * _snowHeight);
    }

    public void ShowX2Timer()
    {
        _X2Timer.SetActive(true);

        // reset mask height
        _X2Mask.sizeDelta = new Vector2(_SnowMask.sizeDelta.x, _x2Height);
    }

    public void HideX2Timer()
    {
        _X2Timer.SetActive(false);
    }

    public void UpdateX2Mask(float normalizedRemainingTime)
    {
        _X2Mask.sizeDelta = new Vector2(_SnowMask.sizeDelta.x, normalizedRemainingTime * _x2Height);
    }

    public void ShowJumpTimer()
    {
        _JumpTimer.SetActive(true);

        // reset mask width
        _JumpTimerMask.sizeDelta = new Vector2(_jumptimerWidth, _JumpTimerMask.sizeDelta.y);
    }

    public void HideJumpTimer()
    {
        _JumpTimer.SetActive(false);
    }

    public void StartJumpTimerMask(float duration)
    {
        StartCoroutine(JumpTimer(duration));
    }

    private IEnumerator JumpTimer(float duration)
    {
        float normalizedRemainingTime = 1.0f;

        while (normalizedRemainingTime >= .0f)
        {
            _JumpTimerMask.sizeDelta = new Vector2(normalizedRemainingTime * _jumptimerWidth, _JumpTimerMask.sizeDelta.y);

            normalizedRemainingTime -= Time.deltaTime / duration;

            yield return null;
        }
    }
}

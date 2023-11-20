using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X2 : SlopeObject
{
    [SerializeField]
    private SlopeObjectGameEvent _X2HitGameEvent;

    [Header("Data")]
    [SerializeField]
    private float _DurationInSeconds = 10.0f;

    public float GetDuration()
    {
        return _DurationInSeconds;
    }

    // Increase points' multiplier
    protected override void Hit()
    {
        // play sound effect
        //PlaySoundEffect();

        // play animation
        // TODO

        // launch a score multiplier event to level system
        //_Skier.DoublePoints(_DurationInSeconds);
        _X2HitGameEvent.SlopeObject = this;
        _X2HitGameEvent?.Invoke();
    }

    protected override void ExitHit() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : SlopeObject
{
    [SerializeField]
    private SlopeObjectGameEvent _HeartHitGameEvent;

    // Heal the skier
    protected override void Hit()
    {
        // play sound effect
        //PlaySoundEffect();

        // play animation
        // TODO

        // launch skier heal game event to level system
        //_Skier.Heal();
        _HeartHitGameEvent.SlopeObject = this;
        _HeartHitGameEvent?.Invoke();
    }

    protected override void ExitHit() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : SlopeObject
{
    [SerializeField]
    private SlopeObjectGameEvent _BombHitGameEvent;

    // Damage the skier
    protected override void Hit()
    {
        // play hit sound effect
        //PlaySoundEffect();

        // play animation
        // TODO

        // launch a skier damage event to level system
        //_Skier.Damage();
        _BombHitGameEvent.SlopeObject = this;
        _BombHitGameEvent?.Invoke();
    }

    protected override void ExitHit() { }
}

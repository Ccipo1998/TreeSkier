using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : SlopeObstacle
{
    [SerializeField]
    private SlopeObjectGameEvent _TreeHitGameEvent;

    // Damage the skier
    protected override void Hit()
    {
        // play sound effect
        //PlaySoundEffect();

        // play animation
        // TODO

        // launch skier damage game event to level system
        //_Skier.Damage();
        _TreeHitGameEvent.SlopeObject = this;
        _TreeHitGameEvent?.Invoke();
    }

    protected override void ExitHit() { }
}

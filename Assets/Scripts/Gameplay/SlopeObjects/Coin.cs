using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : SlopeObject
{
    [SerializeField]
    private SlopeObjectGameEvent _CoinHitGameEvent;

    [Header("Data")]
    [SerializeField]
    private uint _PointsGain = 100;

    public uint GetPointsGain()
    {
        return _PointsGain;
    }

    // Add points to the skier
    protected override void Hit()
    {
        // play sound effect
        //PlaySoundEffect();

        // play animation
        // TODO

        // launch score increase game event to level system
        //_Skier.AddPoints(_PointsGain);
        _CoinHitGameEvent.SlopeObject = this;
        _CoinHitGameEvent?.Invoke();
    }

    protected override void ExitHit() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : SlopeObject
{
    [SerializeField]
    private SlopeObjectGameEvent _OnRampGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _OffRampGameEvent;

    [SerializeField]
    private Transform _PointJump;

    [SerializeField]
    private GameObject _MyArrowUp;

    [Header("Data")]
    [SerializeField]
    private uint _PointsGain;
    [SerializeField]
    private int _SnowflakesNumber;

    public Transform GetPointJump() 
    { 
        return _PointJump;
    }

    public int GetSnoflakesNumber()
    {
        return _SnowflakesNumber;
    }

    public uint GetPointsGain()
    {
        return _PointsGain;
    }

    protected override void Hit()
    {
        // launch on ramp game event to level system
        _OnRampGameEvent.SlopeObject = this;
        _OnRampGameEvent?.Invoke();

        _MyArrowUp.SetActive(true);
    }

    protected override void ExitHit()
    {
        // launch off ramp game event to level system
        _OffRampGameEvent.SlopeObject = this;
        _OffRampGameEvent?.Invoke();

        _MyArrowUp.SetActive(false);
    }
}

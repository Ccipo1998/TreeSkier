using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowflake : PoolableObject
{
    [Header("Pool")]
    [SerializeField]
    protected idContainer _PoolManagerId;

    [Header("Effect")]
    [Tooltip("[0,1] range")]
    [SerializeField]
    private float _HitAlphaValue;

    private bool _hitted = false;

    public idContainer GetPoolManagerId()
    {
        return _PoolManagerId;
    }

    public override void Setup()
    {
        // start with not hitted
        _hitted = false;

        // setup visual effect
        Color color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1.0f);
    }

    public void Hit()
    {
        // make effect to visualize hit
        Color color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, _HitAlphaValue);

        _hitted = true;
    }

    public bool IsHit()
    {
        return _hitted;
    }
}

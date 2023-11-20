using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffFog : MonoBehaviour
{
    [SerializeField]
    private SlopeObjectGameEvent _OffTheFogGameEvent;
    [SerializeField]
    private RawImage _MyImage;
    [SerializeField]
    private Fog _MyFog;
    public void AnimationIsFinished()
    {
        _OffTheFogGameEvent.SlopeObject = _MyFog;
        _OffTheFogGameEvent?.Invoke();
        Color mycolor = _MyImage.color;
        _MyImage.color = new Color(mycolor.r, mycolor.g, mycolor.b, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrooler : MonoBehaviour
{
    [SerializeField] private RawImage _Img;
    [SerializeField] private float _X, _Y;

    void Update()
    {
        _Img.uvRect = new Rect(_Img.uvRect.position + new Vector2(_X, _Y) * Time.deltaTime, _Img.uvRect.size);
    }
}
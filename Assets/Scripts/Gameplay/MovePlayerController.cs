using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerController : MonoBehaviour
{
    [SerializeField]
    private idContainer _MoveInputProviderId;

    private MoveInputProvider _moveInputProvider;

    [SerializeField]
    private Skier _Skier;

    private void Awake()
    {
        _moveInputProvider = PlayerController.Instance.GetInput<MoveInputProvider>(_MoveInputProviderId.Id);
    }

    private void OnEnable()
    {
        _moveInputProvider.OnMove += SkierMove;
        _moveInputProvider.OnDisabling += ResetSkierMove;
        //_inputProvider.OnPause += PauseGame;
    }

    private void OnDisable()
    {
        _moveInputProvider.OnMove -= SkierMove;
        _moveInputProvider.OnDisabling -= ResetSkierMove;
        //_inputProvider.OnPause -= PauseGame;
    }

    private void SkierMove(float value)
    {
        _Skier.HorizontalMove(value);
    }

    private void ResetSkierMove()
    {
        _Skier.HorizontalMove(.0f);
    }
}

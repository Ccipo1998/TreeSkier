using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrobaticPlayerController : MonoBehaviour
{
    [SerializeField]
    private idContainer _AcrobaticInputProviderId;

    private AcrobaticInputProvider _acrobaticInputProvider;

    [SerializeField]
    private Camera _Camera;

    private bool _swiping = false;
    //private Vector2 _lastPosition = Vector2.zero;

    private void Awake()
    {
        _acrobaticInputProvider = PlayerController.Instance.GetInput<AcrobaticInputProvider>(_AcrobaticInputProviderId.Id);
    }

    private void OnEnable()
    {
        _acrobaticInputProvider.OnFirstTouch += StartSwipe;
        _acrobaticInputProvider.OnLastTouch += EndSwipe;
        _acrobaticInputProvider.OnTouching += Swiping;
        //_inputProvider.OnPause += PauseGame;
    }

    private void OnDisable()
    {
        _acrobaticInputProvider.OnFirstTouch -= StartSwipe;
        _acrobaticInputProvider.OnLastTouch -= EndSwipe;
        _acrobaticInputProvider.OnTouching -= Swiping;
        //_inputProvider.OnPause -= PauseGame;
    }

    private void StartSwipe()
    {
        _swiping = true;
    }

    private void EndSwipe()
    {
        _swiping = false;

        // update snowflakes hit status
        LevelSystem.Instance.UpdateSnowflakesStatus();
    }

    private void Swiping(Vector2 touchPosition)
    {
        if (!_swiping)
            return;

        // send current touch position to the level system
        Vector3 pos = _Camera.ScreenToWorldPoint(touchPosition);
        LevelSystem.Instance.CheckSnowflakesHit(pos);
    }
}

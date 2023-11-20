using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerController : MonoBehaviour
{
    [SerializeField]
    private idContainer _JumpInputProviderId;

    private JumpInputProvider _jumpInputProvider;

    [SerializeField]
    private Skier _Skier;

    // swipe data

    [Header("Swipe data")]
    [SerializeField]
    private float _MinSwipeLength;
    [SerializeField]
    private float _MaxSwipeTime;
    [SerializeField]
    private Vector2 _SwipeDirection;
    [SerializeField]
    [Tooltip("Degrees")]
    private float _MaxAcceptedAngle;

    // ... swipe data ...
    private Vector2 _startingPosition;
    private float _startingTime;
    private bool _swiping = false;

    private void Awake()
    {
        _jumpInputProvider = PlayerController.Instance.GetInput<JumpInputProvider>(_JumpInputProviderId.Id);
    }

    private void OnEnable()
    {
        _jumpInputProvider.OnJump += SkierAcrobaticJump;
        _jumpInputProvider.OnFirstTouch += StartSwipe;
        _jumpInputProvider.OnTouchingPosition += CheckSwipe;
        _jumpInputProvider.OnLastTouch += EndSwipe;
        //_inputProvider.OnPause += PauseGame;

        _swiping = false;
    }

    private void OnDisable()
    {
        _jumpInputProvider.OnJump -= SkierAcrobaticJump;
        _jumpInputProvider.OnFirstTouch -= StartSwipe;
        _jumpInputProvider.OnTouchingPosition -= CheckSwipe;
        _jumpInputProvider.OnLastTouch -= EndSwipe;
        //_inputProvider.OnPause -= PauseGame;
    }

    private void StartSwipe(Vector2 touchPosition)
    {
        // store starting position and time
        _startingPosition = touchPosition;
        _startingTime = Time.time;
        _swiping = true;
    }

    private void EndSwipe()
    {
        _swiping = false;
    }

    private void CheckSwipe(Vector2 touchPosition)
    {
        if (!_swiping)
            return;

        Vector2 swipe = touchPosition - _startingPosition;

        // check if the swipe meets the length and time requirements
        if (swipe.magnitude < _MinSwipeLength || (Time.time - _startingTime) > _MaxSwipeTime)
        {
            // not acrobatic
            SkierStandardJump();

            return;
        }

        // check if the swipe meets the direction requirement
        swipe = swipe.normalized;
        //float radians = Mathf.Atan2(swipe.x, swipe.y);
        float cosAngle = Vector3.Dot(swipe, _SwipeDirection.normalized);
        float sinAngle = Vector3.Cross(swipe, _SwipeDirection.normalized).magnitude;
        float angle = Mathf.Atan2(sinAngle, cosAngle);
        angle = Mathf.Rad2Deg * angle;
        if (angle > _MaxAcceptedAngle)
        {
            // not acrobatic
            SkierStandardJump();

            return;
        }

        // swipe accepted
        SkierAcrobaticJump();
    }

    private void SkierAcrobaticJump()
    {
        _Skier.AcrobaticJump();
    }

    private void SkierStandardJump()
    {
        _Skier.StandardJump();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpInputProvider : InputProvider
{
    #region Delegate
    public OnVoidDelegate OnJump;
    public OnVoidDelegate OnPause;
    public OnVector2Delegate OnFirstTouch;
    public OnVoidDelegate OnLastTouch;
    public OnVector2Delegate OnTouchingPosition;
    #endregion

    [Header("Input references")]
    [SerializeField]
    private InputActionReference _JumpButton;

    [SerializeField]
    private InputActionReference _ScreenTouch;

    [SerializeField]
    private InputActionReference _TouchPosition;

    [SerializeField]
    private InputActionReference _Pause;

    private void OnEnable()
    {
        _JumpButton.action.Enable();
        _ScreenTouch.action.Enable();
        _TouchPosition.action.Enable();
        _Pause.action.Enable();

        _JumpButton.action.performed += JumpPerformed;
        _ScreenTouch.action.started += TouchStarted;
        _ScreenTouch.action.canceled += TouchEnded;
        _TouchPosition.action.performed += Touching;
        _Pause.action.performed += PausePerformed;
    }

    private void OnDisable()
    {
        _JumpButton.action.Disable();
        _ScreenTouch.action.Disable();
        _TouchPosition.action.Disable();
        _Pause.action.Disable();

        _JumpButton.action.performed -= JumpPerformed;
        _ScreenTouch.action.started -= TouchStarted;
        _ScreenTouch.action.canceled -= TouchEnded;
        _TouchPosition.action.performed -= Touching;
        _Pause.action.performed -= PausePerformed;
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }

    private void Touching(InputAction.CallbackContext obj)
    {
        // get touch position from the other reference
        Vector2 touchPos = _TouchPosition.action.ReadValue<Vector2>();
        OnTouchingPosition?.Invoke(touchPos);
    }

    private void TouchStarted(InputAction.CallbackContext obj)
    {
        // get touch position from the other reference
        Vector2 touchPos = _TouchPosition.action.ReadValue<Vector2>();
        OnFirstTouch?.Invoke(touchPos);
    }

    private void TouchEnded(InputAction.CallbackContext obj)
    {
        OnLastTouch?.Invoke();
    }
}

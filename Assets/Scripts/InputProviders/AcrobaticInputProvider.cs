using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class AcrobaticInputProvider : InputProvider
{
    #region Delegate
    public OnVoidDelegate OnFirstTouch;
    public OnVoidDelegate OnLastTouch;
    public OnVector2Delegate OnTouching;
    public OnVoidDelegate OnPause;
    #endregion

    [Header("Action references")]
    [SerializeField]
    private InputActionReference _ScreenTouch;

    [SerializeField]
    private InputActionReference _TouchPosition;

    [SerializeField]
    private InputActionReference _Pause;

    private void OnEnable()
    {
        _ScreenTouch.action.Enable();
        _TouchPosition.action.Enable();
        _Pause.action.Enable();

        _ScreenTouch.action.started += TouchStarted;
        _ScreenTouch.action.canceled += TouchEnded;
        _TouchPosition.action.performed += Touching;
        _Pause.action.performed += PausePerformed;
    }

    private void OnDisable()
    {
        _ScreenTouch.action.Disable();
        _TouchPosition.action.Disable();
        _Pause.action.Disable();

        _ScreenTouch.action.started -= TouchStarted;
        _ScreenTouch.action.canceled -= TouchEnded;
        _TouchPosition.action.performed -= Touching;
        _Pause.action.performed -= PausePerformed;
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }

    private void TouchStarted(InputAction.CallbackContext obj)
    {
        OnFirstTouch?.Invoke();
    }

    private void Touching(InputAction.CallbackContext obj)
    {
        // get touch position from the other reference
        Vector2 touchPos = _TouchPosition.action.ReadValue<Vector2>();
        OnTouching?.Invoke(touchPos);
    }

    private void TouchEnded(InputAction.CallbackContext obj)
    {
        OnLastTouch?.Invoke();
    }
}

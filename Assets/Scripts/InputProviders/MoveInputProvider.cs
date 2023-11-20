using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveInputProvider : InputProvider
{
    #region Delegate
    public OnFloatDelegate OnMove;
    public OnVoidDelegate OnPause;
    public OnVoidDelegate OnDisabling;
    #endregion

    [Header("Input references")]
    [SerializeField]
    private InputActionReference _Move;

    [SerializeField]
    private InputActionReference _Pause;

    private void OnEnable()
    {
        _Move.action.Enable();
        _Pause.action.Enable();

        _Move.action.performed += MovePerformed;
        _Pause.action.performed += PausePerformed;
    }

    private void OnDisable()
    {
        Disabling();

        _Move.action.Disable();
        _Pause.action.Disable();

        _Move.action.performed -= MovePerformed;
        _Pause.action.performed -= PausePerformed;
    }

    private void MovePerformed(InputAction.CallbackContext obj)
    {
        float value = obj.action.ReadValue<float>();
        OnMove?.Invoke(value);
    }

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }

    private void Disabling()
    {
        OnDisabling?.Invoke();
    }
}

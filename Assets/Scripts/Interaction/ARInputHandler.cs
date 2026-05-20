using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ARInputHandler : MonoBehaviour
{
    public event Action<Vector2> OnPerformTap;

    private ARInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new ARInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.AR.Tap.performed += OnTapPerformed;
    }

    private void OnDisable()
    {
        _inputActions.AR.Tap.performed -= OnTapPerformed;
        _inputActions.Disable();
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPos = _inputActions.AR.TapPosition.ReadValue<Vector2>();
        OnPerformTap?.Invoke(screenPos);
    }
}
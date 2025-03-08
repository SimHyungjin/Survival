using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    PlayerController controller;

    private Vector2 curMoventInput;
    private Vector2 _mouseDelta;


    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        controller.Move(curMoventInput);
        controller.Exhaustion();
        controller.IsGrounded();
        if(controller._isDash)
            CharacterManager.Instance.Player.condition.stamina.Substract(controller.useDashStamina * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if(controller._canLook)
            controller.Look(_mouseDelta);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            curMoventInput = context.ReadValue<Vector2>();
        else if (context.phase == InputActionPhase.Canceled)
            curMoventInput = Vector2.zero;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            controller.Dash();
        else if (context.phase == InputActionPhase.Canceled)
            controller.DashEnd();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            controller.Jump();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            controller.OnAttack();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            controller.inventory?.Invoke();
            controller.ToggleCursor();
        }
    }
}

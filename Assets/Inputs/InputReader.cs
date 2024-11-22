using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;


[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls controls;
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;
    public Vector2 AimPosition { get; private set;}
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Enable();
    }
    private void OnDisable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
            Debug.Log(context.ReadValue<Vector2>());
    }
    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        
        }
        else if(context.canceled){
            PrimaryFireEvent?.Invoke(false);
          
        }

    }
    void IPlayerActions.OnAim(InputAction.CallbackContext context)
    {

        AimPosition = context.ReadValue<Vector2>();

    }
}

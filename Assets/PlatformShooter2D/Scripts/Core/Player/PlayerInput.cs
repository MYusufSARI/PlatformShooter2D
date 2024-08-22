using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header(" Data ")]
    private PlayerInputActions _playerInputActions;
    private InputAction _move;
    private InputAction _jump;


    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _move = _playerInputActions.Player.Move;
        _jump = _playerInputActions.Player.Jump;
    }


    private void OnEnable()
    {
        _playerInputActions.Enable();
    }


    private void OnDisable()
    {
        _playerInputActions.Disable();
    }


    private void Update()
    {
        
    }
}

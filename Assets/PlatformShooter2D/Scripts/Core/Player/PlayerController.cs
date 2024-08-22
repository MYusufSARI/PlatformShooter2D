using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header(" Events ")]
    public static Action OnJump;

    [Header(" Settings ")]
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = 0.2f;
    private float _timeInAir;
    private bool _doublejumpAvailable;

    [Header(" Elements ")]
    private Rigidbody2D _rigidBody;

    [Header(" Data ")]
    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Movement _movement;



    public void Awake()
    {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }


    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
    }


    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
    }


    private void Update()
    {
        GatherInput();

        Movement();

        HandleJump();

        HandleSpriteFlip();

        GravityDelay();
    }


    private void FixedUpdate()
    {
        ExtraGravity();
    }


    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }


    private bool CheckGrounded()
    {
        var isGrounded = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0f, _groundLayer);

        return isGrounded;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }


    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            _timeInAir += Time.deltaTime;
        }

        else
        {
            _timeInAir = 0f;
        }
    }


    private void ExtraGravity()
    {
        if (_timeInAir > _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
        }
    }

    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }


    private void Movement()
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }


    private void HandleJump()
    {
        if (!_frameInput.Jump)
        {
            return;
        }

        if (_doublejumpAvailable)
        {
            _doublejumpAvailable = false;
            OnJump?.Invoke();
        }

        else if (CheckGrounded())
        {
            _doublejumpAvailable = true;
            OnJump?.Invoke();
        }
    }


    private void ApplyJumpForce()
    {
        _rigidBody.velocity = Vector2.zero;

        _timeInAir = 0f;

        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }


    private void HandleSpriteFlip()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}

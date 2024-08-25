using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput => _frameInput.Move;

    public static PlayerController Instance;

    [Header(" Events ")]
    public static Action OnJump;
    public static Action OnJetpack;

    [Header(" Settings ")]

    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;

    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = 0.2f;
    [SerializeField] private float _coyoteTime = 0.5f;
    [SerializeField] private float _jetpackTime = 0.6f;
    [SerializeField] private float _jetpackStrength = 11f;
    [SerializeField] private float _maxFallSpeedVelocity = -20f;

    private float _timeInAir, _coyoteTimer;
    private bool _doublejumpAvailable;
    private Coroutine _jetpackCoroutine;

    [Header(" Elements ")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private TrailRenderer _jetpackTrailRenderer;

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

        OnJetpack += StartJetpack;
    }


    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;

        OnJetpack -= StartJetpack;
    }


    private void Update()
    {
        GatherInput();

        Movement();

        CoyoteTimer();

        HandleJump();

        HandleSpriteFlip();

        GravityDelay();

        Jetpack();
    }


    private void FixedUpdate()
    {
        ExtraGravity();
    }


    private void OnDestroy()
    {
        Fade fade = FindFirstObjectByType<Fade>();

        fade?.FadeInAndOut();
    }


    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }


    public bool CheckGrounded()
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

            if (_rigidBody.velocity.y < _maxFallSpeedVelocity)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _maxFallSpeedVelocity);
            }
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

        if (CheckGrounded())
        {
            OnJump?.Invoke();
        }

        else if (_coyoteTimer > 0f)
        {
            OnJump?.Invoke();
        }

        else if (_doublejumpAvailable)
        {
            _doublejumpAvailable = false;
            OnJump?.Invoke();
        }
    }


    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doublejumpAvailable = true;
        }

        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }


    private void ApplyJumpForce()
    {
        _rigidBody.velocity = Vector2.zero;

        _timeInAir = 0f;

        _coyoteTimer = 0f;

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


    private void Jetpack()
    {
        if (!_frameInput.Jetpack || _jetpackCoroutine != null)
            return;

        OnJetpack?.Invoke();
    }


    private void StartJetpack()
    {
        _jetpackTrailRenderer.emitting = true;

        _jetpackCoroutine = StartCoroutine(JetpackRoutine());
    }


    private IEnumerator JetpackRoutine()
    {
        var jetTime = 0f;

        while (jetTime < _jetpackTime)
        {
            jetTime += Time.deltaTime;

            _rigidBody.velocity = Vector2.up * _jetpackStrength;

            yield return null;
        }

        _jetpackTrailRenderer.emitting = false;
        _jetpackCoroutine = null;
    }
}

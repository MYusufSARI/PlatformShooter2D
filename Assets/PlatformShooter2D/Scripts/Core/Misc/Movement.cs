using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header(" Settings ")]
    private float _moveSpeed = 10f;
    private float _moveX;

    [Header(" Elements ")]
    private Rigidbody2D _rigidBody;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        Move();
    }


    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }


    private void Move()
    {
        var movement = new Vector2(_moveX * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = movement;
    }
}

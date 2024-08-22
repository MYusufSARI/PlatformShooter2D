using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    [Header(" Elements ")]
    private Vector2 _fireDirection;
    private Rigidbody2D _rigidBody;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    public void Initialize(Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }


    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        Destroy(this.gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 20f;
    [SerializeField]

    [Header(" Elements ")]
    private Vector2 _fireDirection;
    private Rigidbody2D _rigidBody;

    [Header(" Data ")]
    private Gun _gun;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }


    public void Initialize(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;

        transform.position = bulletSpawnPos;

        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);

        Knockback knockback = other.gameObject.GetComponent<Knockback>();
        knockback?.GetKnockedBack(PlayerController.Instance.transform.position, _knockbackThrust);

        Flash flash = other.gameObject.GetComponent<Flash>();
        flash?.StartFlash();

        _gun.ReleaseBulletFromPool(this);
    }
}
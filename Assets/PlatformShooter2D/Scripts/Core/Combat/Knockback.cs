using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [Header(" Events ")]
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    [Header(" Elements ")]
    private Rigidbody2D _rigidBody;

    [Header(" Settings ")]
    [SerializeField] private float _knockbackTime = 0.2f;
    private Vector3 _hitDirection;
    private float _knockThrust;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackForce;
        OnKnockbackEnd += StopKnockRoutine;
    }


    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKnockRoutine;
    }


    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust)
    {
        Debug.Log("Knocked Back!");

        _hitDirection = hitDirection;
        _knockThrust = knockbackThrust;

        OnKnockbackStart?.Invoke();
    }


    private void ApplyKnockbackForce()
    {
        var difference = (transform.position - _hitDirection).normalized * _knockThrust * _rigidBody.mass;

        _rigidBody.AddForce(difference, ForceMode2D.Impulse);

        StartCoroutine(KnockRoutine());
    }


    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);

        OnKnockbackEnd?.Invoke();
    }


    private void StopKnockRoutine()
    {
        _rigidBody.velocity = Vector2.zero;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grenade : MonoBehaviour
{
    public Action OnExplode;

    [Header(" Elements ")]
    [SerializeField] private GameObject _explodeVFX;
    private Rigidbody2D _rigidBody;
    private CinemachineImpulseSource _impulseSource;
    private Camera _mainCamera;

    [Header(" Settings ")]
    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private float _torqueAmount = 2f;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _mainCamera = Camera.main;
    }


    private void OnEnable()
    {
        OnExplode += Explosion;
        OnExplode += GrenadeScreenShake;
    }


    private void OnDisable()
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
    }


    private void Start()
    {
        LaunchGrenade();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            OnExplode?.Invoke();
        }
    }


    private void LaunchGrenade()
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - (Vector2)transform.position).normalized;

        _rigidBody.AddForce(directionToMouse * _launchForce, ForceMode2D.Impulse);
        _rigidBody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
    }


    private void Explosion()
    {
        Instantiate(_explodeVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    private void GrenadeScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }
}

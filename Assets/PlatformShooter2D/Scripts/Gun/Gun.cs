using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [Header(" Elements ")]
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    private Camera mainCamera;

    [Header(" Settings ")]
    [SerializeField] private float _gunFireCD = 0.5f;
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    [Header(" Events ")]
    public static Action OnShoot;



    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void Update()
    {
        Shoot();

        RotateCamera();
    }


    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
    }


    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
    }


    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            OnShoot?.Invoke();
        }
    }


    private void ShootProjectile()
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);

        newBullet.Initialize(_bulletSpawnPoint.position, _mousePos);
    }


    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }


    private void RotateCamera()
    {
        _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}

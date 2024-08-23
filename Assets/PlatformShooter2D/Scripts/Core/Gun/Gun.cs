using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;

public class Gun : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _muzzleFlash;

    private Camera mainCamera;
    private Animator _animator;
    private CinemachineImpulseSource _cmImpulseSource;
    private Coroutine _muzzleFlashRoutine;

    [Header(" Settings ")]
    [SerializeField] private float _gunFireCD = 0.5f;
    [SerializeField] private float _muzzleFlashTime = 0.05f;

    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    [Header(" Pool ")]
    private ObjectPool<Bullet> _bulletPool;

    [Header(" ReadOnly ")]
    private static readonly int FIRE_HASH = Animator.StringToHash("A_Fire");

    [Header(" Events ")]
    public static Action OnShoot;



    private void Awake()
    {
        _cmImpulseSource = GetComponent<CinemachineImpulseSource>();

        _animator = GetComponent<Animator>();
    }


    private void Start()
    {
        CreateBulletPool();

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
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
    }


    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
    }


    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }


    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bulletPrefab);
        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
        }, bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet);
        }, false, 20, 40);
        // 20 = Default size of the pool that will be created
        // 40 = Max size of the pool that could be created
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
        Bullet newBullet = _bulletPool.Get();

        newBullet.Initialize(this, _bulletSpawnPoint.position, _mousePos);
    }


    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }


    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }


    private void GunScreenShake()
    {
        _cmImpulseSource.GenerateImpulse();
    }


    private void RotateCamera()
    {
        _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }


    private void MuzzleFlash()
    {
        if (_muzzleFlashRoutine != null)
        {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }


    private IEnumerator MuzzleFlashRoutine()
    {
        _muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(_muzzleFlashTime);

        _muzzleFlash.SetActive(false);
    }
}

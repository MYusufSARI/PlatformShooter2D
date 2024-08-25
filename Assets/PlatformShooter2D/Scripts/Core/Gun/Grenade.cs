using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grenade : MonoBehaviour
{
    public Action OnExplode;
    public Action OnBeep;

    [Header(" Elements ")]
    [SerializeField] private GameObject _explodeVFX;
    [SerializeField] private GameObject _grenadeLight;
    [SerializeField] private LayerMask _enemyLayerMask;

    private Rigidbody2D _rigidBody;
    private CinemachineImpulseSource _impulseSource;
    private Camera _mainCamera;

    [Header(" Settings ")]
    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private float _torqueAmount = 2f;
    [SerializeField] private float _explosionRadius = 3.5f;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private float _lightBlinkTime = 0.15f;
    [SerializeField] private int _totalBlinks = 3;
    [SerializeField] private int _explodeTime = 3;

    private int _currentBlink;



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
        OnExplode += DamageNearby;
        OnExplode += AudioManager.Instance.Grenade_OnExplode;

        OnBeep += BlinkLight;
        OnBeep += AudioManager.Instance.Grenade_OnBeep;
    }


    private void OnDisable()
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
        OnExplode -= DamageNearby;
        OnExplode -= AudioManager.Instance.Grenade_OnExplode;

        OnBeep -= BlinkLight;
        OnBeep -= AudioManager.Instance.Grenade_OnBeep;
    }


    private void Start()
    {
        LaunchGrenade();

        StartCoroutine(CountdownExplodeRoutine());
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


    private void DamageNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);

        foreach (Collider2D hit in hits)
        {
            Health health = hit.GetComponent<Health>();

            health?.TakeDamage(_damageAmount);
        }
    }


    private IEnumerator CountdownExplodeRoutine()
    {
        while (_currentBlink < _totalBlinks)
        {
            yield return new WaitForSeconds(_explodeTime / _totalBlinks);

            OnBeep?.Invoke();

            yield return new WaitForSeconds(_lightBlinkTime);
            _grenadeLight.SetActive(false);
        }

        OnExplode?.Invoke();
    }


    private void BlinkLight()
    {
        _grenadeLight.SetActive(true);

        _currentBlink++;
    }
}

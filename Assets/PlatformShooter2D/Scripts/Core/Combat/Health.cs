using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;

    [Header(" Settings ")]
    [SerializeField] private int _startingHealth = 3;
    private int _currentHealth;

    [Header(" Events ")]
    public static Action OnDeath;



    private void Start()
    {
        ResetHealth();
    }


    private void OnEnable()
    {
        OnDeath += SpawnDeathSplatterPrefab;
        OnDeath += SpawnDeathVFX;
    }


    private void OnDisable()
    {
        OnDeath -= SpawnDeathSplatterPrefab;
        OnDeath -= SpawnDeathVFX;
    }


    public void ResetHealth()
    {
        _currentHealth = _startingHealth;
    }


    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();

            Destroy(gameObject);
        }
    }


    private void SpawnDeathSplatterPrefab()
    {
        GameObject newSplatterPrefab = Instantiate(_splatterPrefab, transform.position, transform.rotation);

        SpriteRenderer deathSplatterSpriteRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();

        ColorChanger colorChanger = GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;

        deathSplatterSpriteRenderer.color = currentColor;
    }


    private void SpawnDeathVFX()
    {
        GameObject deathVFX = Instantiate(_deathVFX, transform.position, transform.rotation);

        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;

        ColorChanger colorChanger = GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;

        ps.startColor = currentColor;
    }
}

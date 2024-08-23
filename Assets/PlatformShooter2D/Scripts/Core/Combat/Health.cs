using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFX;

    [Header(" Elements ")]
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;

    [Header(" Settings ")]
    [SerializeField] private int _startingHealth = 3;
    private int _currentHealth;

    [Header(" Events ")]
    public static Action<Health> OnDeath;



    private void Start()
    {
        ResetHealth();
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
            OnDeath?.Invoke(this);

            Destroy(gameObject);
        }
    }
}

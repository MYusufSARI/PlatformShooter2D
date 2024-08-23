using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    [Header(" Events ")]
    private static Action OnDiscoBallHit;

    [Header(" Settings ")]
    [SerializeField] private float _discoBallPartyTime;
    [SerializeField] private float _discoGlobalLightIntensity = 0.2f;

    private float _defaultGlobalLightIntensity;

    [Header(" Elements ")]
    [SerializeField] private Light2D _globalLight;

    [Header(" Data ")]
    private ColorSpotlight[] _allSpotlights;
    private Coroutine _discoCoroutine;



    private void Awake()
    {
        _defaultGlobalLightIntensity = _globalLight.intensity;
    }


    private void Start()
    {
        _allSpotlights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }


    private void OnEnable()
    {
        OnDiscoBallHit += DimTheLights;
    }


    private void OnDisable()
    {
        OnDiscoBallHit -= DimTheLights;
    }


    public void DiscoBallParty()
    {
        if (_discoCoroutine != null)
        {
            return;
        }

        OnDiscoBallHit?.Invoke();
    }


    private void DimTheLights()
    {
        foreach (ColorSpotlight colorSpotLight in _allSpotlights)
        {
            StartCoroutine(colorSpotLight.ColorSpotlightDiscoParty(_discoBallPartyTime));
        }

        _discoCoroutine = StartCoroutine(GlobalLightResetRoutine());

    }


    private IEnumerator GlobalLightResetRoutine()
    {
        _globalLight.intensity = _discoGlobalLightIntensity;

        yield return new WaitForSeconds(_discoBallPartyTime);

        _globalLight.intensity = _defaultGlobalLightIntensity;

        _discoCoroutine = null;
    }
}

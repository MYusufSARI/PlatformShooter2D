using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Fade : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;

    private Image _image;
    private CinemachineVirtualCamera _virtualCam;

    [Header(" Settings ")]
    [SerializeField] private float _fadeTime = 1.5f;



    private void Awake()
    {
        _image = GetComponent<Image>();
        _virtualCam = FindFirstObjectByType<CinemachineVirtualCamera>();
    }


    public void FadeInAndOut()
    {
        StartCoroutine(FadeIn());
    }


    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeRoutine(1f));

        Respawn();

        StartCoroutine(FadeRoutine(0f));
    }


    private IEnumerator FadeRoutine(float targetAlpha)
    {
        var elapsedTime = 0f;
        var startValue = _image.color.a;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;

            var newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeTime);

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);

            yield return null;
        }

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, targetAlpha);
    }


    private void Respawn()
    {
        var player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity).transform;

        _virtualCam.Follow = player;
    }
}

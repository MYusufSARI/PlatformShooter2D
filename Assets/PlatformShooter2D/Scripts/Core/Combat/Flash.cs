using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    private SpriteRenderer[] _spriteRenderers;

    [Header(" Settings ")]
    [SerializeField] private float _flashTime = 0.1f;



    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }


    public void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }


    private IEnumerator FlashRoutine()
    {
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _whiteFlashMaterial;

            sr.color = Color.white;
        }

        yield return new WaitForSeconds(_flashTime);

        SetDefaultMaterial();
    }


    private void SetDefaultMaterial()
    {
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _defaultMaterial;
        }
    }
}

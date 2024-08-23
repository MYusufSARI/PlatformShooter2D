using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpotlight : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject _spotlightHead;

    [Header(" Settings ")]
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _maxRotation = 45f;

    private float _currentRotation;



    private void Start()
    {
        RandomStartingRotation();
    }


    private void Update()
    {
        RotateHead();
    }


    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;

        var z = Mathf.PingPong(_currentRotation, _maxRotation);

        _spotlightHead.transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }


    private void RandomStartingRotation()
    {
        var randomStartingZ = Random.Range(-_maxRotation, _maxRotation);

        _spotlightHead.transform.localRotation = Quaternion.Euler(0f, 0f, randomStartingZ);

        _currentRotation = randomStartingZ + _maxRotation;
    }
}

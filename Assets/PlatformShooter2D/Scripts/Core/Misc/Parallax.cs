using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header(" Elements ")]
    private Camera _mainCamera;

    [Header(" Settings ")]
    [SerializeField] private float _parallaxOffset = 0.9f;

    private Vector2 _startPos;
    private Vector2 _travel => (Vector2)_mainCamera.transform.position - _startPos;



    private void Awake()
    {
        _mainCamera = Camera.main;
    }


    private void Start()
    {
        _startPos = transform.position;
    }


    private void FixedUpdate()
    {
        Vector2 newPosition = _startPos + new Vector2(_travel.x * _parallaxOffset, 0f);

        transform.position = new Vector2(newPosition.x, transform.position.y); ;
    }
}

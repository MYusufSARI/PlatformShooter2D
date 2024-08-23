using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Color[] _colors;
    [SerializeField] private SpriteRenderer _fillSpriteRenderer;



    public void SetColor(Color color)
    {
        _fillSpriteRenderer.color = color;
    }
}

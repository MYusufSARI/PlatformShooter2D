using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private ParticleSystem _moveDustVFX;



    private void Update()
    {
        DetectMoveDust();
    }


    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }

        else
        {
            if (_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        }
    }
}

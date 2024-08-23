using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private ParticleSystem _poofDustVFX;
    [SerializeField] private Transform _characterSpriteTransform;
    [SerializeField] private Transform _cowboyHatTransform;

    [Header(" Settings ")]
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _cowboyHatModifier = 2f;



    private void Update()
    {
        DetectMoveDust();

        ApplyTilt();
    }


    private void OnEnable()
    {
        PlayerController.OnJump += PlayPoofDustVFX;
    }


    private void OnDisable()
    {
        PlayerController.OnJump -= PlayPoofDustVFX;
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


    private void PlayPoofDustVFX()
    {
        _poofDustVFX.Play();
    }


    private void ApplyTilt()
    {
        float targetAngle;

        if (PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = _tiltAngle;
        }

        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle = -_tiltAngle;
        }

        else
        {
            targetAngle = 0f;
        }

        //Player Sprite
        Quaternion currentCharacterRotation = _characterSpriteTransform.rotation;
        Quaternion targetCharacterRotation =
            Quaternion.Euler(currentCharacterRotation.eulerAngles.x, currentCharacterRotation.eulerAngles.y, targetAngle);

        _characterSpriteTransform.rotation =
            Quaternion.Lerp(currentCharacterRotation, targetCharacterRotation, _tiltSpeed * Time.deltaTime); ;

        //Cowboy Hat Sprite
        Quaternion currentHatRotation = _cowboyHatTransform.rotation;
        Quaternion targetHatRotation =
            Quaternion.Euler(currentHatRotation.eulerAngles.x, currentHatRotation.eulerAngles.y, -targetAngle / _cowboyHatModifier);

        _cowboyHatTransform.rotation =
            Quaternion.Lerp(currentHatRotation, targetHatRotation, _tiltSpeed * _cowboyHatModifier * Time.deltaTime); ;
    }
}

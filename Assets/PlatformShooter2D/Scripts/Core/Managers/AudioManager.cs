using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header(" Data ")]
    [SerializeField] private SoundsCollectionsSO _soundsColletionsSO;

    [Header(" Settings ")]
    [Range(0f, 2f)]
    [SerializeField] private float _masterVolume = 1f;

    [Header(" Elements ")]
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;



    private void Start()
    {
        FightMusic();
    }


    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit += DiscoBallMusic;
    }


    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit -= DiscoBallMusic;
    }


    private void PlayRandomSound(SoundSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            SoundSO soundSO = sounds[Random.Range(0, sounds.Length)];

            SoundToPlay(soundSO);
        }
    }


    private void SoundToPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;
        var pitch = soundSO.Pitch;
        var volume = soundSO.Volume * _masterVolume;
        bool loop = soundSO.Loop;
        AudioMixerGroup audioMixerGroup;

        if (soundSO.RandomizePitch)
        {
            var randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);

            pitch = soundSO.Pitch + randomPitchModifier;
        }

        switch (soundSO.AudioType)
        {
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;

            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }


    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!loop)
            Destroy(soundObject, clip.length);

        if(audioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }

            _currentMusic = audioSource;
        }
    }


    private void Gun_OnShoot()
    {
        PlayRandomSound(_soundsColletionsSO.GunShoot);
    }


    private void PlayerController_OnJump()
    {
        PlayRandomSound(_soundsColletionsSO.Jump);
    }


    private void Health_OnDeath(Health health)
    {
        PlayRandomSound(_soundsColletionsSO.Splat);
    }


    private void FightMusic()
    {
        PlayRandomSound(_soundsColletionsSO.FightMusic);
    }


    private void DiscoBallMusic()
    {
        PlayRandomSound(_soundsColletionsSO.DiscoParty);

        var soundLenght = _soundsColletionsSO.DiscoParty[0].Clip.length;

        Invoke("FightMusic", soundLenght);
    }
}

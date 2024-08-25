using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header(" Data ")]
    [SerializeField] private SoundsCollectionsSO _soundsColletionsSO;

    [Header(" Settings ")]
    [Range(0f, 2f)]
    [SerializeField] private float _masterVolume = 1f;

    [Header(" Elements ")]
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;


    #region Unity Methods

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void Start()
    {
        FightMusic();
    }


    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        Gun.OnGrenadeShoot += Gun_OnGrenadeShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        PlayerController.OnJetpack += PlayerController_OnJetpack;
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit += DiscoBallMusic;
    }


    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        Gun.OnGrenadeShoot -= Gun_OnGrenadeShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        PlayerController.OnJetpack -= PlayerController_OnJetpack;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit -= DiscoBallMusic;
    }

    #endregion

    #region Sound Methods
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

        pitch = RandomizePitch(soundSO, pitch);
        audioMixerGroup = DetermineAudioMixerGroup(soundSO);

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }


    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO)
    {
        AudioMixerGroup audioMixerGroup;
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

        return audioMixerGroup;
    }


    private static float RandomizePitch(SoundSO soundSO, float pitch)
    {
        if (soundSO.RandomizePitch)
        {
            var randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);

            pitch = soundSO.Pitch + randomPitchModifier;
        }

        return pitch;
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

        DetermineMusic(audioMixerGroup, audioSource);
    }


    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
    {
        if (audioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }

            _currentMusic = audioSource;
        }
    }
    #endregion

    #region SFX
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


    private void PlayerController_OnJetpack()
    {
        PlayRandomSound(_soundsColletionsSO.Jetpack);
    }


    public void Grenade_OnBeep()
    {
        PlayRandomSound(_soundsColletionsSO.GrenadeBeep);
    }


    public void Grenade_OnExplode()
    {
        PlayRandomSound(_soundsColletionsSO.GrenadeExplode);
    }


    private void Gun_OnGrenadeShoot()
    {
        PlayRandomSound(_soundsColletionsSO.GrenadeShoot);
    }


    public void Enemy_OnPlayerHit()
    {
        PlayRandomSound(_soundsColletionsSO.PlayerHit);
    }

    #endregion

    #region Music
    private void FightMusic()
    {
        PlayRandomSound(_soundsColletionsSO.FightMusic);
    }


    private void DiscoBallMusic()
    {
        PlayRandomSound(_soundsColletionsSO.DiscoParty);

        var soundLength = _soundsColletionsSO.DiscoParty[0].Clip.length;

        UtilsClass.RunAfterDelay(this, soundLength, FightMusic);
    }
    #endregion
}

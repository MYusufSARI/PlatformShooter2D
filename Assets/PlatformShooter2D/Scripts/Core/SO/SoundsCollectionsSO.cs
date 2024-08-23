using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundsCollectionsSO : ScriptableObject
{
    [Header(" Music ")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoParty;


    [Header(" SFX ")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
}

/*
    SoundEffectVariation.cs
    Randomizes common sounds by choosing through several in a loop.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectVariation : MonoBehaviour
{
    // public AudioClip[] clipArray;

    [HideInInspector] public AudioSource effectSource;

    // private int clipIndex;

    // public float pitchMin;
    // public float pitchMax;
    // public float volumeMin;
    // public float volumeMax;

    [HideInInspector] public float basePitch;
    [HideInInspector] public float baseVolume;

    public float pitchVariation;
    public float volumeVariation;


    void Start()
    {
        effectSource = GetComponent<AudioSource>();

        basePitch = effectSource.pitch;
        baseVolume = effectSource.volume;
    }


    public void PlayVaryPitch()
    {
        VaryPitchVolume();

        effectSource.Play();
    }


    void VaryPitchVolume()
    {
        // Randomize pitch and volume
        effectSource.pitch = Random.Range(basePitch - pitchVariation, basePitch + pitchVariation);
        effectSource.volume = Random.Range(baseVolume - volumeVariation, baseVolume + volumeVariation);
    }


    // public void PlayRandom()
    // {
    //     VaryPitchVolume();

    //     clipIndex = RepeatCheck(clipIndex, clipArray.Length);
    //     effectSource.Play(OneShot(clipArray[clipIndex]));
    // }


    // int RepeatCheck(int previousIndex, int range)
    // {
    //     int index = Random.Range(0, range);

    //     while (index == previousIndex)
    //     {
    //         index = Random.Range(0, range);
    //     }
    //     return index;
    // }
}

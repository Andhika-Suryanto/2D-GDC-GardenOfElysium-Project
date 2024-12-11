using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class Music_Mixer : MonoBehaviour
{
    public AudioMixer Mixer;
    public void SetVolume(float volume)
    {
        Mixer.SetFloat("Volume", volume);
    }
}

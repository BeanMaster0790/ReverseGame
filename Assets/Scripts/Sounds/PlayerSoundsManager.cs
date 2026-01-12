using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSoundsManager : MonoBehaviour
{
    [SerializeField] private List<Sound> _sounds;

    public static PlayerSoundsManager Current;

    void Start()
    {
        PlayerSoundsManager.Current = this;
    }

    public bool PlaySound(string key)
    {
        Sound s = this._sounds.FirstOrDefault(p => p.Key.ToLower() == key.ToLower());

        if(s != null)
        {
            s.AudioSource.Play();

            return true;
        }

        return false;
    }
}

[Serializable]
public class Sound
{
    public string Key;
    public AudioSource AudioSource;
}

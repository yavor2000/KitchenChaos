using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour, IGameService
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    
    private AudioSource _audioSource;
    private ServiceLocator _serviceLocator;
    private float _volume = .3f;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);

        _audioSource = GetComponent<AudioSource>();
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        _audioSource.volume = _volume;
    }

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 0f;
        }

        _audioSource.volume = _volume;
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _volume;
    }

    private void OnDestroy()
    {
        _serviceLocator.Unregister<MusicManager>();
    }
}

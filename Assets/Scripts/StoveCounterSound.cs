using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource _audioSource;
    private ServiceLocator _serviceLocator;
    private SoundManager _soundManager;
    private float _warningSoundTimer;
    private bool _playWaringSound;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_playWaringSound)
        {
            _warningSoundTimer -= Time.deltaTime;
            if (_warningSoundTimer <= 0)
            {
                float warningSoundTimerMax = .2f;
                _warningSoundTimer = warningSoundTimerMax;
                _soundManager.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }

    private void Start()
    {
        _soundManager = _serviceLocator.Get<SoundManager>();
        
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        _playWaringSound = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount; 
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        switch (e.State)
        {
            default:
            case StoveCounter.State.Idle:
            case StoveCounter.State.Burned:
                _audioSource.Stop();
                break;
            case StoveCounter.State.Frying:
            case StoveCounter.State.Fried:
                _audioSource.volume = _soundManager.GetVolume();
                _audioSource.Play();
                break;
        }
    }
}

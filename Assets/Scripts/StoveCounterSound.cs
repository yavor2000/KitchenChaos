using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
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
                _audioSource.Play();
                break;
        }
    }
}

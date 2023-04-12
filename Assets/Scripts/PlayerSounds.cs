using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private SoundManager _soundManager;
    private Player _player;
    private float footstepTimer;
    private float footstepTimerMax = .1f;

    [SerializeField] private float footstepsVolume = 1f;
    

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _soundManager = _serviceLocator.Get<SoundManager>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;
            if (_player.IsWalking)
            {
                _soundManager.PlayFootstepsSound(_player.transform.position, footstepsVolume);
            }
        }
    }
}
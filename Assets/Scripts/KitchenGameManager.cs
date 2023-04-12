using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour, IGameService
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State State { get; set; }
    }

    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private ServiceLocator _serviceLocator;
    private Player _player;
    private SoundManager _soundManager;
    
    private State _state;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer = 10f;

    public State GameState
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                {
                    State = _state
                });
            }
        }
    }

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);
        GameState = State.WaitingToStart;
    }

    private void Start()
    {
        _player = _serviceLocator.Get<Player>();
        _soundManager = _serviceLocator.Get<SoundManager>();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0f)
                {
                    GameState = State.CountdownToStart;
                }
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0f)
                {
                    GameState = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0f)
                {
                    GameState = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }
}

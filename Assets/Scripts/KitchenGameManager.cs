using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour, IGameService
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

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
    private GameInput _gameInput;
    
    private State _state;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer = 0f;
    private float _gamePlayingTimerMax = 240f;
    private bool _isGamePaused = false;

    public bool IsGamePaused
    {
        get => _isGamePaused;
        set
        {
            if (_isGamePaused != value)
            {
                _isGamePaused = value;
            }

            if (_isGamePaused)
            {
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnGameUnPaused?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
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
        _gameInput = _serviceLocator.Get<GameInput>();

        _gameInput.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
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
                    _gamePlayingTimer = _gamePlayingTimerMax;
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

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        IsGamePaused = !IsGamePaused;
        Time.timeScale = _isGamePaused ? 0f : 1f;
    }
    
    public void TogglePauseGame(bool isPaused)
    {
        IsGamePaused = isPaused;
        Time.timeScale = _isGamePaused ? 0f : 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        _serviceLocator.Unregister<KitchenGameManager>();
        _gameInput.OnPauseAction -= GameInput_OnPauseAction;
    }
}

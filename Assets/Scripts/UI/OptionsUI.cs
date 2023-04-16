using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour, IGameService
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicEffectsText;
    

    private ServiceLocator _serviceLocator;
    private KitchenGameManager _gameManager;
    private SoundManager _soundManager;
    private MusicManager _musicManager;

    private float _minVolume = 0f;
    private float _maxVolume = 10f;
    
    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);
    }

    private void Start()
    {
        _soundManager = _serviceLocator.Get<SoundManager>();
        _musicManager = _serviceLocator.Get<MusicManager>();
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        
        _gameManager.OnGameUnPaused += GameManager_OnGameUnPaused;
        
        soundEffectsButton.onClick.AddListener(() =>
        {
            _soundManager.ChangeVolume();
            UpdateVisual();
        });
        
        musicButton.onClick.AddListener(() =>
        {
            _musicManager.ChangeVolume();
            UpdateVisual();
        });
        
        closeButton.onClick.AddListener(() =>
        {
            if (_gameManager != null)
            {
                _gameManager.TogglePauseGame(false);
            }

            Hide();
        });
        
        Hide();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        int volumeEffectsUI = Mathf.RoundToInt(Mathf.Lerp(_minVolume, _maxVolume, _soundManager.GetVolume()));
        int volumeMusicUI = Mathf.RoundToInt(Mathf.Lerp(_minVolume, _maxVolume, _musicManager.GetVolume()));
        soundEffectsText.text = $"Sound Effects: {volumeEffectsUI}";
        musicEffectsText.text = $"Music: {volumeMusicUI}";
    }

    private void OnDestroy()
    {
        _serviceLocator?.Unregister<OptionsUI>();
    }

    public void Show()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }
    
    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }
}

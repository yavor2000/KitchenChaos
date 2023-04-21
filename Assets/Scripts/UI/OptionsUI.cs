using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour, IGameService
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicEffectsText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Transform pressToRebindTransform;

    private Action _onCloseButtonAction;
    private ServiceLocator _serviceLocator;
    private KitchenGameManager _gameManager;
    private SoundManager _soundManager;
    private MusicManager _musicManager;
    private GameInput _gameInput;

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
        _gameInput = _serviceLocator.Get<GameInput>();
        
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
            _onCloseButtonAction();
        });
        
        moveUpButton.onClick.AddListener(() =>{ RebindBinding(GameInput.Binding.Move_Up); });
        
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        
        interactAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
                
        gamepadInteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlt); });
                
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
                
        Hide();
        HidePressToRebindKey();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        int volumeEffectsUI = Mathf.RoundToInt(Mathf.Lerp(_minVolume, _maxVolume, _soundManager.GetVolume()));
        int volumeMusicUI = Mathf.RoundToInt(Mathf.Lerp(_minVolume, _maxVolume, _musicManager.GetVolume()));
        soundEffectsText.text = $"Sound Effects: {volumeEffectsUI}";
        musicEffectsText.text = $"Music: {volumeMusicUI}";
        moveUpText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Up));
        moveDownText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Down));
        moveLeftText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Left));
        moveRightText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Right));
        interactText.text = _gameInput.GetBindingText((GameInput.Binding.Interact));
        interactAltText.text = _gameInput.GetBindingText((GameInput.Binding.InteractAlternate));
        pauseText.text = _gameInput.GetBindingText((GameInput.Binding.Pause));
        gamepadInteractText.text = _gameInput.GetBindingText((GameInput.Binding.Gamepad_Interact));
        gamepadInteractAltText.text = _gameInput.GetBindingText((GameInput.Binding.Gamepad_InteractAlt));
        gamepadPauseText.text = _gameInput.GetBindingText((GameInput.Binding.Gamepad_Pause));
    }

    private void OnDestroy()
    {
        _serviceLocator?.Unregister<OptionsUI>();
    }

    public void Show(Action onCloseButtonAction)
    {
        _onCloseButtonAction = onCloseButtonAction;
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        soundEffectsButton.Select();
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

    private void ShowPressToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(true);
    }
    
    private void HidePressToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        if (_gameInput == null) return;

        ShowPressToRebindKey();
        _gameInput.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public class GamePauseUI : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private KitchenGameManager _gameManager;
    private OptionsUI _optionsUI;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button resumeButton;
    

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
        resumeButton.onClick.AddListener(() =>
        {
            _gameManager.TogglePauseGame();
        });
        
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            _optionsUI.Show(Show);
        });
    }

    private void Start()
    {
        _optionsUI = _serviceLocator.Get<OptionsUI>();
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        _gameManager.OnGamePaused += GameManager_OnGamePaused;
        _gameManager.OnGameUnPaused += GameManager_OnGameUnPaused;
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }
    
    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameManager.OnGamePaused -= GameManager_OnGamePaused;
        _gameManager.OnGameUnPaused -= GameManager_OnGameUnPaused;
    }
}
}

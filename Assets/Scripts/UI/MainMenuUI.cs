using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    // private ServiceLocator _serviceLocator;
    // private KitchenGameManager _gameManager;
    
    private void Awake()
    {
        // ServiceLocator.Initiailze();
        // _serviceLocator = ServiceLocator.Current;
        
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    // private void Start()
    // {
    //     _gameManager = _serviceLocator.Get<KitchenGameManager>();
    //     _gameManager.TogglePauseGame(false);
    // }
}
}

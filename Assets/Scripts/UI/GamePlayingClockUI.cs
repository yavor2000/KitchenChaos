using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private ServiceLocator _serviceLocator;
    private KitchenGameManager _gameManager;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    private void Start()
    {
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
    }

    private void Update()
    {
        timerImage.fillAmount = _gameManager.GetGamePlayingTimerNormalized();
    }
}

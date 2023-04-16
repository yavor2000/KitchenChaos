using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private ServiceLocator _serviceLocator;
    private KitchenGameManager _gameManager;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    private void Start()
    {
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        _gameManager.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        if (countdownText.gameObject.activeInHierarchy)
        {
            countdownText.text = Mathf.Ceil(_gameManager.GetCountdownToStartTimer()).ToString(CultureInfo.CurrentCulture);
        }
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (e != null && ((KitchenGameManager.OnStateChangedEventArgs) e).State ==
            KitchenGameManager.State.CountdownToStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        countdownText.gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }
}
}

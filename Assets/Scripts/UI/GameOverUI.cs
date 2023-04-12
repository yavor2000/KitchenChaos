using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    
    private ServiceLocator _serviceLocator;
    private KitchenGameManager _gameManager;
    private DeliveryManager _deliveryManager;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    private void Start()
    {
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        _deliveryManager = _serviceLocator.Get<DeliveryManager>();
        _gameManager.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    // private void Update()
    // {
    //     if (recipesDeliveredText.gameObject.active)
    //     {
    //         recipesDeliveredText.text = Mathf.Ceil(_gameManager.GetCountdownToStartTimer()).ToString(CultureInfo.CurrentCulture);
    //     }
    // }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (e != null && ((KitchenGameManager.OnStateChangedEventArgs) e).State ==
            KitchenGameManager.State.GameOver)
        {
            Show();
            recipesDeliveredText.text = _deliveryManager.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
}

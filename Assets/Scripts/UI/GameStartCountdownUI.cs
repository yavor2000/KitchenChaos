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
    private SoundManager _soundManager;
    private Animator _animator;
    private int _previousCountdownNumber;
    private static readonly int NUMBER_POPUP = Animator.StringToHash("NumberPopup");

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        _gameManager.OnStateChanged += GameManager_OnStateChanged;
        _soundManager = _serviceLocator.Get<SoundManager>();
        Hide();
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(_gameManager.GetCountdownToStartTimer());
        if (countdownText.gameObject.activeInHierarchy)
        {
            countdownText.text = countdownNumber.ToString(CultureInfo.CurrentCulture);
        }

        if (_previousCountdownNumber != countdownNumber)
        {
            _previousCountdownNumber = countdownNumber;
            _animator.SetTrigger(NUMBER_POPUP);
            _soundManager.PlayCountdownSound();
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

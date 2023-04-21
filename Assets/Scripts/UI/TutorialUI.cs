using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAltText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    private ServiceLocator _serviceLocator;
    private GameInput _gameInput;
    private KitchenGameManager _gameManager;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    private void Start()
    {
        _gameInput = _serviceLocator.Get<GameInput>();
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        _gameInput.OnBindRebindAction += GameInput_OnBindRebindAction;
        _gameManager.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, KitchenGameManager.OnStateChangedEventArgs e)
    {
        if (e.State is KitchenGameManager.State.CountdownToStart or KitchenGameManager.State.GamePlaying)
        {
            Hide();
        }
    }

    private void GameInput_OnBindRebindAction(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Up));
        keyMoveDownText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Down));
        keyMoveLeftText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Left));
        keyMoveRightText.text = _gameInput.GetBindingText((GameInput.Binding.Move_Right));
        keyInteractText.text = _gameInput.GetBindingText((GameInput.Binding.Interact));
        keyInteractAltText.text = _gameInput.GetBindingText((GameInput.Binding.InteractAlternate));
        keyPauseText.text = _gameInput.GetBindingText((GameInput.Binding.Pause));
        keyGamepadInteractText.text = _gameInput.GetBindingText((GameInput.Binding.Gamepad_Interact));
        keyGamepadInteractAltText.text = _gameInput.GetBindingText((GameInput.Binding.Gamepad_InteractAlt));
        keyGamepadPauseText.text = _gameInput.GetBindingText((GameInput.Binding.Gamepad_Pause));
    }

    private void Show()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

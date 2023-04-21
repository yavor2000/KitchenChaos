using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";
    
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;
    
    

    private ServiceLocator _serviceLocator;
    private DeliveryManager _deliveryManager;
    private Animator _animator;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _deliveryManager = _serviceLocator.Get<DeliveryManager>();

        _deliveryManager.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        _deliveryManager.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(POPUP);
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(POPUP);
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";
    }
}

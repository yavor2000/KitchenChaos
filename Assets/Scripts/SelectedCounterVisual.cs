using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private Player _player;

    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;
    
    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    private void Start()
    {
        _player = _serviceLocator.Get<Player>();
        Debug.Log("Selected counter vis, player is " + _player);
        _player.OnSelectedCounterChanged += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        ClearCounter cc = e.selectedCounter;
        // Debug.Log($"{DateTime.Now} counter changed to {(cc != null ? cc.transform.position.ToString() : "null")}");
        if (e.selectedCounter == clearCounter)
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
        visualGameObject.SetActive(true);
    }
    
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}

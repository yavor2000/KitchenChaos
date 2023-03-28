using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private ServiceLocator _serviceLocator;
    private Player _player;
    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
    }

    private void Start()
    {
        _player = _serviceLocator.Get<Player>();
        Debug.Log("Selected counter vis, player " + _player);
        _player.OnSelectedCounterChanged += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        Debug.Log($"Player_OnSelectedCounterChange, sender {sender}, to {e.selectedCounter}");
    }
}

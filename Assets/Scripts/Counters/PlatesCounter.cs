using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;


    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _platesSpawnedAmount;
    private int _platesSpawnedAmountmax = 4;

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;
            if (_platesSpawnedAmount < _platesSpawnedAmountmax)
            {
                _platesSpawnedAmount++;
                // KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this);
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        { // Player is empty handed
            if (_platesSpawnedAmount > 0)
            { // and there are plates
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                _platesSpawnedAmount--;
            }
        }
        else
        {
            
        }
    }
}
}
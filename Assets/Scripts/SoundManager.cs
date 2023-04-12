using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour, IGameService
{
    private ServiceLocator _serviceLocator;
    private DeliveryManager _deliveryManager;
    private DeliveryCounter _deliveryCounter;
    private Player _player;

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    

    private void Awake()
    {
        Debug.Log("SoundManager awake");
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register<SoundManager>(this);
    }

    private void Start()
    {
        Debug.Log("SoundManager start");
        _deliveryManager = _serviceLocator.Get<DeliveryManager>();
        _deliveryCounter = _serviceLocator.Get<DeliveryCounter>();
        _player = _serviceLocator.Get<Player>();

        _deliveryManager.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        _deliveryManager.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        _player.OnPickedSomething += Player_OnPickedSomething;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter tc = sender as TrashCounter;
        Vector3 pos = tc != null ? tc.transform.position : _player.transform.position;
        PlaySound(audioClipRefsSO.trash, pos);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter bc = sender as BaseCounter;
        Vector3 pos = bc != null ? bc.transform.position : _player.transform.position;
        PlaySound(audioClipRefsSO.objectDrop, pos);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, _player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        Vector3 pos = cuttingCounter ? cuttingCounter.transform.position : _player.transform.position;
        PlaySound(audioClipRefsSO.chop, pos);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        // PlaySound(audioClipRefsSO.deliverySuccess, Camera.main.transform.position);
        PlaySound(audioClipRefsSO.deliverySuccess, _deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, _deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}

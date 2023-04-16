using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour, IGameService
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    
    private ServiceLocator _serviceLocator;
    private DeliveryManager _deliveryManager;
    private DeliveryCounter _deliveryCounter;
    private Player _player;
    private float _volume = 1f;

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;


    private void Awake()
    {
        Debug.Log("SoundManager awake");
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);

        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _volume);
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

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplayer = 1f)
    {
        Debug.Log($"play with {_volume} * {volumeMultiplayer}");
        AudioSource.PlayClipAtPoint(audioClip, position, _volume * volumeMultiplayer);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplayer = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position,
            _volume * volumeMultiplayer);
    }

    public void PlayFootstepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.footstep, position, _volume * volume);
    }

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 0f;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _volume;
    }
    
    private void OnDestroy()
    {
        _serviceLocator.Unregister<SoundManager>();
    }
}

using System;
using Counters;
using UnityEngine;

namespace UI
{
public class StoveFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    
    [SerializeField] private StoveCounter stoveCounter;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        _animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        Debug.Log(e.ProgressNormalized);
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
        _animator.SetBool(IS_FLASHING, show);
    }
}
}

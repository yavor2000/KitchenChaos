using System;
using Counters;
using UnityEngine;

namespace UI
{
public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;


    private void Start()
    {
        Hide();
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        Debug.Log(e.ProgressNormalized);
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
        if (show)
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
}

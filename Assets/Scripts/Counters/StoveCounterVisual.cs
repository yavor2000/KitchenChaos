using System;
using UnityEngine;

namespace Counters
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] private GameObject stoveOnGameObject;
        [SerializeField] private GameObject particlesGameObject;
        [SerializeField] private StoveCounter stoveCounter;

        private void Start()
        {
            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            switch (e.state)
            {
                case StoveCounter.State.Idle:
                case StoveCounter.State.Burned:
                    Hide();
                    break;
                case StoveCounter.State.Frying:
                case StoveCounter.State.Fried:
                    Show();
                    break;
            }
        }

        private void Show()
        {
            stoveOnGameObject.SetActive(true);
            particlesGameObject.SetActive(true);
        }
        
        private void Hide()
        {
            stoveOnGameObject.SetActive(false);
            particlesGameObject.SetActive(false);
        }
    }
}

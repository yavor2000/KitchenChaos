using System;
using UnityEngine;

namespace Counters
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        [SerializeField] private CuttingCounter cuttingCounter;
    
        private Animator _animator;
        private static readonly int Cut = Animator.StringToHash("Cut");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            // Debug.Log("Awake anim is " + _animator);
        }

        private void Start()
        {
            cuttingCounter.OnCut += CuttingCounter_OnCut;
        }

        private void CuttingCounter_OnCut(object sender, EventArgs e)
        {
            // Debug.Log("anim to call is " + _animator);
            _animator.SetTrigger(Cut);
        }
    }
}

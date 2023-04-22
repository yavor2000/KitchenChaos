using System;
using TMPro;
using UnityEngine;

namespace UI
{
public class DiagnosticUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private float _pollingTime = 1f;
    private float _time;
    private int _frameCount;

    private void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;

        if (_time >= _pollingTime)
        {
            int frameCount = Mathf.RoundToInt(_frameCount / _time); 
            fpsText.text = frameCount.ToString() + " fps";
            _time -= _pollingTime;
            _frameCount = 0;
        }
    }
}
}

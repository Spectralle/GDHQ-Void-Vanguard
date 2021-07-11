using System.Collections;
using UnityEngine;

public class ConclusionFlashEffect : MonoBehaviour
{
    [SerializeField] private float _flickerDelay = 1;
    [SerializeField] private CanvasGroup _flickerGroup;

    private bool _isFlickering;


    private void Start()
    {
        if (_flickerGroup)
            StartFlicker();
        else
            enabled = false;
    }

    public void StartFlicker()
    {
        _isFlickering = true;
        StartCoroutine(ContinuousFlicker());
    }

    public void StopFlicker() => _isFlickering = false;

    private IEnumerator ContinuousFlicker()
    {
        while (_isFlickering)
        {
            yield return new WaitForSecondsRealtime(_flickerDelay);
            _flickerGroup.alpha = 0.3f;
            yield return new WaitForSecondsRealtime(_flickerDelay);
            _flickerGroup.alpha = 1;
        }
    }
}

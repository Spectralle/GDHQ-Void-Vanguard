using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerPostProcessEffects : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _ppVolume;
    [SerializeField, Range(0.1f, 4f)] private float defaultFadeSpeed = 1.5f;

    private Vignette vignette;


    private void Start() => _ppVolume?.sharedProfile.TryGetSettings<Vignette>(out vignette);

    #region Blindness
    public void EnableBlindness(float duration) => StartCoroutine(EnableBlindEffect(duration, 0.7f, defaultFadeSpeed));
    public void EnableBlindness(float duration, float intensity, float fadeSpeed) => StartCoroutine(EnableBlindEffect(duration, intensity, fadeSpeed));

    private IEnumerator EnableBlindEffect(float duration, float intensity, float fadeSpeed)
    {
        StopCoroutine(DisableBlindEffect(fadeSpeed));
        StopCoroutine(MaintainBlindness(duration, fadeSpeed));

        while (vignette.intensity.value < intensity)
        {
            vignette.intensity.value += (Time.deltaTime * fadeSpeed);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MaintainBlindness(duration, fadeSpeed));
    }

    private IEnumerator DisableBlindEffect(float fadeSpeed)
    {
        while (vignette.intensity.value > 0)
        {
            vignette.intensity.value -= (Time.deltaTime * fadeSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MaintainBlindness(float duration, float fadeSpeed)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(DisableBlindEffect(fadeSpeed));
    }
    #endregion


    private void OnDisable()
    {
        if (vignette)
            vignette.intensity.value = 0;
    }
}

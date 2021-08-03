using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIHider : MonoBehaviour
{
    [SerializeField] private float _transparency = 0.3f;

    private CanvasGroup _canvasGroup;


    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.activeSelf)
            StartCoroutine(ChangeUICanvasAlpha(_transparency));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.activeSelf)
            StartCoroutine(ChangeUICanvasAlpha(1f));
    }

    private IEnumerator ChangeUICanvasAlpha(float endValue)
    {
        bool isRaising = _canvasGroup.alpha < endValue;

        if (isRaising)
        {
            while (_canvasGroup.alpha < endValue)
            {
                _canvasGroup.alpha += (Time.deltaTime * 3f);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_canvasGroup.alpha > endValue)
            {
                _canvasGroup.alpha -= (Time.deltaTime * 3f);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

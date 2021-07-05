using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip _explosionAudioClip;

    private void Awake()
    {
        TryGetComponent(out AudioSource _asrc);
        _asrc.clip = _explosionAudioClip;
        _asrc.Play();
        Destroy(gameObject, 3f);
    }
}
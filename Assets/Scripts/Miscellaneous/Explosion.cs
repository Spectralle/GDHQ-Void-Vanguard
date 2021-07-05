using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip _explosionAudioClip;

    private void Awake()
    {
        AudioSource src = GetComponent<AudioSource>();
        src.clip = _explosionAudioClip;
        src.Play();
        Destroy(gameObject, 3f);
    }
}
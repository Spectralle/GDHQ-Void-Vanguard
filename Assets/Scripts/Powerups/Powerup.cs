using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupType _type;
    [SerializeField] private int _fallSpeed = 3;
    [SerializeField] private int _duration = 5;
    [SerializeField] private AudioClip _powerupAudioClip;


    private void Update()
    {
        transform.Translate(Vector3.down * _fallSpeed * Time.deltaTime);

        if (transform.position.y < LevelBoundary.D(-2))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out PlayerPowerup plPu);
            if (plPu)
            {
                GetComponent<AudioSource>().PlayOneShot(_powerupAudioClip);
                plPu.ActivatePowerup(_type, _duration, _powerupAudioClip);
            }
            SpawnManager.PowerupsInLevel--;
            Destroy(gameObject);
        }
    }
}

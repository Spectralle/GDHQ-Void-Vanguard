using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionAudioClip;


    private void Update() => transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
            StartCoroutine(Explode(other.gameObject));
    }

    private IEnumerator Explode(GameObject other)
    {
        GameObject explosion = Instantiate(
            _explosionPrefab,
            transform.position,
            Quaternion.identity,
            GameObject.Find("Game Handler/Scene").transform
        );

        GetComponent<AudioSource>().PlayOneShot(_explosionAudioClip, 1.2f);

        Destroy(explosion, 2.55f);
        Destroy(other.gameObject);

        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2.6f);

        SpawnManager.StartSpawning();

        Destroy(gameObject);
    }
}

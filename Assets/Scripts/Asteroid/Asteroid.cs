using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 15;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _text;


    private void Awake()
    {
        int randSpin = Random.Range(0, 100);
        if (randSpin < 50)
            _rotationSpeed = -_rotationSpeed;
    }

    private void Update() => transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile"))
        {
            Destroy(other.gameObject);
            CameraShaker.StartShaking(1.2f, .5f);
            Destroy(_text);
            StartCoroutine(Explode());
        }
    }

    public void MakeExplode() => StartCoroutine(Explode());

    private IEnumerator Explode()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameObject.Find("Game Handler/Scene").transform);

        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2.6f);

        SpawnManager.StartSpawning();

        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 15;
    [SerializeField] private GameObject _explosionPrefab;


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
            StartCoroutine(Explode(other.gameObject));
    }

    private IEnumerator Explode(GameObject other)
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameObject.Find("Game Handler/Scene").transform);

        Destroy(other.gameObject);

        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2.6f);

        SpawnManager.StartSpawning();

        Destroy(gameObject);
    }
}

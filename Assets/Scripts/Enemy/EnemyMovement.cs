using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private AudioClip _explosionAudioClip;

    private Animator _anim;
    private bool _isDestroyed;
    public bool IsDestroyed;


    private void Awake()
    {
        TryGetComponent(out _anim);
        if (!_anim)
            Debug.LogError("No animator on enemy!");
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (!_isDestroyed && transform.position.y < LevelBoundary.D(-2))
        {
            if (SpawnManager.CanSpawn)
                transform.position = SpawnManager.GetSpawnPosition();
            else
                Destroy(gameObject);
        }
    }

    private void OnDestroy() => SpawnManager.EnemiesAlive--;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile"))
        {
            UIManager.i.AddToEnemiesKilled(1);
            UIManager.i.AddToScore(10);
            Destroy(other.gameObject);
            StartCoroutine(Explode());
        }

        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerHealth playerHealth);
            if (playerHealth)
                playerHealth.Damage(1);
            StartCoroutine(Explode());
        }

        if (other.CompareTag("Player Shield"))
        {
            other.GetComponentInParent<PlayerShield>().StopAllCoroutines();
            other.gameObject.SetActive(false);
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        _isDestroyed = true;

        if (_anim)
            _anim.SetTrigger("OnEnemyDeath");

        GetComponent<AudioSource>().PlayOneShot(_explosionAudioClip, 0.9f);

        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2.5f);

        while (_moveSpeed > 0.00f)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, 0, Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private AudioClip _explosionAudioClip;

    private Animator _anim;
    private bool _isDestroyed;
    public bool IsDestroyed => _isDestroyed;


    private void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);

        if (!_isDestroyed && transform.position.y < LevelBoundary.D(-2))
        {
            if (SpawnManager.i.CanSpawn)
                transform.position = SpawnManager.GetSpawnPosition();
            else
                Destroy(gameObject);
        }
    }

    private void OnDestroy() => SpawnManager.i.EnemiesAlive--;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile"))
        {
            UIManager.i.ChangeKills(1);
            UIManager.i.ChangeScore(10);
            Destroy(other.gameObject);
            StartCoroutine(Explode());
        }

        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerHealth playerHealth);
            if (playerHealth)
                playerHealth.Damage();
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        _isDestroyed = true;

        TryGetComponent(out _anim);
        if (_anim)
            _anim.SetTrigger("OnEnemyDeath");

        TryGetComponent(out AudioSource _asrc);
        if (_asrc)
            _asrc.PlayOneShot(_explosionAudioClip, 0.9f);

        TryGetComponent(out Collider2D _c2d);
        if (_c2d)
            _c2d.enabled = false;

        Destroy(gameObject, 2.5f);

        while (_moveSpeed > 0.00f)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, 0, Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
}

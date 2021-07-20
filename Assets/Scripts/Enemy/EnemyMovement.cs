using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField, Range(0, 10)] private float _evadeChance = 3;
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
            {
                StopCoroutine(Evade());
                transform.position = SpawnManager.GetSpawnPosition();
            }
            else
                Destroy(gameObject);
        }
    }

    #region Collide
    private void OnDestroy() => SpawnManager.i.EnemiesAlive--;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile"))
        {
            int evadeChance = Random.Range(1, 10);
            if (evadeChance <= _evadeChance)
                StartCoroutine(Evade());
            else
            {
                Destroy(other.gameObject);
                StartCoroutine(Explode());
            }
        }

        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerHealth playerHealth);
            if (playerHealth)
                playerHealth.Damage();
            StartCoroutine(Explode());
        }
    }
    #endregion

    #region Evade
    private IEnumerator Evade()
    {
        float X = 2.5f;
        int leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0)
            X = -X;

        float relativeX = transform.position.x + X;

        if (relativeX < LevelBoundary.L(2) || relativeX > LevelBoundary.R(2))
        {
            X = -X;
            relativeX = transform.position.x + X;
        }

        while (transform.position.x != X)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector2(relativeX, transform.position.y), Time.deltaTime * 4.5f);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Explode
    public void MakeExplode() => StartCoroutine(Explode());

    private IEnumerator Explode()
    {
        if (_isDestroyed)
            yield return null;

        _isDestroyed = true;

        UIManager.i.ChangeKills(1);
        UIManager.i.ChangeScore(10);

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
    #endregion
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField, Range(0, 10)] private float _evadeChance = 3;
    [SerializeField, Range(0, 2)] private float _sineMoveScale = 0.5f;
    [SerializeField] private AudioClip _explosionAudioClip;

    private Animator _anim;
    private EnemyGun _gun;
    private bool _isDestroyed;
    public bool IsDestroyed => _isDestroyed;
    private float _originX = 0;
    private bool _canSine = true;
    private Vector3 _baseMovementDirection = Vector3.down;
    private int _damageAmount = 1;
    private bool _isShootingAsteroid;


    private void Awake()
    {
        TryGetComponent(out _gun);
        SetOriginX(transform.position.x);
    }

    private void SetOriginX(float X) => _originX = X;
    public void SetMovementDirection(Vector3 dir) => _baseMovementDirection = dir;
    public void SetDamageAmount(int amount) => _damageAmount = amount;
    public void SetAsAsteroid() => _isShootingAsteroid = true;

    private void Update()
    {
        if (_canSine && _sineMoveScale > 0 && _moveSpeed > 0)
        {
            float X = _originX + (Mathf.Sin(transform.position.y) * _sineMoveScale);
            transform.position = new Vector3(X, transform.position.y);
        }

        transform.Translate(_baseMovementDirection * _moveSpeed * Time.deltaTime);

        if (!_isDestroyed && transform.position.y < LevelBoundary.D(-2))
        {
            if (SpawnManager.i.CanSpawn && !_isShootingAsteroid)
            {
                StopCoroutine(Evade());
                transform.position = SpawnManager.GetEnemySpawnPosition();
                SetOriginX(transform.position.x);
                if (_gun.AntiPowerupType == EnemyGun.APType.Advanced)
                    _gun.ShootAdvAntiItemLaser();
            }
            else
                Destroy(gameObject);
        }
    }

    #region Collide
    private void OnDestroy() => SpawnManager.ChangeEnemiesAlive(transform);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Projectile") && !_isShootingAsteroid)
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
                playerHealth.Damage(_damageAmount);
            StartCoroutine(Explode());
        }
    }
    #endregion

    #region Evade
    private IEnumerator Evade()
    {
        _canSine = false;

        float X = 2.5f;
        int leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0)
            X = -X;

        _originX = _originX + X;

        if (_originX < LevelBoundary.L(2) || _originX > LevelBoundary.R(2))
        {
            X = -X;
            _originX = transform.position.x + X;
        }

        while (transform.position.x != X)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector2(_originX, transform.position.y), Time.deltaTime * 4.5f);
            yield return new WaitForEndOfFrame();
        }

        _canSine = true;
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

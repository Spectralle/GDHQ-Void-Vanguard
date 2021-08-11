using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField, Range(0, 10)] private float _evadeChance = 3;
    [SerializeField, Range(0, 10)] private float _evadeDistance = 2.5f;
    [SerializeField, Range(0, 2)] private float _sineXMoveScale = 0.5f;
    [SerializeField, Range(0, 2)] private float _sineYMoveScale = 0.5f;
    [SerializeField] private AudioClip _explosionAudioClip;

    private Animator _anim;
    private EnemyGun _gun;
    private bool _isDestroyed;
    public bool IsDestroyed => _isDestroyed;
    private Vector2 _origin = Vector2.zero;
    private Vector3 _baseMovementDirection = Vector3.down;
    private int _damageAmount = 1;
    private bool _isShootingAsteroid;
    private int _sineDirection = 1;
    private int _randomSineStarter;


    private void Awake()
    {
        TryGetComponent(out _gun);
        SetOrigin(transform.position.x, transform.position.y);

        if (Random.Range(0, 2) == 0)
            _sineDirection = -1;

        _randomSineStarter = Random.Range(1, 500);
    }

    private void SetOrigin(float X, float Y) => _origin = new Vector2(X, Y);

    public void SetMovementDirection(Vector3 dir) => _baseMovementDirection = dir;
    public void SetDamageAmount(int amount) => _damageAmount = amount;
    public void SetAsAsteroid() => _isShootingAsteroid = true;

    private void Update()
    {
        if (!_isShootingAsteroid)
        {
            _origin += (Vector2)_baseMovementDirection * _moveSpeed * Time.deltaTime;

            float X = 0;
            if (_sineXMoveScale > 0)
                X = Mathf.Sin((Time.timeSinceLevelLoad + _randomSineStarter) * _sineDirection * 5) * _sineXMoveScale;
            float Y = 0;
            if (_sineYMoveScale > 0)
                Y = Mathf.Cos((Time.timeSinceLevelLoad + _randomSineStarter) * _sineDirection * 5) * _sineYMoveScale;

            transform.position = _origin + new Vector2(X, Y);
        }
        else
        {
            transform.Translate(_baseMovementDirection * _moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward * 4 * Time.deltaTime);
        }

        CheckForReposition();
    }

    private void CheckForReposition()
    {
        if (!_isDestroyed && transform.position.y < LevelBoundary.D(-2))
        {
            if (SpawnManager.i.CanSpawn && !_isShootingAsteroid)
            {
                StopCoroutine(Evade());
                transform.position = SpawnManager.GetEnemySpawnPosition();
                SetOrigin(transform.position.x, transform.position.y);
                if (_gun.AILaserType == EnemyGun.AILType.Advanced)
                    _gun.ShootAdvAntiItemLaser();
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!_isShootingAsteroid && SpawnManager.EnemyList.Contains(transform))
            SpawnManager.EnemyList.Remove(transform);
    }

    #region Collide
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
        float _sineX = _sineXMoveScale;
        _sineXMoveScale = 0;
        float _sineY = _sineYMoveScale;
        _sineYMoveScale = 0;

        float X = _evadeDistance;
        int leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0)
            X = -X;

        _origin.x += X;

        if (_origin.x < LevelBoundary.L(2) || _origin.x > LevelBoundary.R(2))
        {
            X = -X;
            _origin.x = transform.position.x + X;
        }

        while (leftOrRight == 0 ? transform.position.x > X : transform.position.x < X)
        {
            transform.position = Vector3.Lerp(transform.position, _origin, Time.deltaTime * 4.5f);
            yield return new WaitForEndOfFrame();
        }

        _sineXMoveScale = _sineX;
        _sineYMoveScale = _sineY;
    }
    #endregion

    #region Explode
    public void MakeExplode() => StartCoroutine(Explode());

    private IEnumerator Explode()
    {
        if (_isDestroyed || _isShootingAsteroid)
            yield break;

        _isDestroyed = true;

        SpawnManager.ChangeEnemiesAlive(transform);

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
            _sineXMoveScale = Mathf.Lerp(_sineXMoveScale, 0, Time.deltaTime * 1.5f);
            _sineYMoveScale = Mathf.Lerp(_sineYMoveScale, 0, Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
